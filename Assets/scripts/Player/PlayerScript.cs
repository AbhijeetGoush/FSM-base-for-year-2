using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{


    public class PlayerScript : MonoBehaviour
    {
        public Rigidbody2D rb;
        public BoxCollider2D boxCol;
        public Animator anim;
        SpriteRenderer spriteRenderer;
        HelperScript helper;
        private string currentAnimState;
        public float xv, yv;
        public float runSpeed = 6;
        public float jumpSpeed = 6;
        public int playerHealth = 3;
        public bool grounded;
        float jumpGravity = 1;

        // variables holding the different player states
        public StandingState standingState;
        public RunningState runningState;
        public JumpingState jumpingState;
        public FallingState fallingState;
        public DeathState deathState;
        public StateMachine sm;

        const string ARTHUR_RUNNING = "arthur_run";
        const string ARTHUR_STAND = "arthur_stand";
        const string ARTHUR_JUMP = "arthur_jump";
        const string ARTHUR_FALL = "arthur_fall";

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sm = gameObject.AddComponent<StateMachine>();
            boxCol = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            helper = GetComponent<HelperScript>();
            // add new states here
            standingState = new StandingState(this, sm);
            runningState = new RunningState(this, sm);
            jumpingState = new JumpingState(this, sm);
            fallingState = new FallingState(this, sm);
            deathState = new DeathState(this, sm);

            // initialise the statemachine with the default state
            sm.Init(standingState);
        }

        // Update is called once per frame
        public void Update()
        {
            sm.CurrentState.HandleInput();
            sm.CurrentState.LogicUpdate();

            //output debug info to the canvas
            string s;
            s = string.Format("last state={0}\ncurrent state={1}", sm.LastState, sm.CurrentState);
            UIscript.ui.DrawText(s);

            s = string.Format("current xv={0} yv={1}", xv, yv);
            UIscript.ui.DrawText(s);

            s = string.Format("grounded ={0}", grounded);
            UIscript.ui.DrawText(s);

            s = string.Format("health ={0}", playerHealth);
            UIscript.ui.DrawText(s);

            if (Input.GetKeyDown(KeyCode.F))
            {
                playerHealth -= 1;
            }

            if(playerHealth == 0)
            {
                sm.ChangeState(deathState);
            }
        }



        void FixedUpdate()
        {
            sm.CurrentState.PhysicsUpdate();
            rb.velocity = new Vector2(xv, yv);
        }


        public void CheckForRun()
        {
            if (Input.GetKey("left")) // key held down
            {
                runSpeed = -6;
                sm.ChangeState(runningState);
                ChangeAnimationState(ARTHUR_RUNNING);
                helper.FlipObject(true);
                return;
            }

            if (Input.GetKey("right")) // key held down
            {
                runSpeed = 6;
                sm.ChangeState(runningState);
                ChangeAnimationState(ARTHUR_RUNNING);
                helper.FlipObject(false);
            }
        }


        public void CheckForStand()
        {
                if (Input.GetKey("left") == false) // key held down
                {
                    if (Input.GetKey("right") == false) // key held down
                    {
                        sm.ChangeState(standingState);
                        ChangeAnimationState(ARTHUR_STAND);
                }
                }
                 
        }

        public void CheckForJump()
        {
            if (Input.GetKey(KeyCode.Space) && (grounded == true)) // key held down
            {
                jumpSpeed = 40;
                sm.ChangeState(jumpingState);
                ChangeAnimationState(ARTHUR_JUMP);
            }

        }

        public void CheckForLand()
        {
            if(grounded == true)
            {
                sm.ChangeState(standingState);
                ChangeAnimationState(ARTHUR_STAND);
            }
            if(grounded == false)
            {
                ChangeAnimationState(ARTHUR_FALL);
            }
        }

        public void DoJump()
        {
            yv -= jumpGravity;
            if( yv <-12 )
            {
                yv = -12;
            }

        }

        public void DoGravity()
        {
            sm.ChangeState(fallingState);
        }

        public void Death()
        {
            spriteRenderer.enabled = false;
            
        }

        public void OnCollisionStay2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "platform")
            {
                grounded = true;
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "platform")
            {
                grounded = false;
            }
        }

        void ChangeAnimationState(string newAnimState)
        {
            if (currentAnimState == newAnimState) return;

            anim.Play(newAnimState);

            currentAnimState = newAnimState;
        }
    }

}
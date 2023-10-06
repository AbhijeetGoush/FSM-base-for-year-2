using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace Player
{
    public class JumpingState : State
    {
        // constructor
        public JumpingState(PlayerScript player, StateMachine sm) : base(player, sm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.yv = 20;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Debug.Log("yv=" + player.yv);

            if (player.yv <= -1)
            {
                if (player.grounded == true)
                {
                    sm.ChangeState(player.standingState);
                }
                //player.CheckForLand();
            }
        }

        public override void PhysicsUpdate()
        {
            player.DoJump();
            base.PhysicsUpdate();
        }
    }
}

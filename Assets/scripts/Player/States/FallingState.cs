using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace Player
{
    public class FallingState : State
    {
        // constructor
        public FallingState(PlayerScript player, StateMachine sm) : base(player, sm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.yv = -2;

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
            player.CheckForLand();
            
        }

        public override void PhysicsUpdate()
        {
            if (player.grounded == false)
            {
                player.yv -= 1;
                if (player.yv < -12)
                {
                    player.yv = -12;
                }
            }
            base.PhysicsUpdate();
        }
    }
}
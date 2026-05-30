using Agents.FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerShootingState : AbstractPlayerState
    {
        public PlayerShootingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.PlayerInput.OnLeftMousePressedEnd += HandleMousePressedEnd;
        }

        public override void Update()
        {
            base.Update();
            Player.PlayerGunCompo.Fire();
        }

        public override void Exit()
        {
            Player.PlayerInput.OnLeftMousePressedEnd -= HandleMousePressedEnd;
            base.Exit();
        }

        private void HandleMousePressedEnd()
        {
            Player.ChangeState(PlayerStates.IDLE);
        }
    }
}
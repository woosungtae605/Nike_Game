using FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public PlayerIdleState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.PlayerInput.OnLeftMousePressedStart += HandleLeftMousePressedStart;
        }

        public override void Exit()
        {
            Player.PlayerInput.OnLeftMousePressedStart -= HandleLeftMousePressedStart;
            base.Exit();
        }

        private void HandleLeftMousePressedStart()
        {
            Player.ChangeState(PlayerStates.SHOOTING);
        }
    }
}
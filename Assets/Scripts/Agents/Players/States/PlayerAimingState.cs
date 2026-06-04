using Agents.FSM;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerAimingState : AbstractPlayerState
    {
        public PlayerAimingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.GunCursorModule.Active();
            Player.CoverModule.SetHide(false);
            Player.PlayerInputSo.OnLeftMousePressedStart += HandleLeftMousePressedStart;
            Player.PlayerInputSo.OnRightMousePressedEnd += HandleRightMousePressedEnd;
            Bus<CameraZoomEvent>.Raise(new CameraZoomEvent(5, true));
        }

        public override void Exit()
        {
            Bus<CameraZoomEvent>.Raise(new CameraZoomEvent(0, false));
            Player.PlayerInputSo.OnLeftMousePressedStart -= HandleLeftMousePressedStart;
            Player.PlayerInputSo.OnRightMousePressedEnd -= HandleRightMousePressedEnd;
            base.Exit();
        }

        private void HandleRightMousePressedEnd()
        {
            Player.GunCursorModule.UnActive();
            Player.ChangeState(PlayerStates.IDLE);
        }

        private void HandleLeftMousePressedStart()
        {
            Player.ChangeState(PlayerStates.SHOOTING);
        }
    }
}
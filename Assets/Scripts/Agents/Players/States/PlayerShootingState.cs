using Agents.FSM;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using GameEvents.UI;
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
            Player.PlayerInputSo.OnLeftMousePressedEnd += HandleMousePressedEnd;
            Player.PlayerInputSo.OnRightMousePressedEnd += HandleRightMousePressedEnd;
            Player.CoverModule.SetHide(false);
            Bus<CameraZoomEvent>.Raise(new CameraZoomEvent(5, true));
        }

        public override void Update()
        {
            base.Update();

            if (!Player.IsControl)
                return;

            Player.PlayerGunCompo.TryFirePlayer();
            if (Player.PlayerGunCompo.CurrentAmmo <= 0)
            {
                Player.ChangeState(PlayerStates.RELOADING);
                Player.GunCursorModule.UnActive();
            }
        }

        public override void Exit()
        {
            Player.PlayerInputSo.OnLeftMousePressedEnd -= HandleMousePressedEnd;
            Player.PlayerInputSo.OnRightMousePressedEnd -= HandleRightMousePressedEnd;
            Player.CoverModule.SetHide(true);
            Bus<CameraZoomEvent>.Raise(new CameraZoomEvent(0, false));
            Bus<GunAmmoUIActiveEvent>.Raise(new GunAmmoUIActiveEvent(0, 0, false));
            base.Exit();
        }

        private void HandleRightMousePressedEnd()
        {
            if (!Player.IsControl)
                return;

            Player.ChangeState(PlayerStates.IDLE);
            Player.GunCursorModule.UnActive();
        }

        private void HandleMousePressedEnd()
        {
            if (!Player.IsControl)
                return;

            Player.ChangeState(PlayerStates.AIMING);
        }
    }
}
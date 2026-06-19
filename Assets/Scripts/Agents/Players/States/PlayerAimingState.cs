using Agents.FSM;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using GameEvents.UI;
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
            Player.AimPositionModule?.MoveToAimPosition();
            Player.GunCursorModule.Active();
            Player.CoverModule.SetHide(false);
            Player.PlayerInputSo.OnLeftMousePressedStart += HandleLeftMousePressedStart;
            Player.PlayerInputSo.OnRightMousePressedEnd += HandleRightMousePressedEnd;
            Bus<CameraZoomEvent>.Raise(new CameraZoomEvent(5, true));
            Bus<GunAmmoUIActiveEvent>.Raise(new GunAmmoUIActiveEvent(Player.PlayerGunCompo.CurrentAmmo, Player.PlayerGunCompo.GunData.MaxAmmo, true));
            Player.PlayerGunCompo.GunData.OnAimStart(Player.PlayerGunCompo);
        }

        public override void Update()
        {
            base.Update();

            if (!Player.IsControl)
                return;

            Player.PlayerGunCompo.GunData.OnAimUpdate(Player.PlayerGunCompo);
        }

        public override void Exit()
        {
            Player.PlayerGunCompo.GunData.OnAimEnd(Player.PlayerGunCompo);
            Bus<CameraZoomEvent>.Raise(new CameraZoomEvent(0, false));
            Bus<GunAmmoUIActiveEvent>.Raise(new GunAmmoUIActiveEvent(0, 0, false));
            Player.PlayerInputSo.OnLeftMousePressedStart -= HandleLeftMousePressedStart;
            Player.PlayerInputSo.OnRightMousePressedEnd -= HandleRightMousePressedEnd;
            base.Exit();
        }

        private void HandleRightMousePressedEnd()
        {
            if (!Player.IsControl)
                return;

            Player.GunCursorModule.UnActive();
            Player.ChangeState(PlayerStates.IDLE);
        }

        private void HandleLeftMousePressedStart()
        {
            if (!Player.IsControl)
                return;

            if (Player.PlayerGunCompo.GunData.CanShootInAimingState)
            {
                bool shot = Player.PlayerGunCompo.TryFirePlayer();
                if (!shot)
                    return;

                if (Player.PlayerGunCompo.CurrentAmmo <= 0)
                {
                    Player.ChangeState(PlayerStates.RELOADING);
                    Player.GunCursorModule.UnActive();
                    return;
                }

                Player.GunCursorModule.UnActive();
                Player.ChangeState(PlayerStates.IDLE);
                return;
            }

            Player.ChangeState(PlayerStates.SHOOTING);
        }
    }
}

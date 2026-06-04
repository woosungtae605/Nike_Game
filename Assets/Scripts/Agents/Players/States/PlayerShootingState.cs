using Agents.FSM;
using FSM;
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
        }

        public override void Update()
        {
            base.Update();
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
            base.Exit();
        }

        private void HandleRightMousePressedEnd()
        {
            Player.ChangeState(PlayerStates.IDLE);
            Player.GunCursorModule.UnActive();
        }

        private void HandleMousePressedEnd()
        {
            Player.ChangeState(PlayerStates.AIMING);
        }
    }
}
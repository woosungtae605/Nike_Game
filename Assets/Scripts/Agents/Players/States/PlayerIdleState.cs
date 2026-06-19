using Agents.FSM;
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
            Player.AimPositionModule?.MoveToOriginPosition();
            if (Player.PlayerGunCompo.CurrentAmmo < Player.PlayerGunCompo.GunData.MaxAmmo)
            {
                Player.ChangeState(PlayerStates.RELOADING);
            }
            Player.CoverModule.SetHide(true);
            Player.PlayerInputSo.OnRightMousePressedStart += HandleRightMousePressedStart;
        }

        public override void Exit()
        {
            Player.PlayerInputSo.OnRightMousePressedStart -= HandleRightMousePressedStart;
            base.Exit();
        }

        private void HandleRightMousePressedStart()
        {
            if (!Player.IsControl)
                return;

            Player.ChangeState(PlayerStates.AIMING);
        }
    }
}

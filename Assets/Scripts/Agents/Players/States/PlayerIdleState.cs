using FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerIdleState : AbstractPlayerState, ICanAttack
    {
        public PlayerIdleState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (Player.PlayerGunCompo.CurrentAmmo < Player.PlayerGunCompo.GunData.MaxAmmo)
            {
                Player.ChangeState(PlayerStates.RELOAD);
            }
            Player.CoverModule.SetHide(true);
        }
    }
}
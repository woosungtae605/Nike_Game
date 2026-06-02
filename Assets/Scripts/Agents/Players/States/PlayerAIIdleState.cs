using FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerAIIdleState : AbstractPlayerState
    {
        public PlayerAIIdleState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            if (Player.PlayerGunCompo.CurrentAmmo <= 0)
            {
                Player.ChangeState(PlayerStates.RELOAD);
            }
            Player.CoverModule.SetHide(true);
        }
    }
}
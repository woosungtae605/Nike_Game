using Agents.Enemies;
using Agents.FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerAIShootingState : AbstractPlayerState
    {
        public PlayerAIShootingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.CoverModule.SetHide(false);
        }

        public override void Update()
        {
            base.Update();
            
            if (Player.PlayerGunCompo.CurrentAmmo <= 0)
            {
                Player.ChangeState(PlayerStates.AIRELOADING);
                return;
            }
            
            if (Player.EnemyRegisterSo == null)
            {
                Player.ChangeState(PlayerStates.AIIDLE);
                return;
            }

            Enemy target = Player.PlayerGunCompo.GetAITarget(Player.EnemyRegisterSo);
            
            if (target == null || !target.gameObject.activeSelf)
            {
                Player.ChangeState(PlayerStates.AIIDLE);
                return;
            }
            
            Player.PlayerGunCompo.TryFireAI(target);
        }
        
        public override void Exit()
        {
            Player.CoverModule.SetHide(true);
            base.Exit();
        }
    }
}
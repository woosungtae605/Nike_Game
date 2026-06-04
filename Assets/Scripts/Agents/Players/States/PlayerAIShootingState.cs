using Agents.Enemies;
using Agents.FSM;
using CoreSystem.BusSystem;
using GameEvents.Camera;
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
            
            if (Player.CurrentTarget == null || !Player.CurrentTarget.gameObject.activeSelf)
            {
                Player.ClearTarget();
                Player.ChangeState(PlayerStates.AIIDLE);
                return;
            }
            
            Player.PlayerGunCompo.TryFireAI(Player.CurrentTarget);
        }
        
        public override void Exit()
        {
            Player.CoverModule.SetHide(true);
            base.Exit();
        }
    }
}
using Agents.Enemies;
using Agents.FSM;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class PlayerAIIdleState : AbstractPlayerState
    {
        private float _enterTime;
        private const float ExitTime = 0.5f;
        public PlayerAIIdleState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
        
        public override void Enter()
        {
            _enterTime = Time.time;
            base.Enter();
            if (Player.PlayerGunCompo.CurrentAmmo <= 0)
            {
                Player.ChangeState(PlayerStates.AIRELOADING);
            }
            Player.CoverModule.SetHide(true);
        }

        public override void Update()
        {
            base.Update();
            if (_enterTime + ExitTime < Time.time)
            {
                AbstractEnemy target = Player.PlayerGunCompo.GetAITarget(Player.EnemyRegisterSo);

                if (target == null || !target.gameObject.activeSelf)
                    return;
                
                Player.SetTarget(target);
                Player.ChangeState(PlayerStates.AISHOOTING);
            }
        }
    }
}
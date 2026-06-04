using Agents.FSM;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class PlayerAIIdleState : AbstractPlayerState
    {
        private float _enterTime;
        private float _exitTime = 0.5f;
        public PlayerAIIdleState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
        
        public override void Enter()
        {
            _enterTime = Time.time;
            base.Enter();
            if (Player.PlayerGunCompo.CurrentAmmo <= 0)
            {
                Player.ChangeState(PlayerStates.RELOADING);
            }
            Player.CoverModule.SetHide(true);
        }

        public override void Update()
        {
            base.Update();
            if (_enterTime + _exitTime < Time.time)
            {
                Player.ChangeState(PlayerStates.AISHOOTING);
            }
        }
    }
}
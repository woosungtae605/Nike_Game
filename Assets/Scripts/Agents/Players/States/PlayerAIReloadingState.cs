using Agents.FSM;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class PlayerAIReloadingState : AbstractPlayerState
    {
        public PlayerAIReloadingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
        private float _enterTime;

        public override void Enter()
        {
            base.Enter();
            _enterTime = Time.time;
            
            Player.CoverModule.SetHide(true);
        }

        public override void Update()
        {
            base.Update();
            if (Player.PlayerGunCompo.GunData.ReloadTime + _enterTime < Time.time)
            {
                Player.PlayerGunCompo.Reload();
                
                Player.ChangeState(PlayerStates.AIIDLE);
            }
        }
    }
}
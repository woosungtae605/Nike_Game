using CoreSystem.BusSystem;
using FSM;
using GameEvents.UI;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class PlayerReloading : AbstractPlayerState, ICanAttack
    {
        private float _enterTime;
        public PlayerReloading(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _enterTime = Time.time;
            if(Player.IsControl)
                Bus<NikkeReloadUIActiveEvent>.Raise(new NikkeReloadUIActiveEvent(Player.PlayerGunCompo.GunData.ReloadTime, true));
            
            Player.CoverModule.SetHide(true);
        }

        public override void Update()
        {
            base.Update();
            if (Player.PlayerGunCompo.GunData.ReloadTime + _enterTime < Time.time)
            {
                Player.PlayerGunCompo.Reload();
                
                Player.ChangeState(Player.IsControl ? PlayerStates.IDLE : PlayerStates.AIIDLE);
            }
        }

        public override void Exit()
        {
            if(Player.IsControl)
                Bus<NikkeReloadUIActiveEvent>.Raise(new NikkeReloadUIActiveEvent(0, false));
            base.Exit();
        }
    }
}
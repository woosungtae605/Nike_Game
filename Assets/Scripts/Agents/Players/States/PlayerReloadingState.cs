using Agents.FSM;
using CoreSystem.BusSystem;
using FSM;
using GameEvents.UI;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class PlayerReloadingState : AbstractPlayerState
    {
        private float _enterTime;
        public PlayerReloadingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.AimPositionModule?.MoveToOriginPosition();
            _enterTime = Time.time;
            if(Player.IsControl)
                Bus<NikkeReloadUIActiveEvent>.Raise(new NikkeReloadUIActiveEvent(Player.PlayerGunCompo.GunData.ReloadTime, true));
            
            Player.CoverModule.SetHide(true);
            if(Player.PlayerGunCompo.CurrentAmmo > 0)
                Player.PlayerInputSo.OnRightMousePressedStart += HandleRightMousePressedStart;
        }

        private void HandleRightMousePressedStart()
        {
            Player.ChangeState(PlayerStates.AIMING);
        }

        public override void Update()
        {
            base.Update();
            if (Player.PlayerGunCompo.GunData.ReloadTime + _enterTime < Time.time)
            {
                Player.PlayerGunCompo.Reload();
                
                Player.ChangeState(PlayerStates.IDLE);
            }
        }

        public override void Exit()
        {
            Bus<NikkeReloadUIActiveEvent>.Raise(new NikkeReloadUIActiveEvent(0, false));
            Player.PlayerInputSo.OnRightMousePressedStart -= HandleRightMousePressedStart;
            base.Exit();
        }
    }
}

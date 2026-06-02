using System;
using Agents.CombatSystem;
using Agents.Module;
using Agents.Players.Gun;
using Agents.Players.States;
using CoreSystem.BusSystem;
using FSM;
using GameEvents.UI;
using Systems;
using UnityEngine;

namespace Agents.Players
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerInputSO PlayerInputSo { get; private set; }
        [field: SerializeField] public PlayerDataSO PlayerData { get; private set; }
        
        [field: SerializeField] public Transform CameraTransform { get; private set; }
        [SerializeField] private StateListSO playerStates;
        
        public PlayerGun PlayerGunCompo { get; private set; }
        public CoverModule CoverModule { get; private set; }
        
        private AgentStateMachine _stateMachine;

        public bool IsControl { get; private set; }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            
            _stateMachine = new AgentStateMachine(this, playerStates.states);
            PlayerGunCompo = GetModule<PlayerGun>();
            CoverModule = GetModule<CoverModule>();
            
            HealthModule.ChangeHealth(PlayerData.NikkeHp);
            
            ChangeState(PlayerStates.IDLE);
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();
            PlayerInputSo.OnLeftMousePressedStart += HandleLeftMousePressedStart;
        }

        private void OnDestroy()
        {
            PlayerInputSo.OnLeftMousePressedStart -= HandleLeftMousePressedStart;
        }

        private void HandleLeftMousePressedStart()
        {
            if (_stateMachine.CurrentState is ICanAttack && IsControl)
            {
                ChangeState(PlayerStates.SHOOTING);
            }
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        public void PlayerControl()
        {
            IsControl = true;
            ChangeState(PlayerStates.IDLE);
        }

        public void PlayerNotControl()
        {
            IsControl = false;
            Bus<NikkeReloadUIActiveEvent>.Raise(new NikkeReloadUIActiveEvent(0, false));
            ChangeState(PlayerStates.AIIDLE);
        }

        public void ChangeState(PlayerStates newState) => _stateMachine.ChangeState((int)newState);

        public override void ApplyDamage(DamageData damageData)
        {
            if (CoverModule != null && CoverModule.IsHide && !CoverModule.IsCoverBroken)
            {
                CoverModule.ApplyCoverDamage(damageData);
                return;
            }
            
            base.ApplyDamage(damageData);
        }
    }
}
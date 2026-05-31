using System;
using Agents.Players.Gun;
using Agents.Players.States;
using FSM;
using Systems;
using UnityEngine;

namespace Agents.Players
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
        [SerializeField] private StateListSO playerStates;
        
        public PlayerGun PlayerGunCompo { get; private set; }
        
        private AgentStateMachine _stateMachine;

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _stateMachine = new AgentStateMachine(this, playerStates.states);
            PlayerGunCompo = GetModule<PlayerGun>();
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();
            PlayerInput.OnLeftMousePressedStart += HandleLeftMousePressedStart;
        }

        private void OnDestroy()
        {
            PlayerInput.OnLeftMousePressedStart -= HandleLeftMousePressedStart;
        }

        private void HandleLeftMousePressedStart()
        {
            if (_stateMachine.CurrentState is ICanAttack)
            {
                ChangeState(PlayerStates.SHOOTING);
            }
        }

        private void Start()
        {
            ChangeState(PlayerStates.IDLE);
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        public void ChangeState(PlayerStates newState) => _stateMachine.ChangeState((int)newState);
    }
}
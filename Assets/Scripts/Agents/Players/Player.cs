using System;
using Agents.CombatSystem;
using Agents.Enemies;
using Agents.FSM;
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
        public GunCursorModule GunCursorModule { get; private set; }
        
        private AgentStateMachine _stateMachine;

        public bool IsControl { get; private set; }
        
        public EnemyRegisterSo EnemyRegisterSo { get; private set; }
        public AbstractEnemy CurrentTarget { get; private set; }
        public int AttackDamage { get; private set; }
        public int MaxHp { get; private set; }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            
            _stateMachine = new AgentStateMachine(this, playerStates.states);
            PlayerGunCompo = GetModule<PlayerGun>();
            CoverModule = GetModule<CoverModule>();
            GunCursorModule = GetModule<GunCursorModule>();
            
            AttackDamage = GetBaseAttackDamage();
            MaxHp = PlayerData.MaxHp;
            HealthModule.ChangeHealth(MaxHp);
        }

        public void SetBattleStats(int attackDamage, int maxHp)
        {
            AttackDamage = Mathf.Max(0, attackDamage);
            MaxHp = Mathf.Max(1, maxHp);

            if (HealthModule != null)
                HealthModule.ChangeHealth(MaxHp);
        }

        private int GetBaseAttackDamage()
        {
            if (PlayerData == null || PlayerData.PlayerGunData == null || PlayerData.PlayerGunData.GunData == null)
                return 0;

            return PlayerData.PlayerGunData.GunData.Damage;
        }

        public void SetEnemyRegister(EnemyRegisterSo enemyRegisterSo)
        {
            EnemyRegisterSo = enemyRegisterSo;
        }
        public void SetTarget(AbstractEnemy target)
        {
            CurrentTarget = target;
        }
        public void ClearTarget()
        {
            CurrentTarget = null;
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        public void PlayerControl()
        {
            IsControl = true;
            GunCursorModule.UnActive();
            ChangeState(PlayerStates.IDLE);
        }

        public void SetControl(bool control)
        {
            IsControl = control;
        }

        public void PlayerNotControl()
        {
            IsControl = false;
            Bus<NikkeReloadUIActiveEvent>.Raise(new NikkeReloadUIActiveEvent(0, false));
            GunCursorModule.UnActive();
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

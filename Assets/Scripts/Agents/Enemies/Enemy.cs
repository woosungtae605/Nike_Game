using System;
using Agents.CombatSystem;
using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.UI;
using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Enemies
{
    public class Enemy : Agent, IPoolable
    {
        [field: SerializeField] public EnemyDataSO EnemyDataSo { get; private set; }
        
        public PoolItemSo PoolItem { get; set; }
        
        public GameObject GameObject => this != null ? gameObject : null;
        
        private EnemyManager _enemyManager;
        
        public void SetManager(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        public void ResetItem()
        {
            HealthModule.ChangeHealth(EnemyDataSo.MaxHp);
            
            HealthModule.OnDeath -= HandleDeath;
            HealthModule.OnDeath += HandleDeath;
        }
        
        private void HandleDeath()
        {
            HealthModule.OnDeath -= HandleDeath;

            if (_enemyManager != null)
                _enemyManager.NotifyEnemyDead(this);
        }
    }
}
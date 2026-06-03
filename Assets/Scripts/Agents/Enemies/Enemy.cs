using System;
using Agents.Module;
using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Enemies
{
    public class Enemy : Agent, IPoolable
    {
        [SerializeField] private EnemyRegisterSo enemyRegisterSo;
        [field: SerializeField] public EnemyDataSO EnemyDataSo { get; private set; }
        
        [Header("Pools")]
        public PoolItemSo PoolItem { get; set; }
        
        public GameObject GameObject => this != null ? gameObject : null;

        public void ResetItem()
        {
            HealthModule.ChangeHealth(EnemyDataSo.MaxHp);
            enemyRegisterSo.Register(this);
        }

        private void OnDisable()
        {
            enemyRegisterSo.UnRegister(this);
        }
    }
}
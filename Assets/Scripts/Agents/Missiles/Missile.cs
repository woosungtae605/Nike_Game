using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Missiles
{
    public abstract class Missile : Agent, IPoolable
    {
        [field: SerializeField] public Transform HitPos { get; private set; }
        [field: SerializeField] public MissileSO MissileSo { get; private set; }

        public PoolItemSo PoolItem { get; set; }
        public GameObject GameObject => this != null ? gameObject : null;
        public PoolManagerSo PoolManagerSo { get; set; }
        public void ResetItem()
        {
            HealthModule.ChangeHealth(MissileSo.MaxHealth);
            HealthModule.OnDeath -= HandleDeath;
            HealthModule.OnDeath += HandleDeath;
        }

        protected virtual void HandleDeath()
        {
            HealthModule.OnDeath -= HandleDeath;

            if (PoolManagerSo != null)
                PoolManagerSo.Push(this);
        }

        public abstract void Shot(Vector3 startPos, Vector3 targetPos, float curveAngle = 0f);
    }
}
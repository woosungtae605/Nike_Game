using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Missiles
{
    public abstract class Missile : Agent, IPoolable
    {
        [field: SerializeField] public Transform HitPos { get; private set; }
        [SerializeField] private MissileSO missileSo;

        public PoolItemSo PoolItem { get; set; }
        public GameObject GameObject => this != null ? gameObject : null;
        public void ResetItem()
        {
            HealthModule.ChangeHealth(missileSo.MaxHealth);
        }
    }
}
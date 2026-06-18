using Gamelib.ObjectPool.Runtime;
using Module;
using Systems;
using UnityEngine;

namespace Agents.Enemies.Module
{
    public class EnemyDeathEffectModule : MonoBehaviour, IModule
    {
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private PoolItemSo deathEffect;

        public void Initialize(ModuleOwner owner)
        {
        }

        public void Play(Transform effectPoint)
        {
            if (poolManagerSo == null || deathEffect == null)
                return;

            ParticlePooling particle = poolManagerSo.Pop<ParticlePooling>(deathEffect);
            if (particle == null)
                return;

            particle.PoolManagerSo = poolManagerSo;
            particle.Play(effectPoint != null ? effectPoint.position : transform.position);
        }
    }
}

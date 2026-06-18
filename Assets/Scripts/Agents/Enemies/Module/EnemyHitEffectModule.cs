using Agents.CombatSystem;
using Gamelib.ObjectPool.Runtime;
using Module;
using Systems;
using UnityEngine;

namespace Agents.Enemies.Module
{
    public class EnemyHitEffectModule : MonoBehaviour, IModule
    {
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private PoolItemSo hitEffect;
        [SerializeField] private float normalOffset = 0.03f;

        public void Initialize(ModuleOwner owner)
        {
        }

        public void Play(DamageData damageData, Transform fallbackPoint)
        {
            if (poolManagerSo == null || hitEffect == null)
                return;

            Vector3 position = GetHitPosition(damageData, fallbackPoint);
            if (damageData.HitNormal != Vector3.zero)
                position += damageData.HitNormal.normalized * normalOffset;

            ParticlePooling particle = poolManagerSo.Pop<ParticlePooling>(hitEffect);
            if (particle == null)
                return;

            particle.PoolManagerSo = poolManagerSo;
            particle.Play(position);
        }

        private Vector3 GetHitPosition(DamageData damageData, Transform fallbackPoint)
        {
            if (damageData.HitPoint != Vector3.zero)
                return damageData.HitPoint;

            return fallbackPoint != null ? fallbackPoint.position : transform.position;
        }
    }
}

using System.Collections;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Camera;
using GameEvents.UI;
using Gamelib.ObjectPool.Runtime;
using Systems;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

namespace Agents.Enemies.Obliterators
{
    public class Obliterator : AbstractEnemy
    {
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private PoolItemSo effect;
        [field: SerializeField] public Transform CameraTarget { get; private set; }
        private CinemachineImpulseSource _impulseSource;
        protected override void HandleDeath()
        {
            HealthModule.OnDeath -= HandleDeath;
            PlayDeathEffect();
            ChangeState(EnemyState.DEATH);
            StartCoroutine(StartAction());
        }

        public override void ResetItem()
        {
            base.ResetItem();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private IEnumerator StartAction()
        {
            Bus<BattleEndEvent>.Raise(new BattleEndEvent());
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(CameraTarget, 1f));
            yield return new WaitForSeconds(1.5f);

            ParticlePooling particlePooling = poolManagerSo.Pop<ParticlePooling>(this.effect);

            particlePooling.PoolManagerSo = poolManagerSo;
            particlePooling.Play(HitPos.position);

            _impulseSource.GenerateImpulse();
            yield return new WaitForSeconds(0.7f);
            Bus<ClearUIEvent>.Raise(new ClearUIEvent());
        }
    }
}

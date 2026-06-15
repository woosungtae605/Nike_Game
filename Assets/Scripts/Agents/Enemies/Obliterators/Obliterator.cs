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
        [SerializeField] private PoolItemSo _effect;
        private CinemachineImpulseSource _impulseSource;
        protected override void HandleDeath()
        {
            HealthModule.OnDeath -= HandleDeath;
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
            yield return new WaitForSeconds(1.5f);

            ParticlePooling effect = poolManagerSo.Pop<ParticlePooling>(_effect);

            effect.PoolManagerSo = poolManagerSo;
            effect.Play(HitPos.position);

            _impulseSource.GenerateImpulse();
            yield return new WaitForSeconds(0.7f);
            Bus<ClearUIEvent>.Raise(new ClearUIEvent());
        }
    }
}
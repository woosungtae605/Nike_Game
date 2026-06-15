using System.Collections;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Agents.Enemies.Obliterators
{
    public class Obliterator : AbstractEnemy
    {
        private CinemachineImpulseSource impulseSource;
        protected override void HandleDeath()
        {
            HealthModule.OnDeath -= HandleDeath;
            
        }

        public override void ResetItem()
        {
            base.ResetItem();
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private IEnumerator StartAction()
        {
            Bus<BattleEndEvent>.Raise(new BattleEndEvent());
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(HitPos, 1));
            yield return new WaitForSeconds(1);
            impulseSource.GenerateImpulse();
        }
    }
}
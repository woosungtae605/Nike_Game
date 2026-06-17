using CoreSystem.BusSystem;
using GameEvents.UI;
using Systems.GameSystem.Wave;
using UnityEngine;

namespace Agents.Enemies
{
    public class PlayerSensor : MonoBehaviour
    {
        [SerializeField] private WaveInformationSO waveInformationSO;
        [SerializeField] private LayerMask layerMask;

        private void OnTriggerEnter(Collider other)
        {
            int targetMask = layerMask.value;
            if (targetMask == 0)
                targetMask = LayerMask.GetMask("Player");

            if ((targetMask & (1 << other.gameObject.layer)) == 0)
                return;

            if(other.TryGetComponent<Agent>(out Agent agent))
            {
                Bus<BattleGoUIEvent>.Raise(new BattleGoUIEvent(waveInformationSO));
            }
        }
    }
}

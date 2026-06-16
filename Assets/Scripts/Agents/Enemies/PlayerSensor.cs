using System;
using Systems.GameSystem.Wave;
using UnityEngine;

namespace Agents.Enemies
{
    public class PlayerSensor : MonoBehaviour
    {
        [SerializeField] private WaveInformationSO waveInformationSO;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                Debug.Log("dd");
        }
    }
}
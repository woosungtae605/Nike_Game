using UnityEngine;

namespace Systems.GameSystem.Wave
{
    [CreateAssetMenu(fileName = "CurrentWaveInformation", menuName = "SO/Wave/CurrentWaveInformation", order = 0)]
    public class CurrentWaveInformationSO : ScriptableObject
    {
        [SerializeField] private WaveInformationSO currentWaveInformation;

        public WaveInformationSO CurrentWaveInformation => currentWaveInformation;
        public WaveDataSo WaveData => currentWaveInformation != null ? currentWaveInformation.WaveData : null;
        public int GetCoin => currentWaveInformation != null ? currentWaveInformation.GetCoin : 0;

        public void SetCurrentWaveInformation(WaveInformationSO waveInformation)
        {
            currentWaveInformation = waveInformation;
        }

        public void Clear()
        {
            currentWaveInformation = null;
        }
    }
}

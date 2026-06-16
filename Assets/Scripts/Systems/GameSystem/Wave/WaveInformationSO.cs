using UnityEngine;

namespace Systems.GameSystem.Wave
{
    [CreateAssetMenu(fileName = "WaveInformation", menuName = "SO/WaveInformation", order = 0)]
    public class WaveInformationSO : ScriptableObject
    {
        [field: SerializeField] public WaveDataSo WaveData { get; private set; }
        [field: SerializeField] public int WaveNumber { get; private set; }
    }
}
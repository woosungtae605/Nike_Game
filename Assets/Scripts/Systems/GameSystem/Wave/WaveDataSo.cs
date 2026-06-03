using UnityEngine;

namespace Systems.GameSystem.Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "SO/Wave/WaveData", order = 0)]
    public class WaveDataSo : ScriptableObject
    {
        [field: SerializeField] public SpawnDataSo[] SpawnDataSos { get; private set; }
    }
}
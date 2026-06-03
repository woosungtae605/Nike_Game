using Agents.Enemies;
using UnityEngine;

namespace Systems.GameSystem.Wave
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "SO/Wave/SpawnData", order = 0)]
    public class SpawnDataSo : ScriptableObject
    {
        [field: SerializeField] public EnemySpawnData[] EnemySpawnDatas { get; private set; }
        [field: SerializeField] public float NextSpawnDelay { get; private set; }
    }
}
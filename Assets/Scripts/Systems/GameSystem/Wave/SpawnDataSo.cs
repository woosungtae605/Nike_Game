using Agents.Enemies;
using Systems.GameSystem.Wave.Conditions;
using UnityEngine;

namespace Systems.GameSystem.Wave
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "Wave/SpawnData", order = 0)]
    public class SpawnDataSo : ScriptableObject
    {
        [field: SerializeField] public EnemySpawnData[] EnemySpawnDatas { get; private set; }
        [field: SerializeField] public SpawnConditionSO SpawnConditionSo { get; private set; }
        [field: SerializeField] public bool IsBossSpawned { get; private set; }
    }
}
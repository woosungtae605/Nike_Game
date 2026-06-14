using UnityEngine;

namespace Agents.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyDataSO", order = 0)]
    public class EnemyDataSO : ScriptableObject
    {
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField] public int MaxHp { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; } // 루트모션 쓰는 에너미는 무시
    }
}
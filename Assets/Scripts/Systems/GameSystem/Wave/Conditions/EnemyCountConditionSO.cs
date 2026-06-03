using Agents.Enemies;
using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    [CreateAssetMenu(fileName = "EnemyCount", menuName = "Wave/Condition/EnemyCount", order = 0)]
    public class EnemyCountConditionSO : SpawnConditionSO
    {
        [SerializeField] private int enemyCount;

        public override bool GoToNext()
        {
            return Context.EnemyRegister.EnemyCount <= enemyCount;
        }
    }
}
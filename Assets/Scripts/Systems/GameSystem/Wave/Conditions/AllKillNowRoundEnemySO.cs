using Agents.Enemies;
using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    [CreateAssetMenu(fileName = "EnemyCount", menuName = "Wave/Condition/AllKillNowRoundEnemy", order = 0)]
    public class AllKillNowRoundEnemySO : SpawnConditionSO
    {
        public override bool GoToNext()
        {
            foreach (Enemy enemy in Context.SpawnedEnemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                    return false;
            }
            
            return true;
        }
    }
}
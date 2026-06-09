using Agents.Enemies;
using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    [CreateAssetMenu(fileName = "AllKillNowRoundEnemy", menuName = "Wave/Condition/AllKillNowRoundEnemy", order = 0)]
    public class AllKillNowRoundEnemySO : SpawnConditionSO
    {
        public override bool GoToNext()
        {
            foreach (AbstractEnemy enemy in Context.SpawnedEnemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                    return false;
            }
            
            return true;
        }
    }
}
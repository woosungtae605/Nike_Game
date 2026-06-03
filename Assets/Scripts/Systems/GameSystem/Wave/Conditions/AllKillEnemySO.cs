using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    [CreateAssetMenu(fileName = "AllKillEnemy", menuName = "Wave/Condition/AllKillEnemy", order = 0)]
    public class AllKillEnemySO : SpawnConditionSO
    {
        public override bool GoToNext()
        {
            return Context.EnemyRegister.EnemyCount <= 0;
        }
    }
}
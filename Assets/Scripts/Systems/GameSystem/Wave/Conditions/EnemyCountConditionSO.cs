using Agents.Enemies;
using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    [CreateAssetMenu(fileName = "EnemyCount", menuName = "Wave/Condition/EnemyCount", order = 0)]
    public class EnemyCountConditionSO : SpawnConditionSO
    {
        [SerializeField] private EnemyRegisterSo enemyRegisterSo;
        [SerializeField] private int enemyCount;
        
        public override void Initialize()
        {
            Debug.Assert(enemyRegisterSo != null, "enemyRegisterSo is null");
        }

        public override bool GoToNext()
        {
            return enemyCount <= enemyRegisterSo.EnemyCount;
        }
    }
}
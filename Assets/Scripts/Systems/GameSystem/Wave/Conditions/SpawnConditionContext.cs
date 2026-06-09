using System.Collections.Generic;
using Agents.Enemies;

namespace Systems.GameSystem.Wave.Conditions
{
    public class SpawnConditionContext
    {
        public EnemyRegisterSo EnemyRegister { get; private set; }
        public IReadOnlyList<AbstractEnemy> SpawnedEnemies { get; private set; }

        public SpawnConditionContext(EnemyRegisterSo enemyRegister, IReadOnlyList<AbstractEnemy> spawnedEnemies)
        {
            EnemyRegister = enemyRegister;
            SpawnedEnemies = spawnedEnemies;
        }
    }
}
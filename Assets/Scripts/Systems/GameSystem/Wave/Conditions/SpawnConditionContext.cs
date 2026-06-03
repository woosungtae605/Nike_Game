using System.Collections.Generic;
using Agents.Enemies;

namespace Systems.GameSystem.Wave.Conditions
{
    public class SpawnConditionContext
    {
        public EnemyRegisterSo EnemyRegister { get; private set; }
        public IReadOnlyList<Enemy> SpawnedEnemies { get; private set; }

        public SpawnConditionContext(EnemyRegisterSo enemyRegister, IReadOnlyList<Enemy> spawnedEnemies)
        {
            EnemyRegister = enemyRegister;
            SpawnedEnemies = spawnedEnemies;
        }
    }
}
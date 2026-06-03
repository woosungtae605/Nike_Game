using System;
using Agents.Enemies;
using Gamelib.ObjectPool.Runtime;

namespace Systems.GameSystem.Wave
{
    [Serializable]
    public struct EnemySpawnData
    {
        public PoolItemSo enemyDataSos;
        public int spawnPos;
        public int amount;
    }
}
using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private EnemyRegisterSo enemyRegisterSo;

        public EnemyRegisterSo EnemyRegister => enemyRegisterSo;
        
        public void ClearEnemies()
        {
            enemyRegisterSo.Clear();
        }
        
        public Enemy SpawnEnemy(PoolItemSo enemyType, Vector3 spawnPosition)
        {
            Enemy enemy = poolManagerSo.Pop<Enemy>(enemyType);

            if (enemy == null)
                return null;

            enemy.SetManager(this);
            enemy.transform.position  = spawnPosition;
            enemyRegisterSo.Register(enemy);
            
            return enemy;
        }
        
        public void PushEnemy(Enemy enemy)
        {
            if (enemy == null)
                return;

            poolManagerSo.Push(enemy);
        }
        
        public void NotifyEnemyDead(Enemy enemy)
        {
            if (enemy == null)
                return;
            
            enemyRegisterSo.UnRegister(enemy);
            poolManagerSo.Push(enemy);
        }
    }
}
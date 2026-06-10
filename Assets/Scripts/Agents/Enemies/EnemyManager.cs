using Agents.Players;
using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private EnemyRegisterSo enemyRegisterSo;
        [SerializeField] private PlayerManager playerManager;

        public EnemyRegisterSo EnemyRegister => enemyRegisterSo;
        
        public void ClearEnemies()
        {
            enemyRegisterSo.Clear();
        }
        
        public AbstractEnemy SpawnEnemy(PoolItemSo enemyType, Vector3 spawnPosition)
        {
            AbstractEnemy abstractEnemy = poolManagerSo.Pop<AbstractEnemy>(enemyType);

            if (abstractEnemy == null)
                return null;

            abstractEnemy.SetManager(this);
            abstractEnemy.transform.position = spawnPosition;
            abstractEnemy.SetVariableValue("PlayerManager", playerManager);
            abstractEnemy.SetVariableValue("Enemy", abstractEnemy);
            enemyRegisterSo.Register(abstractEnemy);
            
            return abstractEnemy;
        }
        
        public void PushEnemy(AbstractEnemy abstractEnemy)
        {
            if (abstractEnemy == null)
                return;

            poolManagerSo.Push(abstractEnemy);
        }
        
        public void NotifyEnemyDead(AbstractEnemy abstractEnemy)
        {
            if (abstractEnemy == null)
                return;
            
            enemyRegisterSo.UnRegister(abstractEnemy);
            poolManagerSo.Push(abstractEnemy);
        }
    }
}

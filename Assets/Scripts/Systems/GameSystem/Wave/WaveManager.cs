using Agents.Enemies;
using Reflex.Attributes;
using UnityEngine;

namespace Systems.GameSystem.Wave
{
    public class WaveManager : MonoBehaviour
    {
         [Inject] private Transform[] _spawnPoints;

         private EnemyManager _enemyManager;
         
         public void SetEnemyManager(EnemyManager enemyManager)
         {
             _enemyManager = enemyManager;
         }
    }
}
using Agents.Enemies;
using Reflex.Attributes;
using UnityEngine;

namespace Systems.GameSystem.Wave
{
    public class WaveManager : MonoBehaviour
    {
         [Inject] private Transform[] _spawnPoints;
         
         [SerializeField] private WaveDataSo waveData;

         private EnemyManager _enemyManager;
         
         public void SetEnemyManager(EnemyManager enemyManager)
         {
             _enemyManager = enemyManager;
         }
    }
}
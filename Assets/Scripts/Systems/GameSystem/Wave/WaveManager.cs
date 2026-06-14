using System;
using System.Collections;
using System.Collections.Generic;
using Agents.Enemies;
using Reflex.Attributes;
using Systems.GameSystem.Wave.Conditions;
using UnityEngine;

namespace Systems.GameSystem.Wave
{
    public class WaveManager : MonoBehaviour
    {
         [Inject] private Transform[] _spawnPoints;
         
         [SerializeField] private WaveDataSo waveData;

         private EnemyManager _enemyManager;
         private Coroutine _waveRoutine;

         public Action OnClear;         
         public Action<int, int> OnWaveStarted;
         public Action<AbstractEnemy> OnBossSpawn;
         public int WaveCount => waveData == null || waveData.SpawnDataSos == null ? 0 : waveData.SpawnDataSos.Length;

         
         public void SetEnemyManager(EnemyManager enemyManager)
         {
             _enemyManager = enemyManager;
         }
         
         public void StartWave()
         {
             if (_waveRoutine != null)
                 StopCoroutine(_waveRoutine);

             _waveRoutine = StartCoroutine(WaveRoutine());
         }

         private IEnumerator WaveRoutine()
         {
             int waveCount = WaveCount;
             
             int lastWaveCount = waveCount - 1;
             for (int i = 0; i < waveCount; i++)
             {
                 SpawnDataSo spawnData = waveData.SpawnDataSos[i];
                    
                 List<AbstractEnemy> spawnedEnemies = Spawn(spawnData);

                 if (spawnData.IsBossSpawned)
                 {
                     OnBossSpawn?.Invoke(spawnedEnemies[0]);
                 }
                 else
                 {
                     OnWaveStarted?.Invoke(i + 1, lastWaveCount);
                 }
                 if (spawnData.SpawnConditionSo == null)
                     continue;
                 
                 SpawnConditionContext context = new SpawnConditionContext(_enemyManager.EnemyRegister, spawnedEnemies);

                 spawnData.SpawnConditionSo.Initialize(context);

                 while (!spawnData.SpawnConditionSo.GoToNext())
                 {
                     yield return null;
                 }
             }
             
             OnClear?.Invoke();
         }
         
         private List<AbstractEnemy> Spawn(SpawnDataSo spawnData)
         {
             List<AbstractEnemy> spawnedEnemies = new();
             
             foreach (EnemySpawnData enemySpawnData in spawnData.EnemySpawnDatas)
             {
                 SpawnEnemyData(enemySpawnData, spawnedEnemies);
             }
             
             return spawnedEnemies;
         }
         
         private void SpawnEnemyData(EnemySpawnData spawnData, List<AbstractEnemy> spawnedEnemies)
         {
             if (spawnData.spawnPos < 0 || spawnData.spawnPos >= _spawnPoints.Length)
             {
                 Debug.LogError($"Spawn position index {spawnData.spawnPos} is out of range");
                 return;
             }

             Transform spawnPoint = _spawnPoints[spawnData.spawnPos];

             for (int i = 0; i < spawnData.amount; i++)
             {
                 AbstractEnemy abstractEnemy = _enemyManager.SpawnEnemy(spawnData.enemyDataSos, spawnPoint.position);

                 if (abstractEnemy != null)
                     spawnedEnemies.Add(abstractEnemy);
             }
         }
    }
}
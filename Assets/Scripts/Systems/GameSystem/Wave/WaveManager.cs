using System;
using System.Collections;
using System.Collections.Generic;
using Agents.Enemies;
using CoreSystem.BusSystem;
using GameEvents.UI;
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
         private WaveInformationSO _waveInformation;
         private bool _hasRaisedTutorialEnemySpawn;
         private bool _hasRaisedTutorialBossSpawn;

         public Action OnClear;         
         public Action<int, int> OnWaveStarted;
         public Action<AbstractEnemy> OnBossSpawn;
         public int WaveCount => waveData == null || waveData.SpawnDataSos == null ? 0 : waveData.SpawnDataSos.Length;

         public void SetWaveInformation(WaveInformationSO waveInformation)
         {
             _waveInformation = waveInformation;
             _hasRaisedTutorialEnemySpawn = false;
             _hasRaisedTutorialBossSpawn = false;

             if (waveInformation == null || waveInformation.WaveData == null)
                 return;

             waveData = waveInformation.WaveData;
         }
         
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

         public void StopWave()
         {
             if (_waveRoutine == null)
                 return;

             StopCoroutine(_waveRoutine);
             _waveRoutine = null;
         }

         private IEnumerator WaveRoutine()
         {
             int waveCount = WaveCount;
             if (waveCount <= 0)
             {
                 OnClear?.Invoke();
                 yield break;
             }
             
             int lastWaveCount = waveCount - 1;
             for (int i = 0; i < waveCount; i++)
             {
                 SpawnDataSo spawnData = waveData.SpawnDataSos[i];
                    
                 List<AbstractEnemy> spawnedEnemies = Spawn(spawnData);
                 RaiseTutorialEnemySpawnIfNeeded(spawnedEnemies);

                 if (spawnData.IsBossSpawned)
                 {
                     if (spawnedEnemies.Count > 0)
                     {
                         RaiseTutorialBossSpawnIfNeeded(spawnedEnemies[0]);
                         OnBossSpawn?.Invoke(spawnedEnemies[0]);
                     }
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
             
             _waveRoutine = null;
             OnClear?.Invoke();
         }

         private void RaiseTutorialEnemySpawnIfNeeded(List<AbstractEnemy> spawnedEnemies)
         {
             if (_hasRaisedTutorialEnemySpawn || _waveInformation == null || !_waveInformation.IsTutorial)
                 return;

             if (spawnedEnemies == null || spawnedEnemies.Count <= 0)
                 return;

             _hasRaisedTutorialEnemySpawn = true;
             Bus<TutorialEnemySpawnEvent>.Raise(new TutorialEnemySpawnEvent(spawnedEnemies[0]));
         }

         private void RaiseTutorialBossSpawnIfNeeded(AbstractEnemy boss)
         {
             if (_hasRaisedTutorialBossSpawn || _waveInformation == null || !_waveInformation.IsTutorial)
                 return;

             if (boss == null)
                 return;

             _hasRaisedTutorialBossSpawn = true;
             Bus<TutorialBossSpawnEvent>.Raise(new TutorialBossSpawnEvent(boss));
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

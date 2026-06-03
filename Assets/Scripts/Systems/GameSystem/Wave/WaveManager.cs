using System;
using System.Collections;
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
         private Coroutine _waveRoutine;

         public Action OnClear;
         
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
             foreach (SpawnDataSo spawnData in waveData.SpawnDataSos)
             {
                 Spawn(spawnData);

                 if (spawnData.SpawnConditionSo == null)
                     continue;

                 spawnData.SpawnConditionSo.Initialize();

                 while (!spawnData.SpawnConditionSo.GoToNext())
                 {
                     yield return null;
                 }
             }
             
             OnClear?.Invoke();
         }
         
         private void Spawn(SpawnDataSo spawnData)
         {
             foreach (EnemySpawnData enemySpawnData in spawnData.EnemySpawnDatas)
             {
                 SpawnEnemyData(enemySpawnData);
             }
         }
         
         private void SpawnEnemyData(EnemySpawnData spawnData)
         {
             if (spawnData.spawnPos < 0 || spawnData.spawnPos >= _spawnPoints.Length)
             {
                 Debug.LogError($"Spawn position index {spawnData.spawnPos} is out of range");
                 return;
             }

             Transform spawnPoint = _spawnPoints[spawnData.spawnPos];

             for (int i = 0; i < spawnData.amount; i++)
             {
                 _enemyManager.SpawnEnemy(spawnData.enemyDataSos, spawnPoint.position);
             }
         }
    }
}
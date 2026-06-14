using System;
using Agents.Enemies;
using Agents.Players;
using CoreSystem.BusSystem;
using GameEvents;
using Systems.GameSystem.Wave;
using UnityEngine;

namespace Systems.GameSystem
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private WaveManager waveManager;

        private void Awake()
        {
            Bus<BattleStartEvent>.OnEvent += HandleBattleStart;
        }

        private void OnDestroy()
        {
            Bus<BattleStartEvent>.OnEvent -= HandleBattleStart;
        }

        private void HandleBattleStart(BattleStartEvent obj)
        {
            Debug.Assert(enemyManager != null, "enemyManager is null");
            Debug.Assert(playerManager != null, "playerManager is null");
            Debug.Assert(waveManager != null, "waveManager is null");
            
            //enemy 초기화
            enemyManager.ClearEnemies();
            
            //player 초기화
            playerManager.Init(enemyManager.EnemyRegister);
            
            //Wave 초기화
            waveManager.SetEnemyManager(enemyManager);
            waveManager.StartWave();   
        }
    }
}
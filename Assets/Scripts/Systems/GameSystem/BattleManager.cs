using System;
using Agents.Enemies;
using Agents.Players;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Coin;
using Systems.GameSystem.Wave;
using UnityEngine;

namespace Systems.GameSystem
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private CurrentWaveInformationSO currentWaveInformation;
        [SerializeField] private WaveInformationSO fallbackWaveInformation;

        private WaveInformationSO _waveInformation;
        private bool _isBattleCleared;

        private void Awake()
        {
            Bus<BattleStartEvent>.OnEvent += HandleBattleStart;
            Bus<BattleEndEvent>.OnEvent += HandleBattleEnd;

            if (waveManager != null)
                waveManager.OnClear += HandleWaveClear;
        }

        private void OnDestroy()
        {
            Bus<BattleStartEvent>.OnEvent -= HandleBattleStart;
            Bus<BattleEndEvent>.OnEvent -= HandleBattleEnd;

            if (waveManager != null)
                waveManager.OnClear -= HandleWaveClear;
        }

        private void HandleBattleEnd(BattleEndEvent obj)
        {
            TryGiveClearReward();
            playerManager.AllPlayerDummy();
        }

        private void HandleBattleStart(BattleStartEvent obj)
        {
            Debug.Assert(enemyManager != null, "enemyManager is null");
            Debug.Assert(playerManager != null, "playerManager is null");
            Debug.Assert(waveManager != null, "waveManager is null");

            _isBattleCleared = false;
            _waveInformation = ResolveWaveInformation();
            
            //enemy 초기화
            enemyManager.ClearEnemies();
            
            //player 초기화
            playerManager.Init(enemyManager.EnemyRegister);
            
            //Wave 초기화
            waveManager.SetWaveInformation(_waveInformation);
            waveManager.SetEnemyManager(enemyManager);
            waveManager.StartWave();   
        }

        private void HandleWaveClear()
        {
            TryGiveClearReward();
            Bus<BattleEndEvent>.Raise(new BattleEndEvent());
        }

        private WaveInformationSO ResolveWaveInformation()
        {
            if (currentWaveInformation != null && currentWaveInformation.CurrentWaveInformation != null)
                return currentWaveInformation.CurrentWaveInformation;

            return fallbackWaveInformation;
        }

        private void TryGiveClearReward()
        {
            if (_isBattleCleared)
                return;

            _isBattleCleared = true;

            int getCoin = _waveInformation != null ? _waveInformation.GetCoin : 0;
            Debug.Log($"[Battle] Clear reward coin: {getCoin}", this);

            if (getCoin > 0)
                Bus<CoinEvent>.Raise(new CoinEvent(getCoin));
        }
    }
}

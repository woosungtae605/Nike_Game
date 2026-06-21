using Agents.Enemies;
using Agents.Players;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Coin;
using GameEvents.UI;
using Systems.GameSystem.Wave;
using Systems.SaveSystem;
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
        [SerializeField] private SaveFileNameSO waveClearSaveFile;
        [SerializeField] private PlayerDataSos playerDataSos;

        private WaveInformationSO _waveInformation;
        private bool _isBattleCleared;
        private bool _isBattleFailed;

        private void Awake()
        {
            Bus<BattleStartEvent>.OnEvent += HandleBattleStart;
            Bus<BattleEndEvent>.OnEvent += HandleBattleEnd;
            Bus<BattleFailEvent>.OnEvent += HandleBattleFail;

            if (waveManager != null)
                waveManager.OnClear += HandleWaveClear;
        }

        private void OnDestroy()
        {
            Bus<BattleStartEvent>.OnEvent -= HandleBattleStart;
            Bus<BattleEndEvent>.OnEvent -= HandleBattleEnd;
            Bus<BattleFailEvent>.OnEvent -= HandleBattleFail;

            if (waveManager != null)
                waveManager.OnClear -= HandleWaveClear;
        }

        private void HandleBattleEnd(BattleEndEvent obj)
        {
            if (_isBattleFailed)
                return;

            TryGiveClearReward();
            playerManager.AllPlayerDummy();
        }

        private void HandleBattleFail(BattleFailEvent obj)
        {
            if (_isBattleFailed || _isBattleCleared)
                return;

            _isBattleFailed = true;
            waveManager?.StopWave();
            playerManager.AllPlayerDummy();
            enemyManager?.ClearEnemies();
            Bus<FailUIEvent>.Raise(new FailUIEvent());
        }

        private void HandleBattleStart(BattleStartEvent obj)
        {
            Debug.Assert(enemyManager != null, "enemyManager is null");
            Debug.Assert(playerManager != null, "playerManager is null");
            Debug.Assert(waveManager != null, "waveManager is null");

            _isBattleCleared = false;
            _isBattleFailed = false;
            _waveInformation = ResolveWaveInformation();
            
            enemyManager.ClearEnemies();
            playerManager.Init(enemyManager.EnemyRegister);
            
            waveManager.SetWaveInformation(_waveInformation);
            waveManager.SetEnemyManager(enemyManager);
            waveManager.StartWave();
        }

        private void HandleWaveClear()
        {
            if (_isBattleFailed)
                return;

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
            if (_isBattleCleared || _isBattleFailed)
                return;

            _isBattleCleared = true;

            bool firstClear = _waveInformation != null && WaveClearSave.MarkCleared(waveClearSaveFile, _waveInformation);
            int getCoin = firstClear ? _waveInformation.GetCoin : 0;
            Debug.Log($"[Battle] Clear reward coin: {getCoin} / FirstClear: {firstClear}", this);

            if (getCoin > 0)
                Bus<CoinEvent>.Raise(new CoinEvent(getCoin));

            TryUnlockClearPlayer(firstClear);
        }

        private void TryUnlockClearPlayer(bool firstClear)
        {
            if (_waveInformation == null || playerDataSos == null || _waveInformation.UnlockPlayerData == null)
                return;

            if (!firstClear && playerDataSos.HasNowPlayerData(_waveInformation.UnlockPlayerData))
                return;

            bool unlocked = playerDataSos.UnlockPlayerData(_waveInformation.UnlockPlayerData);
            Debug.Log($"[Battle] Unlock player reward: {_waveInformation.UnlockPlayerData.NikkeName} / Unlocked: {unlocked}", this);
        }
    }
}



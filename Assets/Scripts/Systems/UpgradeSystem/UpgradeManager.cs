using System;
using System.Collections.Generic;
using Agents.Players;
using Systems.Coin;
using Systems.SaveSystem;
using UnityEngine;

namespace Systems.UpgradeSystem
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private SaveFileNameSO saveFileSo;

        [Header("Settings")]
        [SerializeField] private int maxLevel = 100;
        [SerializeField] private int baseCost = 100;
        [SerializeField] private int costIncrease = 50;
        [SerializeField] private int attackIncreasePerLevel = 1;
        [SerializeField] private int hpIncreasePerLevel = 10;
        
        public int MaxLevel => maxLevel;
        
        private readonly Dictionary<int, int> _levels = new();
        private bool _isLoaded;

        private void Awake()
        {
            Load();
        }

        public int GetLevel(PlayerDataSO playerData)
        {
            EnsureLoaded();

            if (playerData == null)
                return 0;

            return _levels.GetValueOrDefault(playerData.NikkeID, 0);
        }
        
        public int GetUpgradeCost(PlayerDataSO playerData)
        {
            int level = GetLevel(playerData);
            return baseCost + level * costIncrease;
        }

        public bool CanUpgrade(PlayerDataSO playerData)
        {
            return playerData != null && GetLevel(playerData) < maxLevel;
        }

        public bool TryUpgrade(PlayerDataSO playerData)
        {
            if (!CanUpgrade(playerData))
                return false;

            int currentLevel = GetLevel(playerData);
            _levels[playerData.NikkeID] = currentLevel + 1;

            Save();
            return true;
        }

        public bool TryUpgrade(PlayerDataSO playerData, CoinManager coinManager)
        {
            if (!CanUpgrade(playerData) || coinManager == null)
                return false;

            int cost = GetUpgradeCost(playerData);
            if (!coinManager.TryUseCoin(cost))
                return false;

            return TryUpgrade(playerData);
        }

        public int GetAttack(PlayerDataSO playerData)
        {
            if (playerData == null)
                return 0;

            return playerData.PlayerGunData.GunData.Damage + GetLevel(playerData) * attackIncreasePerLevel;
        }

        public int GetMaxHp(PlayerDataSO playerData)
        {
            if (playerData == null)
                return 0;

            return playerData.MaxHp + GetLevel(playerData) * hpIncreasePerLevel;
        }

        private void Save()
        {
            UpgradeSaveDatas saveDatas = new UpgradeSaveDatas();

            foreach ((int playerId, int level) in _levels)
            {
                saveDatas.upgradeSaveDatas.Add(new UpgradeSaveData(playerId, level));
            }

            JsonSaveService.Save(saveFileSo, saveDatas);
            _isLoaded = true;
        }

        private void Load()
        {
            _levels.Clear();
            _isLoaded = true;

            if (!JsonSaveService.TryLoad(saveFileSo, out UpgradeSaveDatas saveDatas))
                return;

            if (saveDatas?.upgradeSaveDatas == null)
                return;

            foreach (UpgradeSaveData saveData in saveDatas.upgradeSaveDatas)
            {
                _levels[saveData.playerId] = saveData.level;
            }
        }

        private void EnsureLoaded()
        {
            if (_isLoaded)
                return;

            Load();
        }
        
    }

    [Serializable]
    public class UpgradeSaveDatas
    {
        public List<UpgradeSaveData> upgradeSaveDatas = new();
    }
    [Serializable]
    public class UpgradeSaveData
    {
        public int playerId;
        public int level;

        public UpgradeSaveData(int playerId, int level)
        {
            this.playerId = playerId;
            this.level = level;
        }
    }
}

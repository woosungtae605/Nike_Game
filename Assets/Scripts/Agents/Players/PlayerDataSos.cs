using System.Collections.Generic;
using Systems.SaveSystem;
using UnityEngine;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "PlayerDatas", menuName = "SO/PlayerDatas", order = 0)]
    public class PlayerDataSos : ScriptableObject
    {
        [SerializeField] private SaveFileNameSO saveFileName;
        [SerializeField] private List<PlayerDataSO> allPlayerDatas = new List<PlayerDataSO>();
        private List<PlayerDataSO> _nowPlayerDatas = new List<PlayerDataSO>();

        public IReadOnlyList<PlayerDataSO> AllPlayerDatas => allPlayerDatas;
        public IReadOnlyList<PlayerDataSO> NowPlayerDatas => _nowPlayerDatas;

        private void OnEnable()
        {
            foreach (PlayerDataSO playerData in allPlayerDatas)
            {
                AddNowPlayerData(playerData);
            }
            Load();
        }

        public void SetNowPlayerDatas(IEnumerable<PlayerDataSO> playerDatas)
        {
            _nowPlayerDatas.Clear();

            foreach (PlayerDataSO playerData in playerDatas)
            {
                AddNowPlayerData(playerData, false);
            }

            Save();
        }

        public void AddNowPlayerData(PlayerDataSO playerData, bool save = true)
        {
            if (playerData == null || _nowPlayerDatas.Contains(playerData))
                return;

            _nowPlayerDatas.Add(playerData);

            if (save)
                Save();
        }

        public void RemoveNowPlayerData(PlayerDataSO playerData, bool save = true)
        {
            if (playerData == null)
                return;

            if (!_nowPlayerDatas.Remove(playerData))
                return;

            if (save)
                Save();
        }

        public void Save()
        {
            SaveData saveData = new SaveData();

            foreach (PlayerDataSO playerData in _nowPlayerDatas)
            {
                if (playerData == null)
                    continue;

                saveData.playerIds.Add(playerData.NikkeID);
            }

            JsonSaveService.Save(saveFileName, saveData);
        }

        public void Load()
        {
            _nowPlayerDatas.Clear();

            if (!JsonSaveService.TryLoad(saveFileName, out SaveData saveData))
                return;

            if (saveData == null || saveData.playerIds == null)
                return;

            foreach (int playerId in saveData.playerIds)
            {
                PlayerDataSO playerData = GetPlayerData(playerId);
                if (playerData != null)
                    AddNowPlayerData(playerData, false);
            }
        }

        public PlayerDataSO GetPlayerData(int nikkeId)
        {
            return allPlayerDatas.Find(playerData => playerData != null && playerData.NikkeID == nikkeId);
        }

        [System.Serializable]
        private class SaveData
        {
            public List<int> playerIds = new List<int>();
        }
    }
}

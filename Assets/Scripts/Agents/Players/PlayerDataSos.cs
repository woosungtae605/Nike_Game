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
        [SerializeField] private List<PlayerDataSO> nowPlayerDatas = new List<PlayerDataSO>();

        public IReadOnlyList<PlayerDataSO> AllPlayerDatas => allPlayerDatas;
        public IReadOnlyList<PlayerDataSO> NowPlayerDatas => nowPlayerDatas;

        private void OnEnable()
        {
            if (!TryLoad())
                Save();
        }

        public void SetNowPlayerDatas(IEnumerable<PlayerDataSO> playerDatas)
        {
            nowPlayerDatas.Clear();

            foreach (PlayerDataSO playerData in playerDatas)
            {
                AddNowPlayerData(playerData, false);
            }

            Save();
        }

        public bool HasNowPlayerData(PlayerDataSO playerData)
        {
            return playerData != null && nowPlayerDatas.Contains(playerData);
        }

        public bool UnlockPlayerData(PlayerDataSO playerData)
        {
            if (HasNowPlayerData(playerData))
                return false;

            AddNowPlayerData(playerData);
            return true;
        }

        public void AddNowPlayerData(PlayerDataSO playerData, bool save = true)
        {
            if (playerData == null || nowPlayerDatas.Contains(playerData))
                return;

            nowPlayerDatas.Add(playerData);

            if (save)
                Save();
        }

        public void RemoveNowPlayerData(PlayerDataSO playerData, bool save = true)
        {
            if (playerData == null)
                return;

            if (!nowPlayerDatas.Remove(playerData))
                return;

            if (save)
                Save();
        }

        public void Save()
        {
            SaveData saveData = new SaveData();

            foreach (PlayerDataSO playerData in nowPlayerDatas)
            {
                if (playerData == null)
                    continue;

                saveData.playerIds.Add(playerData.NikkeID);
            }

            JsonSaveService.Save(saveFileName, saveData);
        }

        public void Load()
        {
            TryLoad();
        }

        private bool TryLoad()
        {
            if (!JsonSaveService.TryLoad(saveFileName, out SaveData saveData))
                return false;

            nowPlayerDatas.Clear();

            if (saveData == null || saveData.playerIds == null)
                return false;

            foreach (int playerId in saveData.playerIds)
            {
                PlayerDataSO playerData = GetPlayerData(playerId);
                if (playerData != null)
                    AddNowPlayerData(playerData, false);
            }

            return true;
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

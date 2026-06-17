using System;
using System.Collections.Generic;
using Systems.SaveSystem;
using UnityEngine;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "PlayerSquad", menuName = "SO/PlayerSquad", order = 0)]
    public class PlayerSquadSO : ScriptableObject
    {
        private const int SquadCount = 5;
        private const int EmptyId = -1;

        [SerializeField] private SaveFileNameSO saveFileName;
        [SerializeField] private PlayerDataSos playerDataSos;

        public PlayerDataSO[] PlayerDataSos { get; private set; } = new PlayerDataSO[SquadCount];
        public event Action OnChanged;

        private void OnEnable()
        {
            EnsureSquadSize();
            Load();
        }

        public bool Equip(PlayerDataSO playerData)
        {
            if (playerData == null || IsEquipped(playerData))
                return false;

            int emptyIndex = GetFirstEmptyIndex();
            if (emptyIndex < 0)
                return false;

            return Equip(emptyIndex, playerData);
        }

        public bool Equip(int index, PlayerDataSO playerData)
        {
            if (!IsValidIndex(index) || playerData == null)
                return false;

            if (IsEquipped(playerData))
                return false;

            PlayerDataSos[index] = playerData;
            SaveAndNotify();
            return true;
        }

        public void Unequip(int index)
        {
            if (!IsValidIndex(index) || PlayerDataSos[index] == null)
                return;

            PlayerDataSos[index] = null;
            SaveAndNotify();
        }

        public bool IsEquipped(PlayerDataSO playerData)
        {
            if (playerData == null)
                return false;

            foreach (PlayerDataSO squadPlayer in PlayerDataSos)
            {
                if (squadPlayer == playerData)
                    return true;
            }

            return false;
        }

        public void Save()
        {
            if (saveFileName == null)
                return;

            SaveData saveData = new SaveData();

            for (int i = 0; i < SquadCount; i++)
            {
                PlayerDataSO playerData = PlayerDataSos[i];
                saveData.playerIds.Add(playerData != null ? playerData.NikkeID : EmptyId);
            }

            JsonSaveService.Save(saveFileName, saveData);
        }

        public void Load()
        {
            EnsureSquadSize();
            Clear(false);

            if (saveFileName == null || playerDataSos == null)
                return;

            if (!JsonSaveService.TryLoad(saveFileName, out SaveData saveData))
                return;

            if (saveData == null || saveData.playerIds == null)
                return;

            int count = Mathf.Min(SquadCount, saveData.playerIds.Count);
            for (int i = 0; i < count; i++)
            {
                int playerId = saveData.playerIds[i];
                if (playerId == EmptyId)
                    continue;

                PlayerDataSos[i] = playerDataSos.GetPlayerData(playerId);
            }

            OnChanged?.Invoke();
        }

        public void Clear(bool save = true)
        {
            EnsureSquadSize();

            for (int i = 0; i < SquadCount; i++)
            {
                PlayerDataSos[i] = null;
            }

            if (save)
                SaveAndNotify();
        }

        private int GetFirstEmptyIndex()
        {
            for (int i = 0; i < SquadCount; i++)
            {
                if (PlayerDataSos[i] == null)
                    return i;
            }

            return -1;
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < SquadCount;
        }

        private void SaveAndNotify()
        {
            Save();
            OnChanged?.Invoke();
        }

        private void EnsureSquadSize()
        {
            if (PlayerDataSos == null || PlayerDataSos.Length != SquadCount)
                PlayerDataSos = new PlayerDataSO[SquadCount];
        }

        [Serializable]
        private class SaveData
        {
            public List<int> playerIds = new List<int>();
        }
    }
}

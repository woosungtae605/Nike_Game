using System;
using System.Collections.Generic;
using Systems.SaveSystem;
using UnityEngine;

namespace Systems.GameSystem.Wave
{
    [CreateAssetMenu(fileName = "WaveInformation", menuName = "SO/WaveInformation", order = 0)]
    public class WaveInformationSO : ScriptableObject
    {
        [field: SerializeField] public WaveDataSo WaveData { get; private set; }
        [field: SerializeField] public string WaveName { get; private set; }
        [field: SerializeField] public int WaveNumber { get; private set; }
        [field: SerializeField] public int GetCoin { get; private set; }
    }

    [Serializable]
    public class WaveClearSaveData
    {
        public List<int> clearedWaveNumbers = new List<int>();
    }

    public static class WaveClearSave
    {
        private static WaveClearSaveData _cache;
        private static SaveFileNameSO _cacheSaveFile;

        public static bool IsCleared(SaveFileNameSO saveFile, WaveInformationSO waveInformation)
        {
            if (saveFile == null || waveInformation == null)
                return false;

            WaveClearSaveData data = Load(saveFile);
            return data.clearedWaveNumbers.Contains(waveInformation.WaveNumber);
        }

        public static bool MarkCleared(SaveFileNameSO saveFile, WaveInformationSO waveInformation)
        {
            if (saveFile == null || waveInformation == null)
                return false;

            WaveClearSaveData data = Load(saveFile);
            if (data.clearedWaveNumbers.Contains(waveInformation.WaveNumber))
                return false;

            data.clearedWaveNumbers.Add(waveInformation.WaveNumber);
            Save(saveFile, data);
            return true;
        }

        public static bool HasAnyCleared(SaveFileNameSO saveFile)
        {
            if (saveFile == null)
                return false;

            WaveClearSaveData data = Load(saveFile);
            return data.clearedWaveNumbers != null && data.clearedWaveNumbers.Count > 0;
        }

        private static WaveClearSaveData Load(SaveFileNameSO saveFile)
        {
            if (_cache != null && _cacheSaveFile == saveFile)
                return _cache;

            _cacheSaveFile = saveFile;

            if (!JsonSaveService.TryLoad(saveFile, out _cache))
                _cache = new WaveClearSaveData();

            return _cache;
        }

        private static void Save(SaveFileNameSO saveFile, WaveClearSaveData data)
        {
            _cacheSaveFile = saveFile;
            _cache = data;
            JsonSaveService.Save(saveFile, data);
        }
    }
}
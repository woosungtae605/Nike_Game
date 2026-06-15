using System;
using System.IO;
using UnityEngine;

namespace Systems.SaveSystem
{
    public class JsonSaveService : MonoBehaviour
    {
        public static void Save<T>(SaveFileNameSO saveFileName, T data)
        {
            if (saveFileName == null)
            {
                Debug.LogError("SaveFileNameSO is null.");
                return;
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFileName.SavePath, json);
        }

        public static bool TryLoad<T>(SaveFileNameSO saveFileName, out T data)
        {
            data = default;

            if (saveFileName == null)
            {
                Debug.LogError("SaveFileNameSO is null.");
                return false;
            }

            if (!File.Exists(saveFileName.SavePath))
                return false;

            try
            {
                string json = File.ReadAllText(saveFileName.SavePath);
                data = JsonUtility.FromJson<T>(json);
                return data != null;
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to load json save. Path: {saveFileName.SavePath}\n{exception}");
                return false;
            }
        }

        public static bool Exists(SaveFileNameSO saveFileName)
        {
            return saveFileName != null && File.Exists(saveFileName.SavePath);
        }

        public static void Delete(SaveFileNameSO saveFileName)
        {
            if (saveFileName == null || !File.Exists(saveFileName.SavePath))
                return;

            File.Delete(saveFileName.SavePath);
        }
    }
}

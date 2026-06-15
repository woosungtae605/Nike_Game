using System.IO;
using UnityEngine;

namespace Systems.SaveSystem
{
    [CreateAssetMenu(fileName = "SavePath", menuName = "SO/Save/SavePath", order = 0)]
    public class SaveFileNameSO : ScriptableObject
    {
        [SerializeField] private string savePath;
        private const string Extension = ".json";
        private const string SaveFolder = "Saves";
        
        public string SavePath
        {
            get
            {
                string folderPath = Path.Combine(Application.persistentDataPath, SaveFolder);
                Directory.CreateDirectory(folderPath);

                return Path.Combine(folderPath, savePath + Extension);
            }
        }
    }
}
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.GameSystem.Wave.Editor
{
    [CustomEditor(typeof(WaveDataSo))]
    public class WaveDataSoEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            Button button = new Button();
            button.clicked += HandleOnClick;
            button.text = "nameChange";
            root.Add(button);
            
            return root;
        }

        private void HandleOnClick()
        { 
            WaveDataSo waveDataSo = target as WaveDataSo;
            Debug.Assert(waveDataSo != null, "waveDataSo is null");

            if (waveDataSo.SpawnDataSos == null)
                return;

            for (int i = 0; i < waveDataSo.SpawnDataSos.Length; i++)
            {
                SpawnDataSo spawnDataSo = waveDataSo.SpawnDataSos[i];
                if (spawnDataSo == null)
                    continue;

                string path = AssetDatabase.GetAssetPath(spawnDataSo);
                AssetDatabase.RenameAsset(path, $"__TempSpawnData{i + 1}");
            }

            AssetDatabase.SaveAssets();

            for (int i = 0; i < waveDataSo.SpawnDataSos.Length; i++)
            {
                SpawnDataSo spawnDataSo = waveDataSo.SpawnDataSos[i];
                if (spawnDataSo == null)
                    continue;

                string path = AssetDatabase.GetAssetPath(spawnDataSo);
                string error = AssetDatabase.RenameAsset(path, $"SpawnData{i + 1}");

                if (!string.IsNullOrEmpty(error))
                    Debug.LogError(error, spawnDataSo);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
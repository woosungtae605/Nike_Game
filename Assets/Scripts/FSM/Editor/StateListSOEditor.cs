using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    
    public static class CodeFormat
    {
        public static string EnumFormat =
            @"namespace Agents.FSM
{{
    public enum {0}
    {{
        {1}
    }}
}}";
    }

    
    [CustomEditor(typeof(StateListSO))]
    public class StateListSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default;
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            editorView.CloneTree(root);

            root.Q<Button>("GenerateButton").clicked += HandleGenerateEnumClick;
            return root;
        }

        private void HandleGenerateEnumClick()
        {
            StateListSO listData = target as StateListSO;

            int index = 0;
            string enumString = string.Join(",", listData.states.Select(so =>
            {
                so.stateIndex = index;
                EditorUtility.SetDirty(so);
                return $"{so.stateName} = {index++}";
            }));
 
            string code = string.Format(CodeFormat.EnumFormat, listData.enumName , enumString);
 
            string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            string directoryName = Path.GetDirectoryName(scriptPath); //이 코드가 있는 디렉토리를 가져와.
            DirectoryInfo parentDirectory = Directory.GetParent(directoryName); // 이 코드의 디렉토리의 부모 디렉토리를 가져와

            string path = parentDirectory.FullName;
            File.WriteAllText($"{path}/{listData.enumName}.cs", code);
 
            AssetDatabase.SaveAssets(); // ctrl + s키랑 같은 기능
            AssetDatabase.Refresh(); // 갱신 -> 이걸 해야 새롭게 컴파일을 한다.
        }
    }
}
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    namespace Agents.Players.Gun.Editor
    {
        [CustomEditor(typeof(PlayerGunDataSO))]
        public class GunSoEditor : UnityEditor.Editor
        {
            [SerializeField] private VisualTreeAsset visualTree;
            private Dictionary<string, Type> _typeMap;

            private void OnEnable()
            {
                BuildTypeMap();
            }

            public override VisualElement CreateInspectorGUI()
            {
                VisualElement root = new  VisualElement();
                visualTree.CloneTree(root);
                
                
                DropdownField dropdown = root.Q<DropdownField>("Dropdown");
                
                dropdown.choices = _typeMap.Keys.ToList();
                
                dropdown.RegisterValueChangedCallback(evt =>
                {
                    if (string.IsNullOrEmpty(evt.newValue) || !_typeMap.TryGetValue(evt.newValue, out var type)) 
                        return;
                    
                    serializedObject.Update();
                    SerializedProperty prop = serializedObject.FindProperty("gunData");
                    
                    if (prop.managedReferenceValue?.GetType().Name == evt.newValue)
                        return;
                    
                    prop.managedReferenceValue = Activator.CreateInstance(type);
                    serializedObject.ApplyModifiedProperties();
                    Debug.Log($"타입 변경 완료: {evt.newValue}");
                });
                return root;
            }

            private void BuildTypeMap()
            {
                Assembly gunAssembly = Assembly.GetAssembly(typeof(GunData.GunData));
                
                _typeMap = gunAssembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(GunData.GunData)))
                    .ToDictionary(type => type.Name);
            }
        }
    }
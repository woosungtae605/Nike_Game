using System;
using System.Linq;
using Tools.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class StageEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset editorView = default;

    private ObjectField _rootObjectField;
    private ObjectField _prefabListField;
    private VisualElement _itemSelectContainer;
    private DropdownField _itemDropdownField;
    private VisualElement _previewImage;
    private IntegerField _cellSizeField; //1
    
    private static GameObject _rootObject;
    private static StagePrefabListSO _prefabList;
    private static int _cellSize = 5;  //2

    private bool _isReadyToPlacement = false;
    private GameObject _selectedPrefab;
    
    [MenuItem("Tools/StageEditorWindow")]
    public static void ShowWindow()
    {
        StageEditorWindow wnd = GetWindow<StageEditorWindow>();
        wnd.titleContent = new GUIContent("StageEditorWindow");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += HandleOnSceneGui;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= HandleOnSceneGui;
    }

    private void HandleOnSceneGui(SceneView sceneView)
    {
        if (!_isReadyToPlacement) return;
        
        Event evt = Event.current;
        
        Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); //넓이는 무제한 평면

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 snappedPoint = new Vector3(
                Mathf.Floor(worldPoint.x / _cellSize) * _cellSize + _cellSize * 0.5f,
                0,
                Mathf.Floor(worldPoint.z / _cellSize) * _cellSize + _cellSize * 0.5f
            );
            
            Handles.color = Color.green;
            Handles.DrawWireCube(snappedPoint, new Vector3(_cellSize, 0.01f, _cellSize));

            if (evt.type == EventType.MouseDown && evt.button == 0)
            {
                PlacePrefab(snappedPoint);
                evt.Use(); //이벤트를 소모시켜서 기본동작을 못하게 만든다.
            }
            
            sceneView.Repaint();
        }
    }

    private void PlacePrefab(Vector3 snappedPoint)
    {
        if (_selectedPrefab == null || !_isReadyToPlacement)
            return;
        
        Vector3 pivotOffset = new Vector3(_cellSize * 0.5f, 0, -_cellSize * 0.5f);
        Vector3 placedPoint = snappedPoint + pivotOffset;
        
        GameObject newInstance = PrefabUtility.InstantiatePrefab(_selectedPrefab, _rootObject.transform)
            as GameObject;
        Debug.Assert(newInstance != null, "게임오브젝트 기반이 아닌 프리팹은 생성할 수 없습니다.");
        
        newInstance.transform.position = placedPoint;
        
        Undo.RegisterCreatedObjectUndo(newInstance, $"Placed prefab {newInstance.name}");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        editorView.CloneTree(root);
        
        _rootObjectField = root.Q<ObjectField>("RootObjectField");
        _rootObjectField.RegisterValueChangedCallback(HandleRootObjectChange);

        _itemSelectContainer = root.Q<VisualElement>("ItemSelectContainer");
        _itemDropdownField = root.Q<DropdownField>("ItemDropdownField");
        
        _prefabListField = root.Q<ObjectField>("PrefabListObjectField");
        _prefabListField.RegisterValueChangedCallback(HandlePrefabListChange);
        
        _previewImage = root.Q<VisualElement>("PreviewImage");
        _itemDropdownField.RegisterValueChangedCallback(HandleItemSelect);
        
        _cellSizeField = root.Q<IntegerField>("CellSizeIntField");
        _cellSizeField.RegisterValueChangedCallback(evt => { _cellSize = evt.newValue; });
        
        _cellSizeField.SetValueWithoutNotify(_cellSize);
        
        if(_rootObject != null)
            _rootObjectField.SetValueWithoutNotify(_rootObject);
        if(_prefabList != null)
            _prefabListField.SetValueWithoutNotify(_prefabList);

        CheckItemSelectContainerActive();
    }

    private void HandleItemSelect(ChangeEvent<string> evt)
    {
        if (string.IsNullOrEmpty(evt.newValue))
        {
            _previewImage.style.backgroundImage = null;
            _isReadyToPlacement = false;
            _selectedPrefab = null;
            return;
        }
        
        _selectedPrefab = _prefabList.prefabs[_itemDropdownField.index]; //현재 선택된 인덱스의 프리팹을 가져온다.
        
        Texture2D preview = AssetPreview.GetAssetPreview(_selectedPrefab);
        //선택한 게임 오브젝트의 프리뷰 이미지를 얻는다.
        if (preview != null)
        {
            _previewImage.style.backgroundImage = preview;
        }
        else
        {
            _previewImage.schedule.Execute(() =>
            {
                preview = AssetPreview.GetAssetPreview(_selectedPrefab);
                if (preview != null)
                {
                    _previewImage.style.backgroundImage = preview;
                }
            }).Until(() => !AssetPreview.IsLoadingAssetPreview(_selectedPrefab.GetInstanceID()));
        }
        
        _isReadyToPlacement = true;
    }

    private void CheckItemSelectContainerActive()
    {
        
        bool isReady = _rootObject != null && _prefabList != null;
        _itemSelectContainer.style.display = isReady ? DisplayStyle.Flex : DisplayStyle.None;

        if (isReady)
        {
            _itemDropdownField.choices.Clear(); //이건 List<string>
            _itemDropdownField.choices.AddRange(
                _prefabList.prefabs.Select(prefab => prefab.name));
        }
        else
        {
            _isReadyToPlacement = false;
        }
    }

    private void HandlePrefabListChange(ChangeEvent<Object> evt)
    {
        _prefabList = evt.newValue as StagePrefabListSO;
        CheckItemSelectContainerActive();
    }

    private void HandleRootObjectChange(ChangeEvent<Object> evt)
    {
        //Debug.Log($"{evt.previousValue} 에서 {evt.newValue} 로 변화");
        GameObject newRootObject = evt.newValue as GameObject;

        if (PrefabUtility.IsPartOfPrefabAsset(newRootObject))
        {
            _rootObjectField.SetValueWithoutNotify(evt.previousValue); //이전값으로 되돌리고
            EditorUtility.DisplayDialog("Error", 
                "루트 오브젝트는 반드시 씬에 있는 오브젝트여야 합니다.", "OK");
            return;
        }
        
        _rootObject = newRootObject;
        CheckItemSelectContainerActive();
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Tools.Editor
{
    [CreateAssetMenu(fileName = "Stage prefab list", menuName = "SO/Stage prefabs", order = 10)]
    public class StagePrefabListSO : ScriptableObject
    {
        public List<GameObject> prefabs = new();
    }
}
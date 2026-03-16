using Systems.AnimationSystems;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "FSM/State data", order = 0)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public int stateIndex;
        public AnimParamSO stateParam;
    }
}
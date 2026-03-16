using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(fileName = "FSM state manager", menuName = "FSM/State list", order = 0)]
    public class StateListSO : ScriptableObject
    {
        public string enumName;
        public StateSO[] states;
    }
}
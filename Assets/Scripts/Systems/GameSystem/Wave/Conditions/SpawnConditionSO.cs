using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    public abstract class SpawnConditionSO : ScriptableObject
    {
        public abstract void Initialize();
        public abstract bool GoToNext();
    }
}
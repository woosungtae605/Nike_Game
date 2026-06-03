using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    public abstract class SpawnConditionSO : ScriptableObject
    {
        protected SpawnConditionContext Context { get; private set; }

        public virtual void Initialize(SpawnConditionContext context)
        {
            Context = context;
        }
        public abstract bool GoToNext();
    }
}
using UnityEngine;

namespace Systems.GameSystem.Wave.Conditions
{
    [CreateAssetMenu(fileName = "SpawnDelay", menuName = "Wave/Condition/SpawnDelay", order = 0)]
    public class SpawnDelayConditionSO : SpawnConditionSO
    {
        [SerializeField] private float delay;
        private float _enterTime;
        
        public override void Initialize()
        {
            _enterTime = Time.time;
        }

        public override bool GoToNext()
        {
            return _enterTime + delay <= Time.time;
        }
    }
}
using System;
using Unity.Behavior;
using UnityEngine;

namespace Agents.Enemies.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "repeatTimeOperation ", story: "[RepeatCount] [Operation] [Count]", category: "Conditions", id: "2cdb2a3ac02348e94bd57d1dcef4fb69")]
    public partial class RepeatTimeOperationCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<int> RepeatCount;
        [Comparison(comparisonType: ComparisonType.All)]
        [SerializeReference] public BlackboardVariable<ConditionOperator> Operation;
        [SerializeReference] public BlackboardVariable<int> Count;

        public override bool IsTrue()
        {
            if (RepeatCount == null || Operation == null || Count == null)
            {
                return false;
            }

            return ConditionUtils.Evaluate(RepeatCount.Value, Operation, Count.Value);
        }

    }
}

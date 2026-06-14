using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResetRepeatCount", story: "Set [RepeatCount] to [Value]", category: "Action", id: "a0611dfdfe8a3bd3aedd635cd45429f8")]
public partial class ResetRepeatCountAction : Action
{
    [SerializeReference] public BlackboardVariable<int> RepeatCount;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        if (RepeatCount == null)
        {
            return Status.Failure;
        }

        if (Value == null)
        {
            return Status.Failure;
        }

        RepeatCount.Value = Value.Value;

        return Status.Success;
    }
}


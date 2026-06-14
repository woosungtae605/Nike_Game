using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AddRepeatCount", story: "Add [Amount] to [RepeatCount]", category: "Action", id: "ac1661e7c5c642d40c3f281f4833dd74")]
public partial class AddRepeatCountAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Amount;
    [SerializeReference] public BlackboardVariable<int> RepeatCount;

    protected override Status OnStart()
    {
        if (RepeatCount == null)
        {
            return Status.Failure;
        }

        int addValue = 1;

        if (Amount != null)
        {
            addValue = Amount.Value;
        }

        RepeatCount.Value += addValue;

        return Status.Success;
    }
}


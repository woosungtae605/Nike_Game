using Agents.Enemies.IronCladDesers;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IronCladDeserFirstRunTrue", story: "[IronCladDeser] FirstRun [Value]", category: "Action", id: "9e87eb96a3852181289b5d9986003224")]
public partial class IronCladDeserFirstRunTrueAction : Action
{
    [SerializeReference] public BlackboardVariable<IronCladDeser> IronCladDeser;
    [SerializeReference] public BlackboardVariable<bool> Value;

    protected override Status OnStart()
    {
        if (IronCladDeser == null && IronCladDeser.Value == null)
            return Status.Failure;

        IronCladDeser.Value.SetFIrstRun(Value.Value);
        return Status.Success;
    }
}


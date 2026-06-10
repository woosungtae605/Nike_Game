using Agents.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "GotoLeft", story: "[Enemy] go to left", category: "Conditions", id: "eea96deadd6c5b3a0c64e7232040cfa5")]
public partial class GotoLeftCondition : Condition
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

    public override bool IsTrue()
    {
        return Enemy.Value.GotoLeft;
    }
}

using System;
using Unity.Behavior;
using UnityEngine;

namespace Agents.Enemies.IronCladDesers.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckIronCladFirstrun ", story: "[IronCladDesert] is first run", category: "Conditions", id: "b0bb9d69dffb2e09023c900e669b0318")]
    public partial class CheckIronCladFirstrunCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<IronCladDeser> IronCladDesert;

        public override bool IsTrue()
        {
            return IronCladDesert.Value.FirstRun;
        }
    }
}

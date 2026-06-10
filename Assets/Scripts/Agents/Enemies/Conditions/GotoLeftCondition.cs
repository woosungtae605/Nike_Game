using System;
using Unity.Behavior;
using UnityEngine;

namespace Agents.Enemies.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "GotoLeft", story: "[Enemy] go to left", category: "Conditions", id: "eea96deadd6c5b3a0c64e7232040cfa5")]
    public partial class GotoLeftCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        public override bool IsTrue()
        {
            if (Enemy == null || Enemy.Value == null)
            {
                return false;
            }

            bool gotoLeft = Enemy.Value.GotoLeft;
            return gotoLeft;
        }
    }
}

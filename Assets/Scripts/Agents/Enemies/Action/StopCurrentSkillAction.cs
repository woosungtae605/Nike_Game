using System;
using Agents.Enemies.Module;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopCurrentSkill", story: "[Enemy] stop current skill", category: "Action/Skill", id: "5989fb3c8c874aa0a625ce0192a4f29a")]
    public partial class StopCurrentSkillAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        protected override Status OnStart()
        {
            if (Enemy == null || Enemy.Value == null)
                return Status.Failure;

            EnemySkillModule skillModule = Enemy.Value.GetModule<EnemySkillModule>();
            if (skillModule == null)
                return Status.Failure;

            skillModule.StopCurrentSkill();
            return Status.Success;
        }
    }
}

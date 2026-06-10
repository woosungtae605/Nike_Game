using System;
using Agents.Enemies.Module;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "UseSkill", story: "[Enemy] use skill [SkillIndex] on [Target]", category: "Action/Skill", id: "3aeb199a86d24f17808a5883d6300e03")]
    public partial class UseSkillAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<int> SkillIndex;
        [SerializeReference] public BlackboardVariable<GameObject> Target;

        protected override Status OnStart()
        {
            if (Enemy == null || Enemy.Value == null)
                return Status.Failure;

            if (SkillIndex == null)
                return Status.Failure;

            EnemySkillModule skillModule = Enemy.Value.GetModule<EnemySkillModule>();
            if (skillModule == null)
                return Status.Failure;

            GameObject target = Target?.Value;
            if (!skillModule.CanUseSkill(SkillIndex.Value, target))
                return Status.Failure;

            skillModule.UseSkill(SkillIndex.Value, target);
            return Status.Success;
        }
    }
}

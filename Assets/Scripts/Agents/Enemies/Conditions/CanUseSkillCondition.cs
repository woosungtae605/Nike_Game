using System;
using Agents.Enemies.Module;
using Unity.Behavior;
using UnityEngine;

namespace Agents.Enemies.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CanUseSkill", story: "[Enemy] can use skill [SkillIndex] on [Target]", category: "Conditions/Skill", id: "4dbbc4ad89a2410a9abfa77ec6b682dd")]
    public partial class CanUseSkillCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<int> SkillIndex;
        [SerializeReference] public BlackboardVariable<GameObject> Target;

        public override bool IsTrue()
        {
            if (Enemy == null || Enemy.Value == null)
                return false;

            EnemySkillModule skillModule = Enemy.Value.GetModule<EnemySkillModule>();
            if (skillModule == null)
                return false;

            if (SkillIndex == null)
                return false;

            GameObject target = Target?.Value;
            return skillModule.CanUseSkill(SkillIndex.Value, target);
        }
    }
}

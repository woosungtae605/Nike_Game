using System;
using Systems.AnimationSystems;
using Unity.Behavior;
using UnityEngine;

namespace Agents.Enemies.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "ClipCompare", story: "[Enemy] clip [Operator] [Target]", category: "Conditions", id: "1f5e86822fb357aa1a3a7a729486685b")]
    public partial class ClipCompareCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [Comparison(comparisonType: ComparisonType.Boolean)]
        [SerializeReference] public BlackboardVariable<ConditionOperator> Operator;
        [SerializeReference] public BlackboardVariable<AnimParamSO> Target;

        public override bool IsTrue()
        {
            if (Enemy.Value == null || Enemy.Value.Renderer == null)
                return false;

            AnimatorStateInfo stateInfo = Enemy.Value.Renderer.Animator.GetCurrentAnimatorStateInfo(0);
            
        
            return Operator.Value switch {
                ConditionOperator.Equal => stateInfo.shortNameHash == Target.Value.ParamHash,
                ConditionOperator.NotEqual => stateInfo.shortNameHash != Target.Value.ParamHash,
                _ => false
            };
        }
    }
}

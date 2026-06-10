using Agents.Enemies;
using System;
using Agents.Enemies.Module;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "EnemySKillEnd", story: "[Enemy] skill [Index] end", category: "Conditions", id: "759d9a0ce969111acf333b56b650cddc")]
public partial class EnemySKillEndCondition : Condition
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<int> Index;

    private EnemySkillModule _enemySkillModule;
    
    public override bool IsTrue()
    {
        return !Enemy.Value.EnemySkillModule.SkillDict[Index].IsUsing;
    }
}

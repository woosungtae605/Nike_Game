using System;
using Agents.CombatSystem;
using Agents.Enemies.Module;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SkillCoolTimeInitialization", story: "[Enemy] initialize all skill count", category: "Action", id: "703f836d12fddc89b4a4da3bcd3dee61")]
    public partial class SkillCoolTimeInitializationAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        private EnemySkillModule _enemySkill;
        protected override Status OnStart()
        {
            if (Enemy == null || Enemy.Value == null)
                return Status.Failure;

            _enemySkill = Enemy.Value.EnemySkillModule;
            
            if(_enemySkill == null)
                return Status.Failure;

            foreach (ISkill skill in _enemySkill.SkillDict.Values)
            {
                skill.StopSkill();
            }
            
            return Status.Success;
        }
    }
}


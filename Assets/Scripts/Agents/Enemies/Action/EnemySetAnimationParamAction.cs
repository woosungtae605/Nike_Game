using Agents.Enemies;
using System;
using Systems.AnimationSystems;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemySetAnimationParam", story: "[Enemy] set [AnimParam] [Value]", category: "Action/Animation", id: "b12e5bedae1d0dea225d43e872759118")]
public partial class EnemySetAnimationParamAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<AnimParamSO> AnimParam;
    [SerializeReference] public BlackboardVariable<float> Value;

    private IRenderer _renderer;

    protected override Status OnStart()
    {
        if (Enemy == null || Enemy.Value == null || AnimParam.Value == null)
            return Status.Failure;

        _renderer = Enemy.Value.Renderer;

        _renderer.SetFloat(AnimParam.Value, Value.Value);
        return Status.Success;
    }
}


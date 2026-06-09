using System;
using Systems.AnimationSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.DoomShotEnemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Play Clip", story: "[Enemy] play [Clip] at [Layer] and [Position]", category: "Action/Animation", id: "242916ccff44d3b0888a96ac0c919f2c")]
    public partial class PlayClipAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<AnimParamSO> Clip;
        [SerializeReference] public BlackboardVariable<int> Layer;
        [SerializeReference] public BlackboardVariable<float> Position;

        [SerializeReference] public BlackboardVariable<float> CrossDuration = new(0.1f);

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.Renderer == null || Clip.Value == null)
                return Status.Failure;

            Enemy.Value.Renderer.PlayClip(Clip.Value.ParamHash,
                Position.Value, CrossDuration.Value, Layer.Value);

            return Status.Success;
        }
    }
}


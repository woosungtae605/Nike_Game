using System;
using Systems.AnimationSystems;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
namespace Agents.Enemies.DoomShotEnemies.Events
{
    [CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationChannel", message: "Play [Clip]", category: "Events", id: "7982c2312a1441c67dc7630fc48f8ce5")]
    public sealed partial class AnimationChannel : EventChannel<AnimParamSO> { }
}


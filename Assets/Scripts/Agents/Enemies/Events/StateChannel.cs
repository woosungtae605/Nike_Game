using Agents.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StateChannel")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StateChannel", message: "Change [State]", category: "Events", id: "1c57067c5f434c80ce910b6fc0f3bf17")]
public sealed partial class StateChannel : EventChannel<EnemyState> { }


using System.Collections.Generic;
using UnityEngine;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "PlayerSquad", menuName = "SO/PlayerSquad", order = 0)]
    public class PlayerSquadSO : ScriptableObject
    {
        public PlayerDataSO[] PlayerDataSos { get; private set; } = new PlayerDataSO[5];
    }
}
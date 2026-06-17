using System.Collections.Generic;
using UnityEngine;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "PlayerSquad", menuName = "SO/PlayerSquad", order = 0)]
    public class PlayerSquadSO : ScriptableObject
    {
        List<PlayerDataSO>  playerDataSos = new List<PlayerDataSO>();
    }
}
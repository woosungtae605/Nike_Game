using System.Collections.Generic;
using UnityEngine;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "PlayerDatas", menuName = "SO/PlayerDatas", order = 0)]
    public class PlayerDataSos : ScriptableObject
    {
        [SerializeField] private List<PlayerDataSO> allPlayerDatas = new List<PlayerDataSO>();
        private List<PlayerDataSO>  _nowPlayerDatas = new List<PlayerDataSO>();
    }
}
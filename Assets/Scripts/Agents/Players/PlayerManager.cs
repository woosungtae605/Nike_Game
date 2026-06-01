using System.Collections.Generic;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private List<Player> playerList;
        public Player CurrentPlayer { get; private set; }

        public void ChangePlayer(int index)
        {
            CurrentPlayer = playerList[index];
        }
    }
}
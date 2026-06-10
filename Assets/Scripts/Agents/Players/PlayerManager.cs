using System;
using System.Collections.Generic;
using Agents.Enemies;
using CoreSystem.BusSystem;
using FSM;
using GameEvents.Camera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Agents.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private List<Player> playerList;
        public IReadOnlyList<Player> Players => playerList;
        public Player CurrentPlayer { get; private set; }

        private EnemyRegisterSo _enemyRegisterSo;

        private void Awake()
        {
            Debug.Assert(playerList != null && playerList.Count > 0, "Player list is empty");
        }

        public void Init(EnemyRegisterSo enemyRegisterSo)
        {
            _enemyRegisterSo = enemyRegisterSo;
            foreach (Player player in playerList)
            {
                player.SetEnemyRegister(_enemyRegisterSo);
                player.PlayerNotControl();
            }
            ChangePlayer(0);
        }

        private void Update()
        {
            if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
            {
                ChangePlayer(0);
            }
            else if(Keyboard.current[Key.Digit2].wasPressedThisFrame)
            {
                ChangePlayer(1);
            }
        }

        private void ChangePlayer(int index)
        {
            if (index < 0 || index >= playerList.Count)
            {
                Debug.LogError($"Player index {index} is out of range");
                return;   
            }
            if(CurrentPlayer != null)
                CurrentPlayer.PlayerNotControl();
            
            CurrentPlayer = playerList[index];
            CurrentPlayer.PlayerControl();
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(playerList[index].CameraTransform));
        }
    }
}

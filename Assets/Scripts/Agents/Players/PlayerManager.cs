using System;
using System.Collections.Generic;
using Agents.Enemies;
using Agents.FSM;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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

        private void Start()
        {
            AllPlayerDummy();
        }

        public void AllPlayerDummy()
        {
            foreach (Player player in playerList)
            {
                player.ChangeState(PlayerStates.Dummy);
            }
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

        public Player GetClosestPlayer(Vector3 origin)
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in playerList)
            {
                if (player == null || !player.gameObject.activeInHierarchy)
                    continue;

                float distance = (player.transform.position - origin).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }

        public Player GetRandomPlayer()
        {
            List<Player> activePlayers = new List<Player>();

            foreach (Player player in playerList)
            {
                if (player == null || !player.gameObject.activeInHierarchy)
                    continue;

                activePlayers.Add(player);
            }

            if (activePlayers.Count == 0)
                return null;

            int randomIndex = Random.Range(0, activePlayers.Count);
            return activePlayers[randomIndex];
        }

        private void Update()
        {
            if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
            {
                ChangePlayer(0);
            }
            else if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
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
            if (CurrentPlayer != null)
                CurrentPlayer.PlayerNotControl();
            
            CurrentPlayer = playerList[index];
            CurrentPlayer.PlayerControl();
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(playerList[index].CameraTransform, 0.2f));
        }
    }
}

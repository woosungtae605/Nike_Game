using System;
using System.Collections.Generic;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Agents.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private List<Player> playerList;
        public Player CurrentPlayer { get; private set; }
        
        private void Awake()
        {
            Debug.Assert(playerList != null && playerList.Count > 0, "Player list is empty");
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

        public void ChangePlayer(int index)
        {
            if (index < 0 || index >= playerList.Count)
            {
                Debug.LogError($"Player index {index} is out of range");
                return;   
            }
            
            CurrentPlayer = playerList[index];
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(playerList[index].CameraTransform));
        }
    }
}
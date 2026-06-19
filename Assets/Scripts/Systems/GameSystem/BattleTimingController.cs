using System;
using System.Collections;
using Agents.Players;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Camera;
using UI.BattleUI.StartUIs;
using UnityEngine;

namespace Systems.GameSystem
{
    public class BattleTimingController : MonoBehaviour
    {
        [SerializeField] private StartUICanvas startUICanvas;
        [SerializeField] private PlayerManager playerManager;
        
        [Header("Settings")]
        [SerializeField] private float startDelay = 0.5f;

        [SerializeField] private float cameraDelay = 0.5f;

        private void Awake()
        {
            startUICanvas.OnEnd += HandleEnd;
        }

        private void OnDestroy()
        {
            startUICanvas.OnEnd -= HandleEnd;
        }

        private void HandleEnd()
        {
            Bus<BattleStartEvent>.Raise(new BattleStartEvent());
        }

        private void Start()
        {
            BattleStart();
        }

        private void BattleStart()
        {
            StartCoroutine(BattleDelay());
        }

        private IEnumerator BattleDelay()
        {
            yield return new WaitForSeconds(startDelay);
            startUICanvas.ActionStart();
            yield return new WaitForSeconds(cameraDelay);
            
            foreach (Player player in playerManager.Players)
            {
                if (player != null)
                {
                    Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(player.CameraTransform, 0.7f));
                    break;
                }
            }
        }
    }
}
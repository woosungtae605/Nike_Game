using System;
using System.Collections;
using CoreSystem.BusSystem;
using GameEvents;
using UnityEngine;

namespace Systems.GameSystem
{
    public class BattleStarter : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(BattleDelay());
        }

        public void BattleStart()
        {
            Bus<BattleStartEvent>.Raise(new BattleStartEvent());
        }

        private IEnumerator BattleDelay()
        {
            yield return new WaitForSeconds(2);
            BattleStart();
        }
    }
}
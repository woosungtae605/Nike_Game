using System;
using Systems.Coin;
using TMPro;
using UnityEngine;

namespace UI.MainSceneUI.Nikkes
{
    public class CoinText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private CoinManager coinManager;

        private void OnEnable()
        {
            if (coinManager == null)
                return;

            coinManager.OnChangeCoin += HandleChangeCoin;
            HandleChangeCoin(coinManager.CurrentCoin);
        }

        private void OnDisable()
        {
            if (coinManager != null)
                coinManager.OnChangeCoin -= HandleChangeCoin;
        }

        private void HandleChangeCoin(long coin)
        {
            if (goldText == null)
                return;

            goldText.text = CoinFormatter.Format(coin);
        }
    }
}
using System;
using CoreSystem.BusSystem;
using GameEvents.Coin;
using Systems.SaveSystem;
using UnityEngine;

namespace Systems.Coin
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private SaveFileNameSO saveFileNameSO;

        private CoinData _coinData;

        public long CurrentCoin => _coinData != null ? _coinData.coin : 0;
        public event Action<long> OnChangeCoin;

        private void Awake()
        {
            LoadCoin();
            Bus<CoinEvent>.OnEvent += HandleCoinEvent;
        }

        private void OnDestroy()
        {
            Bus<CoinEvent>.OnEvent -= HandleCoinEvent;
        }

        private void LoadCoin()
        {
            if (!JsonSaveService.TryLoad(saveFileNameSO, out _coinData))
                _coinData = new CoinData { coin = 0 };

            NotifyCoinChanged();
        }

        private void HandleCoinEvent(CoinEvent coinEvent)
        {
            AddCoin(coinEvent.Amount);
        }

        public void AddCoin(long amount)
        {
            _coinData.coin += amount;

            if (_coinData.coin < 0)
                _coinData.coin = 0;

            SaveAndNotify();
        }

        public bool TryUseCoin(long amount)
        {
            if (_coinData.coin < amount)
            {
                Debug.Log("돈이 부족합니다");
                return false;
            }

            _coinData.coin -= amount;
            SaveAndNotify();
            return true;
        }

        private void SaveAndNotify()
        {
            JsonSaveService.Save(saveFileNameSO, _coinData);
            NotifyCoinChanged();
        }

        private void NotifyCoinChanged()
        {
            OnChangeCoin?.Invoke(_coinData.coin);
            Bus<CoinChangedEvent>.Raise(new CoinChangedEvent(_coinData.coin));
        }
    }
}

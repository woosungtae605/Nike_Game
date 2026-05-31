using System;
using CoreSystem;
using GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.Reload
{
    public class NikkeReloadUI : MonoBehaviour, IUIElement<float>
    {
        private Slider _slider;
        private bool _isStart = false;
        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>(true);
            Debug.Assert(_slider != null, "Slider not found in children");
            _slider.gameObject.SetActive(false);
            Bus<NikkeReloadUIActiveEvent>.OnEvent += HandleNikkeReloadUIActiveEvent;
        }
        
        private void OnDestroy()
        {
            Bus<NikkeReloadUIActiveEvent>.OnEvent -= HandleNikkeReloadUIActiveEvent;
        }

        private void HandleNikkeReloadUIActiveEvent(NikkeReloadUIActiveEvent e)
        {
            if(e.IsActive)
                Show(e.ReloadTime);
            else
                Hide();
        }
        public void Show(float item)
        {
            _slider.gameObject.SetActive(true);
            _slider.maxValue = item;
            _slider.value = 0;
            _isStart = true;
        }

        private void Update()
        {
            if (!_isStart) return;
            
            _slider.value += Time.deltaTime;
            
            if (_slider.value >= _slider.maxValue)
            {
                Hide();
            }
        }

        public void Hide()
        {
            _isStart = false;
            _slider.gameObject.SetActive(false);
        }
    }
}
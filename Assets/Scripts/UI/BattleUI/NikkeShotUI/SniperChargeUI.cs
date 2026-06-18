using CoreSystem.BusSystem;
using GameEvents.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.NikkeShotUI
{
    public class SniperChargeUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI percentText;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            SetVisible(false);
        }
        
        private void OnEnable()
        {
            Bus<SniperChargeUIEvent>.OnEvent += HandleCharge;
        }
        
        private void OnDisable()
        {
            Bus<SniperChargeUIEvent>.OnEvent -= HandleCharge;
        }

        private void HandleCharge(SniperChargeUIEvent obj)
        {
            SetVisible(obj.Active);

            if (!obj.Active)
                return;

            slider.value = obj.Percent;
            percentText.text = $"{Mathf.RoundToInt(obj.DisplayPercent)}%";
        }

        private void SetVisible(bool value)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.alpha = value ? 1f : 0f;
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }
    }
}

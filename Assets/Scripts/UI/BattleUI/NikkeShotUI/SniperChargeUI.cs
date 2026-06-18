using CoreSystem.BusSystem;
using GameEvents.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.NikkeShotUI
{
    public class SniperChargeUI : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI percentText;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (root == null)
                root = gameObject;

            _canvasGroup = root.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
                _canvasGroup = root.AddComponent<CanvasGroup>();

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

using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Settings
{
    public class SoundSettingUI : MonoBehaviour
    {
        [Header("Sound Settings")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Toggle masterToggle;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Toggle bgmToggle;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle sfxToggle;
        
        [SerializeField] private Button exitBtn;
        [SerializeField] private GameObject exitObject;

        private bool _isRefreshing;

        private void Awake()
        {
            AddListeners();
        }

        private void OnEnable()
        {
            RefreshUI();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            if (exitBtn != null)
                exitBtn.onClick.AddListener(Hide);

            if (masterSlider != null)
                masterSlider.onValueChanged.AddListener(HandleMasterVolumeChanged);
            if (bgmSlider != null)
                bgmSlider.onValueChanged.AddListener(HandleBGMVolumeChanged);
            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(HandleSFXVolumeChanged);

            if (masterToggle != null)
                masterToggle.onValueChanged.AddListener(HandleMasterToggleChanged);
            if (bgmToggle != null)
                bgmToggle.onValueChanged.AddListener(HandleBGMToggleChanged);
            if (sfxToggle != null)
                sfxToggle.onValueChanged.AddListener(HandleSFXToggleChanged);
        }

        private void RemoveListeners()
        {
            if (exitBtn != null)
                exitBtn.onClick.RemoveListener(Hide);

            if (masterSlider != null)
                masterSlider.onValueChanged.RemoveListener(HandleMasterVolumeChanged);
            if (bgmSlider != null)
                bgmSlider.onValueChanged.RemoveListener(HandleBGMVolumeChanged);
            if (sfxSlider != null)
                sfxSlider.onValueChanged.RemoveListener(HandleSFXVolumeChanged);

            if (masterToggle != null)
                masterToggle.onValueChanged.RemoveListener(HandleMasterToggleChanged);
            if (bgmToggle != null)
                bgmToggle.onValueChanged.RemoveListener(HandleBGMToggleChanged);
            if (sfxToggle != null)
                sfxToggle.onValueChanged.RemoveListener(HandleSFXToggleChanged);
        }

        private void RefreshUI()
        {
            if (SoundManager.Instance == null)
                return;

            _isRefreshing = true;

            SetSliderValue(masterSlider, SoundManager.Instance.GetMasterVolume());
            SetSliderValue(bgmSlider, SoundManager.Instance.GetBGMVolume());
            SetSliderValue(sfxSlider, SoundManager.Instance.GetSFXVolume());

            SetToggleValue(masterToggle, SoundManager.Instance.IsMasterMuted());
            SetToggleValue(bgmToggle, SoundManager.Instance.IsBGMMuted());
            SetToggleValue(sfxToggle, SoundManager.Instance.IsSFXMuted());

            SetSliderInteractable(masterSlider, masterToggle == null || !masterToggle.isOn);
            SetSliderInteractable(bgmSlider, bgmToggle == null || !bgmToggle.isOn);
            SetSliderInteractable(sfxSlider, sfxToggle == null || !sfxToggle.isOn);

            _isRefreshing = false;
        }

        private void SetSliderValue(Slider slider, float value)
        {
            if (slider == null)
                return;

            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.SetValueWithoutNotify(value);
        }

        private void SetToggleValue(Toggle toggle, bool value)
        {
            if (toggle == null)
                return;

            toggle.SetIsOnWithoutNotify(value);
        }

        private void SetSliderInteractable(Slider slider, bool value)
        {
            if (slider != null)
                slider.interactable = value;
        }

        private void HandleMasterVolumeChanged(float value)
        {
            if (_isRefreshing || SoundManager.Instance == null)
                return;

            SoundManager.Instance.SetMaster(value);
        }

        private void HandleBGMVolumeChanged(float value)
        {
            if (_isRefreshing || SoundManager.Instance == null)
                return;

            SoundManager.Instance.SetBGM(value);
        }

        private void HandleSFXVolumeChanged(float value)
        {
            if (_isRefreshing || SoundManager.Instance == null)
                return;

            SoundManager.Instance.SetSFX(value);
        }

        private void HandleMasterToggleChanged(bool isOn)
        {
            if (_isRefreshing || SoundManager.Instance == null)
                return;

            SoundManager.Instance.SetMasterMuted(isOn);
            SetSliderInteractable(masterSlider, !isOn);
        }

        private void HandleBGMToggleChanged(bool isOn)
        {
            if (_isRefreshing || SoundManager.Instance == null)
                return;

            SoundManager.Instance.SetBGMMuted(isOn);
            SetSliderInteractable(bgmSlider, !isOn);
        }

        private void HandleSFXToggleChanged(bool isOn)
        {
            if (_isRefreshing || SoundManager.Instance == null)
                return;

            SoundManager.Instance.SetSFXMuted(isOn);
            SetSliderInteractable(sfxSlider, !isOn);
        }

        private void Hide()
        {
            exitObject.SetActive(false);
        }
    }
}



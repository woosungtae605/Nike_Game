using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Settings
{
    public class SoundSettingUI : MonoBehaviour
    {
        [Header("Sound Settings")]
        [SerializeField] private Button exitBtn;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Toggle masterToggle;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Toggle bgmToggle;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle sfxToggle;
    }
}
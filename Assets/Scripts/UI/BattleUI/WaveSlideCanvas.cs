using System.Collections;
using Systems.GameSystem.Wave;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI
{
    public class WaveSlideCanvas : MonoBehaviour, IUIElement<int>
    {
        [SerializeField] private Slider slider;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private float moveDuration = 0.25f;

        private Coroutine _moveRoutine;

        private void Awake()
        {
            if (slider == null)
                slider = GetComponentInChildren<Slider>();

            InitializeSlider();
        }

        private void OnEnable()
        {
            if (waveManager != null)
                waveManager.OnWaveStarted += HandleWaveStarted;
        }

        private void OnDisable()
        {
            if (waveManager != null)
                waveManager.OnWaveStarted -= HandleWaveStarted;
        }

        private void InitializeSlider()
        {
            if (slider == null || waveManager == null)
                return;

            slider.minValue = 0f;
            slider.maxValue = waveManager.WaveCount;
            slider.value = 0f;
        }

        private void HandleWaveStarted(int currentWave, int totalWave)
        {
            if (slider == null)
                return;

            slider.maxValue = totalWave;
            Show(currentWave);
        }
        public void Show(int currentWave)
        {
            MoveSlider(currentWave);
        }

        public void Hide()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                _moveRoutine = null;
            }

            if (slider != null)
                slider.value = 0f;
            
            gameObject.SetActive(false);
        }

        private void MoveSlider(float targetValue)
        {
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);

            _moveRoutine = StartCoroutine(MoveSliderRoutine(targetValue));
        }

        private IEnumerator MoveSliderRoutine(float targetValue)
        {
            float startValue = slider.value;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / moveDuration);
                slider.value = Mathf.Lerp(startValue, targetValue, t);
                yield return null;
            }

            slider.value = targetValue;
            _moveRoutine = null;
        }
    }
}
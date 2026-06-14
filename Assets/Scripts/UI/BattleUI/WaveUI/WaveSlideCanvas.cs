using System.Collections;
using Systems.GameSystem.Wave;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.WaveUI
{
    public class WaveSlideCanvas : MonoBehaviour, IUIElement<int>
    {
        [SerializeField] private Slider slider;
        [SerializeField] private float moveDuration = 0.25f;

        private Coroutine _moveRoutine;

        public void InitializeSlider()
        {
            if (slider == null)
                slider = GetComponentInChildren<Slider>();

            slider.minValue = 0f;
            slider.value = 0f;
        }

        public void HandleWaveStarted(int currentWave, int totalWave)
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
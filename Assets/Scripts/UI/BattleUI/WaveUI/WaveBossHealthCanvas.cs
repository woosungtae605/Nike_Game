using Agents.Enemies;
using Agents.Module;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.WaveUI
{
    public class WaveBossHealthCanvas : MonoBehaviour
    {
        [Header("Uis")]
        [SerializeField] private Slider enemyHealthSlider;
        [SerializeField] private Slider delayedHealthSlider;
        [SerializeField] private TextMeshProUGUI healthCountText;

        [Header("Settings")]
        [SerializeField] private int healthCount = 10;
        [SerializeField] private float delayedHealthWait = 0.18f;
        [SerializeField] private float delayedHealthMoveDuration = 0.35f;
        
        private AbstractEnemy _enemy;
        private HealthModule _health;
        private MotionHandle _delayedHealthHandle;
        private Coroutine _delayedHealthCoroutine;

        private void OnDisable()
        {
            StopDelayedHealthMotion();
            UnbindHealth();
        }

        public void SetEnemy(AbstractEnemy enemy)
        {
            StopDelayedHealthMotion();
            UnbindHealth();

            _enemy = enemy;
            _health = _enemy != null ? _enemy.HealthModule : null;

            if (_health == null)
                return;

            _health.OnChanged += HandleHealthChanged;
            UpdateHealth(_health.CurrentValue, _health.MaxValue, true);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (_health != null)
                UpdateHealth(_health.CurrentValue, _health.MaxValue, true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void HandleHealthChanged(int currentHealth, int maxHealth)
        {
            UpdateHealth(currentHealth, maxHealth, false);
        }

        private void UpdateHealth(int currentHealth, int maxHealth, bool immediate)
        {
            int segmentCount = Mathf.Max(1, healthCount);
            float segmentHealth = maxHealth / (float)segmentCount;

            int currentCount = currentHealth <= 0
                ? 0
                : Mathf.CeilToInt(currentHealth / segmentHealth);
            currentCount = Mathf.Clamp(currentCount, 0, segmentCount);

            float sliderValue = 0f;
            if (currentCount > 0)
            {
                float segmentStartHealth = (currentCount - 1) * segmentHealth;
                sliderValue = (currentHealth - segmentStartHealth) / segmentHealth;
            }

            sliderValue = Mathf.Clamp01(sliderValue);
            SetSliderValue(enemyHealthSlider, sliderValue);

            if (immediate)
                SetSliderValue(delayedHealthSlider, sliderValue);
            else
                MoveDelayedHealth(sliderValue);

            if (healthCountText != null)
                healthCountText.text = "x" + currentCount.ToString();
        }

        private void SetSliderValue(Slider slider, float value)
        {
            if (slider == null)
                return;

            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = value;
        }

        private void MoveDelayedHealth(float targetValue)
        {
            if (delayedHealthSlider == null)
                return;

            StopDelayedHealthMotion();
            _delayedHealthCoroutine = StartCoroutine(MoveDelayedHealthRoutine(targetValue));
        }

        private System.Collections.IEnumerator MoveDelayedHealthRoutine(float targetValue)
        {
            if (delayedHealthWait > 0f)
                yield return new WaitForSeconds(delayedHealthWait);

            _delayedHealthHandle = LMotion.Create(delayedHealthSlider.value, targetValue, delayedHealthMoveDuration)
                .WithEase(Ease.OutCubic)
                .Bind(value => delayedHealthSlider.value = value)
                .AddTo(this);

            yield return _delayedHealthHandle.ToYieldInstruction();
            _delayedHealthCoroutine = null;
        }

        private void StopDelayedHealthMotion()
        {
            if (_delayedHealthCoroutine != null)
            {
                StopCoroutine(_delayedHealthCoroutine);
                _delayedHealthCoroutine = null;
            }

            if (_delayedHealthHandle.IsActive())
                _delayedHealthHandle.Cancel();
        }

        private void UnbindHealth()
        {
            if (_health != null)
                _health.OnChanged -= HandleHealthChanged;

            _health = null;
            _enemy = null;
        }
    }
}


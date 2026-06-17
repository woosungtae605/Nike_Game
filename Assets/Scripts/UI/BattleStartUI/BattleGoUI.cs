using CoreSystem;
using CoreSystem.BusSystem;
using GameEvents.UI;
using LitMotion;
using LitMotion.Extensions;
using Systems.GameSystem.Wave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.BattleStartUI
{
    public class BattleGoUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI wave;
        [SerializeField] private Button playButton;
        [SerializeField] private float moveDistance = 60f;
        [SerializeField] private float moveDuration = 0.35f;

        private WaveInformationSO _informationSo;
        private RectTransform _waveRect;
        private Vector2 _originPosition;
        private MotionHandle _moveHandle;

        private void Awake()
        {
            _waveRect = transform as RectTransform;
            _originPosition = _waveRect.anchoredPosition;
            playButton.onClick.AddListener(GotoWave);
            Bus<BattleGoUIEvent>.OnEvent += HandleBattleGoUI;
        }

        private void GotoWave()
        {
            FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene(_informationSo.WaveName));
        }

        private void OnDestroy()
        {
            Bus<BattleGoUIEvent>.OnEvent -= HandleBattleGoUI;
            playButton.onClick.RemoveListener(GotoWave);

            if (_moveHandle.IsActive())
                _moveHandle.Cancel();
        }

        private void HandleBattleGoUI(BattleGoUIEvent obj)
        {
            _informationSo = obj.waveInformationSo;
            wave.text = "Wave - " + obj.waveInformationSo.WaveNumber.ToString();
            PlayUpMotion();
        }

        private void PlayUpMotion()
        {
            if (_moveHandle.IsActive())
                _moveHandle.Cancel();

            _waveRect.anchoredPosition = _originPosition;

            Vector2 targetPosition = _originPosition + Vector2.up * moveDistance;
            _moveHandle = LMotion.Create(_originPosition, targetPosition, moveDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_waveRect);
        }
    }
}

using System;
using System.Collections;
using Agents.Enemies;
using Systems;
using Systems.GameSystem.Wave;
using UI.BattleUI.WaveUI.WarningUI;
using UnityEngine;

namespace UI.BattleUI.WaveUI
{
    public class WaveUIPresenter : MonoBehaviour, IBattleStart
    {
        [Header("UIs")]
        [SerializeField] private WarningCanvas warningCanvas;
        [SerializeField] private WaveSlideCanvas waveSlideCanvas;

        [Header("References")]
        [SerializeField] private WaveManager waveManager;
        private void Awake()
        {
            Init();
            BattleStart();
        }

        private void OnDestroy()
        {
            waveManager.OnBossSpawn -= HandleBossSpawn;
            waveManager.OnWaveStarted -= HandleWaveStart;
        }

        private void Init()
        {
            warningCanvas.Init();
            waveManager.OnBossSpawn += HandleBossSpawn;
            
            waveSlideCanvas.InitializeSlider();
            waveManager.OnWaveStarted += HandleWaveStart;
            waveSlideCanvas.gameObject.SetActive(false);
        }

        private void HandleBossSpawn(AbstractEnemy obj)
        {
            StartCoroutine(SpawnMotionCoroutine(obj));
        }

        private IEnumerator SpawnMotionCoroutine(AbstractEnemy obj)
        {
            warningCanvas.Show();
            yield return null;
        }

        private void HandleWaveStart(int arg1, int arg2)
        {
            waveSlideCanvas.HandleWaveStarted(arg1, arg2);
        }

        public void Hide()
        {
            
        }

        public void BattleStart()
        {
            waveSlideCanvas.gameObject.SetActive(true);
        }
    }
}
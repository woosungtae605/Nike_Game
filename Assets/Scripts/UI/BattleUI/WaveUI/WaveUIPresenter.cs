using System;
using System.Collections;
using Agents.Enemies;
using CoreSystem.BusSystem;
using GameEvents;
using Systems;
using Systems.GameSystem.Wave;
using UI.BattleUI.WaveUI.WarningUI;
using UnityEngine;

namespace UI.BattleUI.WaveUI
{
    public class WaveUIPresenter : MonoBehaviour
    {
        [Header("UIs")]
        [SerializeField] private WarningCanvas warningCanvas;
        [SerializeField] private WaveSlideCanvas waveSlideCanvas;
        [SerializeField] private WaveBossHealthCanvas waveBossCanvas;

        [Header("References")]
        [SerializeField] private WaveManager waveManager;

        private Coroutine _coroutine;
        private void Awake()
        {
            Init();
            Bus<BattleStartEvent>.OnEvent += HandleBattleStart;
        }

        private void HandleBattleStart(BattleStartEvent obj)
        {
            waveSlideCanvas.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            waveManager.OnBossSpawn -= HandleBossSpawn;
            waveManager.OnWaveStarted -= HandleWaveStart;
            Bus<BattleStartEvent>.OnEvent -= HandleBattleStart;
        }

        private void Init()
        {
            warningCanvas.Init();
            waveManager.OnBossSpawn += HandleBossSpawn;
            
            waveSlideCanvas.InitializeSlider();
            waveManager.OnWaveStarted += HandleWaveStart;
            waveSlideCanvas.gameObject.SetActive(false);

            waveBossCanvas.gameObject.SetActive(false);
        }

        private void HandleBossSpawn(AbstractEnemy obj)
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            
            _coroutine = StartCoroutine(SpawnMotionCoroutine(obj));
        }

        private IEnumerator SpawnMotionCoroutine(AbstractEnemy obj)
        {
            waveSlideCanvas.gameObject.SetActive(false);
            yield return warningCanvas.ShowRoutine();

            waveBossCanvas.SetEnemy(obj);
            waveBossCanvas.Show();
            
        }

        private void HandleWaveStart(int arg1, int arg2)
        {
            waveSlideCanvas.HandleWaveStarted(arg1, arg2);
        }

        public void Hide()
        { 
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            
            _coroutine = null;
            
            warningCanvas.gameObject.SetActive(false); 
            waveSlideCanvas.gameObject.SetActive(false); 
            waveBossCanvas.gameObject.SetActive(false);
        }
    }
}


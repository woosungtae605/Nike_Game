using CoreSystem;
using Systems.GameSystem.Wave;
using Systems.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.TItle
{
    public class StartBtn : MonoBehaviour
    {
        [SerializeField] private string mainSceneName = "MainScene";
        [SerializeField] private SaveFileNameSO waveClearSaveFile;
        [SerializeField] private CurrentWaveInformationSO currentWaveInformation;
        [SerializeField] private WaveInformationSO practiceWaveInformation;

        public void ClickStart()
        {
            string sceneName = GetStartSceneName();
            FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene(sceneName));
        }

        private string GetStartSceneName()
        {
            if (WaveClearSave.HasAnyCleared(waveClearSaveFile) || practiceWaveInformation == null)
            {
                currentWaveInformation?.Clear();
                return mainSceneName;
            }

            currentWaveInformation?.SetCurrentWaveInformation(practiceWaveInformation);
            return practiceWaveInformation.WaveName;
        }
    }
}
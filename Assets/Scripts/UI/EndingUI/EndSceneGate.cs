using Systems.GameSystem.Wave;
using Systems.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.EndingUI
{
    public class EndSceneGate : MonoBehaviour
    {
        [SerializeField] private SaveFileNameSO waveClearSaveFile;
        [SerializeField] private SaveFileNameSO endingSaveFile;
        [SerializeField] private string endingSceneName = "EndScene";
        [SerializeField] private int finalWaveNumber = 5;
        [SerializeField] private int firstWaveNumber = 1;

        private void Start()
        {
            if (ShouldEnterEnding())
                SceneManager.LoadScene(endingSceneName);
        }

        private bool ShouldEnterEnding()
        {
            if (string.IsNullOrWhiteSpace(endingSceneName))
                return false;

            if (EndingClearSave.IsEndingViewed(endingSaveFile))
                return false;

            return WaveClearSave.HasClearedThrough(waveClearSaveFile, finalWaveNumber, firstWaveNumber);
        }
    }
}

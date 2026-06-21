using Agents.Enemies;
using Systems.GameSystem.Wave;
using UnityEngine;

namespace Sound
{
    public class BossSpawnSoundPlayer : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private AudioClip bossSpawnClip;
        [SerializeField] private float clipStartTime = 11f;
        [SerializeField] private float volumeScale = 5f;

        private void Awake()
        {
            if (waveManager == null)
                waveManager = FindFirstObjectByType<WaveManager>();
        }

        private void OnEnable()
        {
            if (waveManager != null)
                waveManager.OnBossSpawn += HandleBossSpawn;
        }

        private void OnDisable()
        {
            if (waveManager != null)
                waveManager.OnBossSpawn -= HandleBossSpawn;
        }

        private void HandleBossSpawn(AbstractEnemy boss)
        {
            SoundManager.Instance?.PlaySFX(bossSpawnClip, clipStartTime, volumeScale);
        }
    }
}

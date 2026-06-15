using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        private const string MASTER = "MasterVolume";
        private const string BGM = "BGMVolume";
        private const string SFX = "SFXVolume";

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            DontDestroyOnLoad(gameObject);

            LoadVolume(MASTER);
            LoadVolume(BGM);
            LoadVolume(SFX);
        }

        private void LoadVolume(string key)
        {
            float v = PlayerPrefs.GetFloat(key, 1f);
            ApplyVolume(key, v);
        }

        // 슬라이더 값(0~1)을 데시벨로 변환해 믹서에 적용 + 저장
        public void SetMaster(float value) => SetVolume(MASTER, value);
        public void SetBGM(float value) => SetVolume(BGM, value);
        public void SetSFX(float value) => SetVolume(SFX, value);

        private void SetVolume(string key, float value)
        {
            ApplyVolume(key, value);
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        private void ApplyVolume(string key, float value)
        {
            if (mixer == null) { Debug.LogWarning("Mixer 미할당"); return; }
            float dB = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
            if (!mixer.SetFloat(key, dB))
                Debug.LogWarning($"믹서 파라미터 '{key}' 없음 ? Expose 이름 확인");
        }

        // 효과음 재생용
        public void PlaySFX(AudioClip clip)
        {
            if (clip != null) sfxSource.PlayOneShot(clip);
        }

        // 배경음 재생용
        public void PlayBGM(AudioClip clip)
        {
            if (bgmSource.clip == clip) return;
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public float GetSavedVolume(string key) => PlayerPrefs.GetFloat(key, 1f);
    }
}
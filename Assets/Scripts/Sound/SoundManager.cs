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

        private const string MASTER_MUTE = "MasterMute";
        private const string BGM_MUTE = "BGMMute";
        private const string SFX_MUTE = "SFXMute";
    
        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ApplySavedVolume(MASTER, MASTER_MUTE);
            ApplySavedVolume(BGM, BGM_MUTE);
            ApplySavedVolume(SFX, SFX_MUTE);
        }

        public void SetMaster(float value) => SetVolume(MASTER, MASTER_MUTE, value);
        public void SetBGM(float value) => SetVolume(BGM, BGM_MUTE, value);
        public void SetSFX(float value) => SetVolume(SFX, SFX_MUTE, value);

        public void SetMasterMuted(bool muted) => SetMuted(MASTER, MASTER_MUTE, muted);
        public void SetBGMMuted(bool muted) => SetMuted(BGM, BGM_MUTE, muted);
        public void SetSFXMuted(bool muted) => SetMuted(SFX, SFX_MUTE, muted);

        public float GetMasterVolume() => GetSavedVolume(MASTER);
        public float GetBGMVolume() => GetSavedVolume(BGM);
        public float GetSFXVolume() => GetSavedVolume(SFX);

        public bool IsMasterMuted() => GetSavedMuted(MASTER_MUTE);
        public bool IsBGMMuted() => GetSavedMuted(BGM_MUTE);
        public bool IsSFXMuted() => GetSavedMuted(SFX_MUTE);

        private void SetVolume(string volumeKey, string muteKey, float value)
        {
            value = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(volumeKey, value);
            PlayerPrefs.Save();

            ApplyVolume(volumeKey, IsMuted(muteKey) ? 0f : value);
        }

        private void SetMuted(string volumeKey, string muteKey, bool muted)
        {
            PlayerPrefs.SetInt(muteKey, muted ? 1 : 0);
            PlayerPrefs.Save();

            ApplySavedVolume(volumeKey, muteKey);
        }

        private void ApplySavedVolume(string volumeKey, string muteKey)
        {
            float volume = GetSavedVolume(volumeKey);
            ApplyVolume(volumeKey, IsMuted(muteKey) ? 0f : volume);
        }

        private void ApplyVolume(string key, float value)
        {
            if (mixer == null)
            {
                Debug.LogWarning("Mixer 미할당");
                return;
            }

            float dB = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
            if (!mixer.SetFloat(key, dB))
                Debug.LogWarning($"믹서 파라미터 '{key}' 없음 ? Expose 이름 확인");
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip != null && sfxSource != null)
                sfxSource.PlayOneShot(clip);
        }

        public void PlaySFX(AudioClip clip, float startTime)
        {
            PlaySFX(clip, startTime, 1f);
        }

        public void PlaySFX(AudioClip clip, float startTime, float volumeScale)
        {
            if (clip == null || sfxSource == null)
                return;

            startTime = Mathf.Clamp(startTime, 0f, clip.length);
            volumeScale = Mathf.Max(0f, volumeScale);

            while (volumeScale > 0f)
            {
                float layerVolume = Mathf.Min(1f, volumeScale);
                PlaySFXLayer(clip, startTime, layerVolume);
                volumeScale -= layerVolume;
            }
        }

        private void PlaySFXLayer(AudioClip clip, float startTime, float volumeScale)
        {
            if (startTime <= 0f)
            {
                sfxSource.PlayOneShot(clip, volumeScale);
                return;
            }

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
            source.volume = sfxSource.volume * volumeScale;
            source.pitch = sfxSource.pitch;
            source.spatialBlend = sfxSource.spatialBlend;
            source.clip = clip;
            source.time = startTime;
            source.Play();

            Destroy(source, clip.length - startTime + 0.1f);
        }

        public void PlayBGM(AudioClip clip)
        {
            if (bgmSource == null || bgmSource.clip == clip)
                return;

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void StopBGM()
        {
            if (bgmSource == null)
                return;

            bgmSource.Stop();
            bgmSource.clip = null;
        }

        public float GetSavedVolume(string key) => PlayerPrefs.GetFloat(key, 1f);

        private bool GetSavedMuted(string key) => IsMuted(key);

        private bool IsMuted(string key) => PlayerPrefs.GetInt(key, 0) == 1;
    }
}




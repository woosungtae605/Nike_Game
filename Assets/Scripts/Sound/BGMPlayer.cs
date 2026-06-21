using UnityEngine;

namespace Sound
{
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip bgmClip;

        private void Start()
        {
            if (bgmClip == null)
                return;

            SoundManager.Instance?.PlayBGM(bgmClip);
        }
    }
}

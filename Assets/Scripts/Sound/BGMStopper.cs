using UnityEngine;

namespace Sound
{
    public class BGMStopper : MonoBehaviour
    {
        private void Start()
        {
            SoundManager.Instance?.StopBGM();
        }
    }
}

using CoreSystem;
using GameEvents;
using UnityEngine;

namespace Camera
{
    public class CameraRigRoot : MonoBehaviour
    {
        [Header("Camera Controllers")]
        [SerializeField] private CameraPivot cameraPivot;
        [SerializeField] private CameraRecoil cameraRecoil;
        
        public void Awake()
        {
            Bus<CameraRecoilEvent>.OnEvent += cameraRecoil.HandleCameraRecoil;
        }

        private void OnDestroy()
        {
            Bus<CameraRecoilEvent>.OnEvent -= cameraRecoil.HandleCameraRecoil;
        }
    }
}
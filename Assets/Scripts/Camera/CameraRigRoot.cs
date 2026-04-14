using CoreSystem;
using GameEvents;
using Systems;
using UnityEngine;

namespace Camera
{
    public class CameraRigRoot : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private CameraPivot cameraPivot;
        [SerializeField] private CameraRecoil cameraRecoil;
        [SerializeField] private CameraMove cameraMove;
        
        public void Awake()
        {
            Bus<CameraRecoilEvent>.OnEvent += cameraRecoil.HandleCameraRecoil;
            playerInput.OnMouseDeltaPos += cameraPivot.HandleMouseDeltaPos;
            playerInput.OnLeftMousePressedStart += cameraRecoil.HandleCameraRecoil;
        }

        private void OnDestroy()
        {
            Bus<CameraRecoilEvent>.OnEvent -= cameraRecoil.HandleCameraRecoil;
            playerInput.OnMouseDeltaPos -= cameraPivot.HandleMouseDeltaPos;
            playerInput.OnLeftMousePressedStart -= cameraRecoil.HandleCameraRecoil;
        }
    }
}
using CoreSystem;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Camera;
using Reflex.Attributes;
using Systems;
using UnityEngine;

namespace Camera
{
    public class CameraRigRoot : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CameraPivot cameraPivot;
        [SerializeField] private CameraRecoil cameraRecoil;
        [SerializeField] private CameraMove cameraMove;

        [Inject] private PlayerInputSO playerInputSo;

        public void Awake()
        {
            Bus<CameraRecoilEvent>.OnEvent += cameraRecoil.HandleCameraRecoil;
            playerInputSo.OnMouseDeltaPos += cameraPivot.HandleMouseDeltaPos;
        }

        private void OnDestroy()
        {
            Bus<CameraRecoilEvent>.OnEvent -= cameraRecoil.HandleCameraRecoil;
            playerInputSo.OnMouseDeltaPos -= cameraPivot.HandleMouseDeltaPos;
        }
    }
}
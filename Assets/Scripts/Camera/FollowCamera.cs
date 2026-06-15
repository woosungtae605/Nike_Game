using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class FollowCamera : MonoBehaviour
    {
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
            Debug.Assert(_camera != null, "CinemachineCamera is null", this);
        }

        public void TargetLook(GameObject target, float lens)
        {
            if (_camera == null || target == null)
                return;

            _camera.Target = new CameraTarget
            {
                TrackingTarget = target.transform,
                LookAtTarget = target.transform,
                CustomLookAtTarget = true
            };

            LensSettings lensSettings = _camera.Lens;
            lensSettings.FieldOfView = lens;
            _camera.Lens = lensSettings;
        }
    }
}

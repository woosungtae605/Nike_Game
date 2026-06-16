using CoreSystem.BusSystem;
using GameEvents.Camera;
using LitMotion;
using UnityEngine;

namespace Cameras
{
    public class CameraMove : MonoBehaviour
    {
        private MotionHandle _moveHandle;

        private void Awake()
        {
            Bus<CameraChangeEvent>.OnEvent += HandleCameraChange;
        }

        private void OnDestroy()
        {
            Bus<CameraChangeEvent>.OnEvent -= HandleCameraChange;
        }

        private void HandleCameraChange(CameraChangeEvent obj)
        {
            MoveToNekke(obj.TargetTransform, obj.Duration);
        }

        public void MoveToNekke(Transform target, float duration)
        {
            _moveHandle.TryCancel();
            _moveHandle = LMotion.Create(transform.position, target.position,duration).WithEase(Ease.Linear).Bind(x => transform.position = x);
        }
    }
}
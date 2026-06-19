using CoreSystem.BusSystem;
using GameEvents.Camera;
using LitMotion;
using UnityEngine;

namespace Cameras
{
    public class CameraMove : MonoBehaviour
    {
        private MotionHandle _moveHandle;
        private int _currentPriority;
        private float _priorityLockEndTime;

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
            if (Time.time < _priorityLockEndTime && obj.Priority < _currentPriority)
                return;

            MoveToNekke(obj.TargetTransform, obj.Duration);

            if (obj.Priority > 0)
            {
                _currentPriority = obj.Priority;
                _priorityLockEndTime = Time.time + Mathf.Max(obj.Duration, obj.LockDuration);
            }
            else if (Time.time >= _priorityLockEndTime)
            {
                _currentPriority = 0;
            }
        }

        public void MoveToNekke(Transform target, float duration)
        {
            if (target == null)
            {
                Debug.LogWarning("[CameraMove] CameraChangeEvent target is null.", this);
                return;
            }

            _moveHandle.TryCancel();

            if (duration <= 0f)
            {
                transform.position = target.position;
                return;
            }

            _moveHandle = LMotion.Create(transform.position, target.position, duration)
                .WithEase(Ease.Linear)
                .Bind(position => transform.position = position);
        }
    }
}

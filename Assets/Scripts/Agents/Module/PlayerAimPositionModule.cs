using LitMotion;
using LitMotion.Extensions;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class PlayerAimPositionModule : MonoBehaviour, IModule
    {
        [SerializeField] private Transform moveTarget;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float aimOffsetX = 0.5f;
        [SerializeField] private float moveDuration = 0.15f;

        private Vector3 _originLocalPosition;
        private MotionHandle _moveHandle;
        private Transform _cameraProxy;

        public void Initialize(ModuleOwner owner)
        {
            if (moveTarget == null)
                moveTarget = owner != null ? owner.transform : transform;

            if (cameraTarget == null && owner is Players.Player player)
                cameraTarget = player.CameraTransform;

            if (cameraTarget != null)
            {
                GameObject proxy = new GameObject($"{name}_AimCameraProxy");
                proxy.hideFlags = HideFlags.HideInHierarchy;
                _cameraProxy = proxy.transform;
                _cameraProxy.position = cameraTarget.position;
                _cameraProxy.rotation = cameraTarget.rotation;
            }

            _originLocalPosition = moveTarget.localPosition;
        }

        private void OnDisable()
        {
            _moveHandle.TryCancel();
        }

        private void OnDestroy()
        {
            if (_cameraProxy != null)
                Destroy(_cameraProxy.gameObject);
        }

        public void MoveToAimPosition()
        {
            MoveTo(_originLocalPosition + Vector3.right * aimOffsetX);
        }

        public void MoveToOriginPosition()
        {
            MoveTo(_originLocalPosition);
        }

        private void MoveTo(Vector3 targetPosition)
        {
            if (moveTarget == null)
                return;

            _moveHandle.TryCancel();

            if (moveDuration <= 0f)
            {
                moveTarget.localPosition = targetPosition;
                return;
            }

            _moveHandle = LMotion.Create(moveTarget.localPosition, targetPosition, moveDuration)
                .WithEase(Ease.OutCubic)
                .BindToLocalPosition(moveTarget);

            MoveCameraToTargetPosition(targetPosition);
        }

        private void MoveCameraToTargetPosition(Vector3 targetLocalPosition)
        {
            if (cameraTarget == null || _cameraProxy == null)
                return;

            Vector3 localDelta = targetLocalPosition - moveTarget.localPosition;
            Transform parent = moveTarget.parent;
            Vector3 worldDelta = parent != null ? parent.TransformVector(localDelta) : localDelta;

            _cameraProxy.position = cameraTarget.position + worldDelta;
            _cameraProxy.rotation = cameraTarget.rotation;
            Bus<CameraChangeEvent>.Raise(new CameraChangeEvent(_cameraProxy, moveDuration));
        }
    }
}

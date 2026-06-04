using System;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using LitMotion;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float zoomDuration = 0.15f;
        [SerializeField] private Ease zoomEase = Ease.OutCubic;

        private CinemachineCamera _cinemachineCamera;
        private float _originFov;
        private MotionHandle _zoomHandle;

        private void Awake()
        {
            _cinemachineCamera = GetComponent<CinemachineCamera>();
            Debug.Assert(_cinemachineCamera != null, "CinemachineCamera is null");

            _originFov = _cinemachineCamera.Lens.FieldOfView;

            Bus<CameraZoomEvent>.OnEvent += HandleCameraZoom;
        }

        private void OnDestroy()
        {
            _zoomHandle.TryCancel();
            Bus<CameraZoomEvent>.OnEvent -= HandleCameraZoom;
        }

        private void HandleCameraZoom(CameraZoomEvent obj)
        {
            Zoom(obj.ZoomAmount, obj.ZoomIn);
        }

        private void Zoom(float zoomAmount, bool zoomIn)
        {
            float targetFov = zoomIn
                ? _originFov - zoomAmount
                : _originFov + zoomAmount;

            _zoomHandle.TryCancel();

            _zoomHandle = LMotion.Create(
                    _cinemachineCamera.Lens.FieldOfView,
                    targetFov,
                    zoomDuration)
                .WithEase(zoomEase)
                .Bind(fov =>
                {
                    var lens = _cinemachineCamera.Lens;
                    lens.FieldOfView = fov;
                    _cinemachineCamera.Lens = lens;
                });
        }
    }
}
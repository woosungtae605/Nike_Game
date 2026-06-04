using System;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraZoom : MonoBehaviour
    {
        private CinemachineCamera _cinemachineCamera;

        private void Awake()
        {
            _cinemachineCamera = GetComponent<CinemachineCamera>();
            Bus<CameraZoomEvent>.OnEvent += HandleCameraZoom;
        }

        private void OnDestroy()
        {
            Bus<CameraZoomEvent>.OnEvent -= HandleCameraZoom;
        }

        private void HandleCameraZoom(CameraZoomEvent obj)
        {
            Zoom(obj.ZoomAmount, obj.ZoomIn);
        }

        private void Zoom(float zoomAmount, bool zoomIn)
        {
            if (zoomIn)
            {
                _cinemachineCamera.Lens.OrthographicSize -= zoomAmount;
            }
            else
            {
                _cinemachineCamera.Lens.OrthographicSize += zoomAmount;
            }
        }
    }
}
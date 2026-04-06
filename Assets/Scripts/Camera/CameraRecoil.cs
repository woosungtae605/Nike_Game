using System;
using CoreSystem;
using GameEvents;
using LitMotion;
using UnityEngine;

namespace Camera
{
    public class CameraRecoil : MonoBehaviour
    {
        private MotionHandle _rotationHandle;
        private MotionHandle _positionHandle;
        public void HandleCameraRecoil(CameraRecoilEvent evt)
        {
            LMotion.Create(0f, 10f, 0.2f);
        }
        [ContextMenu("ez")]
        public void HandleCameraRecoil()
        {
            Debug.Log("ez");
            LMotion.Create(0f, 10f, 0.2f);
        }
    }
}
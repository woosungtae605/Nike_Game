using System;
using CoreSystem;
using GameEvents;
using LitMotion;
using UnityEngine;

namespace Camera
{
    public class CameraRecoil : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float recoilAngle = 2.5f;
        [SerializeField] private float recoilAngleDuration;
        [SerializeField] private float recoilBack = 0.12f;
        [SerializeField] private float recoilBackDuration;

        private MotionHandle _rotationHandle;
        private MotionHandle _positionHandle;

        private Vector3 _baseLocalPos;
        private Vector3 _baseLocalEuler;

        private void Awake()
        {
            _baseLocalPos = transform.localPosition;
            _baseLocalEuler = transform.localEulerAngles;
        }

        public void HandleCameraRecoil(CameraRecoilEvent evt)
        {
            _rotationHandle.TryCancel();
            _positionHandle.TryCancel();
            if(evt.RotationRecoil)
            {
                _rotationHandle = LMotion.Create(0f, recoilAngle * evt.Power, evt.Duration * recoilAngleDuration)
                        .WithEase(Ease.OutQuad)
                        .WithLoops(2, LoopType.Yoyo)
                        .Bind(x => transform.localRotation = Quaternion.Euler(_baseLocalEuler.x - x, _baseLocalEuler.y, _baseLocalEuler.z));
            }
            if(evt.PositionRecoil)
            {
                _positionHandle = LMotion.Create(0f, recoilBack * evt.Power, evt.Duration * recoilBackDuration)
                        .WithEase(Ease.OutCubic)
                        .WithLoops(2, LoopType.Yoyo)
                        .Bind(z => transform.localPosition = new Vector3(_baseLocalPos.x, _baseLocalPos.y, _baseLocalPos.z - z));
            }
        }
        [ContextMenu("ez")]
        public void HandleCameraRecoil()
        {
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(0.5f,0.5f, true, true));
        }
    }
}
using System.Collections;
using Agents.Enemies;
using LitMotion;
using Systems.GameSystem.Wave;
using Unity.Cinemachine;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private CinemachineCamera normalCamera;
        [SerializeField] private CinemachineCamera bossCamera;
        [SerializeField] private UnityCamera outputCamera;

        [Header("Boss Focus")]
        [SerializeField] private float bossFocusMoveDuration = 0.35f;
        [SerializeField] private float bossFocusHoldDuration = 2f;
        [SerializeField] private float bossFocusLens = 30f;
        [SerializeField] private int normalPriority = 0;
        [SerializeField] private int bossPriority = 30;
        [SerializeField] private int bossInactivePriority = -1;
        [SerializeField] private Ease bossZoomEase = Ease.OutCubic;

        private Coroutine _bossFocusCoroutine;
        private MotionHandle _bossZoomHandle;

        private void Awake()
        {
            if (waveManager == null)
                waveManager = FindFirstObjectByType<WaveManager>();

            if (outputCamera == null)
                outputCamera = UnityCamera.main;

            if (normalCamera != null)
                normalCamera.Priority = normalPriority;

            if (bossCamera != null)
                bossCamera.Priority = bossInactivePriority;

            Debug.Assert(waveManager != null, "WaveManager is null", this);
            Debug.Assert(normalCamera != null, "Normal camera is null", this);
            Debug.Assert(bossCamera != null, "Boss camera is null", this);
        }

        private void OnEnable()
        {
            if (waveManager != null)
                waveManager.OnBossSpawn += HandleBossSpawn;
        }

        private void OnDisable()
        {
            if (waveManager != null)
                waveManager.OnBossSpawn -= HandleBossSpawn;

            StopBossFocus();
        }

        private void HandleBossSpawn(AbstractEnemy boss)
        {
            if (boss == null || bossCamera == null)
                return;

            if (_bossFocusCoroutine != null)
                StopCoroutine(_bossFocusCoroutine);

            _bossFocusCoroutine = StartCoroutine(BossFocusRoutine(boss));
        }

        private IEnumerator BossFocusRoutine(AbstractEnemy boss)
        {
            Transform target = boss.HitPos != null ? boss.HitPos : boss.transform;

            CopyCurrentCameraPose();
            SetBossTarget(target);
            PlayBossZoom();

            if (normalCamera != null)
                normalCamera.Priority = normalPriority;

            bossCamera.Priority = bossPriority;

            yield return new WaitForSeconds(bossFocusMoveDuration + bossFocusHoldDuration);

            bossCamera.Priority = bossInactivePriority;
            ClearBossTarget();
            _bossFocusCoroutine = null;
        }

        private void CopyCurrentCameraPose()
        {
            if (outputCamera == null)
                return;

            bossCamera.transform.SetPositionAndRotation(outputCamera.transform.position, outputCamera.transform.rotation);

            LensSettings lens = bossCamera.Lens;
            lens.FieldOfView = outputCamera.fieldOfView;
            bossCamera.Lens = lens;
        }

        private void SetBossTarget(Transform target)
        {
            bossCamera.Target = new CameraTarget
            {
                TrackingTarget = null,
                LookAtTarget = target,
                CustomLookAtTarget = true
            };
        }

        private void ClearBossTarget()
        {
            bossCamera.Target = new CameraTarget();
        }

        private void PlayBossZoom()
        {
            _bossZoomHandle.TryCancel();

            _bossZoomHandle = LMotion.Create(bossCamera.Lens.FieldOfView, bossFocusLens, bossFocusMoveDuration)
                .WithEase(bossZoomEase)
                .Bind(fov =>
                {
                    LensSettings lens = bossCamera.Lens;
                    lens.FieldOfView = fov;
                    bossCamera.Lens = lens;
                });
        }

        private void StopBossFocus()
        {
            _bossZoomHandle.TryCancel();

            if (_bossFocusCoroutine != null)
            {
                StopCoroutine(_bossFocusCoroutine);
                _bossFocusCoroutine = null;
            }

            if (bossCamera != null)
            {
                bossCamera.Priority = bossInactivePriority;
                ClearBossTarget();
            }
        }
    }
}

using Module;
using Unity.Cinemachine;
using UnityEngine;

namespace Agents.Module
{
    public class CameraImpulseModule : MonoBehaviour, IModule
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private float defaultForce = 1f;
        [SerializeField] private bool ensureListeners = true;

        public void Initialize(ModuleOwner owner)
        {
            if (impulseSource == null)
                impulseSource = GetComponent<CinemachineImpulseSource>();

            if (impulseSource == null)
                impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();

            if (ensureListeners)
                EnsureImpulseListeners();
        }

        public void GenerateImpulse()
        {
            GenerateImpulse(defaultForce);
        }

        public void GenerateImpulse(float force)
        {
            if (impulseSource == null)
                return;

            impulseSource.GenerateImpulseWithForce(force);
        }

        private void EnsureImpulseListeners()
        {
            CinemachineCamera[] cameras = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
            foreach (CinemachineCamera cinemachineCamera in cameras)
            {
                if (cinemachineCamera == null)
                    continue;

                if (!cinemachineCamera.TryGetComponent(out CinemachineImpulseListener _))
                    cinemachineCamera.gameObject.AddComponent<CinemachineImpulseListener>();
            }
        }
    }
}

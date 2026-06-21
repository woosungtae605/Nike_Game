using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.UI;
using Systems.GameSystem.Wave;
using Systems.SaveSystem;
using UnityEngine;

namespace Agents.Enemies
{
    public class PlayerSensor : MonoBehaviour
    {
        [SerializeField] private WaveInformationSO waveInformationSO;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private GameObject clearHideTarget;
        [SerializeField] private SaveFileNameSO waveClearSaveFile;

        private void Awake()
        {
            HideIfCleared();
        }

        private void OnEnable()
        {
            HideIfCleared();
        }

        private void HideIfCleared()
        {
            if (!WaveClearSave.IsCleared(waveClearSaveFile, waveInformationSO))
                return;

            GetHideTarget().SetActive(false);
        }

        private GameObject GetHideTarget()
        {
            if (clearHideTarget != null)
                return clearHideTarget;

            AbstractEnemy enemy = GetComponentInParent<AbstractEnemy>();
            if (enemy != null)
                return enemy.gameObject;

            Animator animator = GetComponentInParent<Animator>();
            if (animator != null)
                return animator.gameObject;

            Rigidbody rigid = GetComponentInParent<Rigidbody>();
            if (rigid != null)
                return rigid.gameObject;

            return gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (WaveClearSave.IsCleared(waveClearSaveFile, waveInformationSO))
            {
                HideIfCleared();
                return;
            }

            int targetMask = layerMask.value;
            if (targetMask == 0)
                targetMask = LayerMask.GetMask("Player");

            if ((targetMask & (1 << other.gameObject.layer)) == 0)
                return;

            if(other.TryGetComponent<Agent>(out Agent agent))
            {
                INavMovement navMove = agent.GetModule<INavMovement>();
                navMove.StopImmediately();

                PlayerMoveInput moveInput = agent.GetModule<PlayerMoveInput>();
                moveInput.CanClickChange(false);
                
                Bus<BattleGoUIEvent>.Raise(new BattleGoUIEvent(waveInformationSO));
            }
        }
    }
}


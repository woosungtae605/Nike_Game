using System.Collections;
using Agents.CombatSystem;
using Agents.Missiles;
using Agents.Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Enemies.CamoDeserts.Skills
{
    public class CamoDesertShootMissile : AbstractEnemySkill
    {
        [SerializeField] private AnimParamSO skillAnimParam;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform[] firePoints;
        [SerializeField] private Missile missile;
        [SerializeField] private float aimSpeed = 10f;
        
        private AgentTriggerModule _trigger;
        private GameObject _target;
        
        private Coroutine _attackCoroutine;
        
        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            _trigger = _enemy.Trigger;
        }
        public override bool CanUseSkill(GameObject target = null)
        {
            return target != null && !IsUsing && NormalizedCooldown >= 1f;
        }
        
        public override void UseSkill(GameObject target = null)
        {
            if (!CanUseSkill(target))
                return;

            base.UseSkill(target);
            _target = target;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
            _attackCoroutine = StartCoroutine(AimAndCastRoutine());
        }

        private IEnumerator AimAndCastRoutine()
        {
            if (_target == null || skillAnimParam == null)
            {
                StopSkill();
                yield break;
            }

            while (_target != null && aimTarget != null)
            {
                Vector3 targetPoint = GetTargetPoint(_target);
                aimTarget.position = Vector3.MoveTowards(aimTarget.position, targetPoint, aimSpeed * Time.deltaTime);

                if (Vector3.Distance(aimTarget.position, targetPoint) <= 0.1f)
                    break;

                yield return null;
            }

            if (_target == null)
            {
                StopSkill();
                yield break;
            }
            _enemy.Renderer.PlayClip(skillAnimParam.ParamHash, 0);
            if (_trigger != null)
            {
                _trigger.OnDamageCast -= HandleDamageCast;
                _trigger.OnAnimationEnd -= HandleAnimationEnd;
                _trigger.OnDamageCast += HandleDamageCast;
                _trigger.OnAnimationEnd += HandleAnimationEnd;
            }
        }
        
        private Vector3 GetTargetPoint(GameObject target)
        {
            if (target.TryGetComponent(out Collider targetCollider))
                return targetCollider.bounds.center;

            Collider childCollider = target.GetComponentInChildren<Collider>();
            if (childCollider != null)
                return childCollider.bounds.center;

            return target.transform.position;
        }

        public override void StopSkill()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
            
            _target = null;
            if (_trigger != null)
            {
                _trigger.OnDamageCast -= HandleDamageCast;
                _trigger.OnAnimationEnd -= HandleAnimationEnd;
            }
            
            base.StopSkill();
        }

        private void HandleAnimationEnd()
        {
            StopSkill();
        }

        private void HandleDamageCast()
        {
            foreach (Transform firePoint in firePoints)
            {
                Missile shotMissile = Instantiate(missile);
                shotMissile.transform.position = firePoint.position;
            }
        }
    }
}
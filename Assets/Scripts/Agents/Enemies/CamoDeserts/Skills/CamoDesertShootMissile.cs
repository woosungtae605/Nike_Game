using System.Collections;
using Agents.CombatSystem;
using Agents.Missiles;
using Agents.Module;
using Gamelib.ObjectPool.Runtime;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Enemies.CamoDeserts.Skills
{
    public class CamoDesertShootMissile : AbstractEnemySkill
    {
        [SerializeField] private AnimParamSO skillAnimParam;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform[] firePoints;
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private PoolItemSo missilePoolItem;
        [SerializeField] private float missileCurveAngle = 35f;

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
            if (_trigger != null)
            {
                _trigger.OnDamageCast -= HandleDamageCast;
                _trigger.OnAnimationEnd -= HandleAnimationEnd;
                _trigger.OnDamageCast += HandleDamageCast;
                _trigger.OnAnimationEnd += HandleAnimationEnd;
            }

            _enemy.Renderer.PlayClip(skillAnimParam.ParamHash, 0);
            _attackCoroutine = null;
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
            if (_target == null || poolManagerSo == null || missilePoolItem == null)
                return;

            Vector3 targetPoint = GetTargetPoint(_target);

            if (firePoints == null)
                return;

            int idx = 0;
            foreach (Transform firePoint in firePoints)
            {
                if (firePoint == null)
                    continue;

                Missile shotMissile = poolManagerSo.Pop<Missile>(missilePoolItem);
                if (shotMissile == null)
                    continue;

                shotMissile.PoolManagerSo = poolManagerSo;

                if(idx == 0)
                    shotMissile.Shot(firePoint.position, targetPoint, -missileCurveAngle);
                else
                    shotMissile.Shot(firePoint.position, targetPoint, missileCurveAngle);
                
                idx++;
            }
        }
    }
}
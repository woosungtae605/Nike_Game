using System.Collections;
using Agents.CombatSystem;
using Agents.Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Enemies.IronCladDesers.Skills
{
    public class IronCladDeserShotSkill : AbstractEnemySkill
    {
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float aimSpeed = 10f;
        
        [SerializeField] private AnimParamSO skillAnimParam;
        
        private EnemyDamageCaster _damageCaster;
        
        private AgentTriggerModule _trigger;
        private Coroutine _attackCoroutine;
        private GameObject _target;

        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            
            _damageCaster = _enemy.GetComponentInChildren<EnemyDamageCaster>();
            _damageCaster?.InitCaster(_enemy);
            _trigger = _enemy.GetModule<AgentTriggerModule>();
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
            if (_target == null)
                yield break;
            
            while (_target != null && aimTarget != null)
            {
                Vector3 targetPoint = GetTargetPoint(_target);
                aimTarget.position = Vector3.MoveTowards(aimTarget.position, targetPoint, aimSpeed * Time.deltaTime);
                if (Vector3.Distance(aimTarget.position, targetPoint) <= 0.1f)
                    break;
                
                yield return null;
            }
            
            _renderer.PlayClip(skillAnimParam.ParamHash, 0);
            
            if (_trigger != null)
            {
                _trigger.OnAnimationEnd -= StopSkill;
                _trigger.OnDamageCast -= CastDamage;
                _trigger.OnAnimationEnd += StopSkill;
                _trigger.OnDamageCast += CastDamage;
            }
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
                _trigger.OnAnimationEnd -= StopSkill;
                _trigger.OnDamageCast -= CastDamage;
            }
            base.StopSkill();
        }
        
        private void CastDamage()
        {
            if (_target == null)
                return;
            
            CastDamage(_target);
        }
        
        private void CastDamage(GameObject target)
        {
            if (_damageCaster == null)
                return;

            Vector3 origin = firePoint.position;
            Vector3 targetPoint = GetTargetPoint(target);
            Vector3 direction = targetPoint - origin;

            Debug.Log("Shot cast");
            _enemy.GunLineEffectModule?.Shot(0.1f, origin, targetPoint);
            _damageCaster.RayCastDamage(origin, direction,
                new DamageData { Attacker = _enemy, Damage = SkillData.damage },
                SkillData.maxDistance, SkillData.hitMask);
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
    }
}
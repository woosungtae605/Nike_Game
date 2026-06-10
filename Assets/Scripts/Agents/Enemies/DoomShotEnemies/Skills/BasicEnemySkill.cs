using System.Collections;
using Agents.CombatSystem;
using Agents.Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Enemies.DoomShotEnemies.Skills
{
    public class BasicEnemySkill : AbstractEnemySkill
    {
        private Transform _aimTarget;
        private Transform _firePoint;
        private EnemyDamageCaster _damageCaster;
        
        [SerializeField] private AnimParamSO skillAnimParam;
        [SerializeField] private float crossFadeDuration = 0.15f;
        [SerializeField] private float aimSpeed = 10f;

        private GameObject _target;
        private Coroutine _attackCoroutine;
        
        private AgentTriggerModule _trigger;

        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            
            _aimTarget = _enemy.transform.Find("Renderer/Rig 1/HeadAim/Target");
            _firePoint = _enemy.transform.Find("Renderer/root/base/center01/center02/center03/center04/cannon_01/cannon_02/cannon_03/cannon_04/cannon_front_02");
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

        public override void StopSkill()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
            _target = null;
            _trigger.OnAnimationEnd -= StopSkill;
            _trigger.OnDamageCast -= CastDamage;
            base.StopSkill();
        }

        private IEnumerator AimAndCastRoutine()
        {
            if (_target == null)
                yield break;
            
            yield return new WaitForSeconds(aimSpeed);

            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, aimSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target.transform.position) <= 0.1f)
                    break;
                
                yield return null;
            }
            
            _renderer.PlayClip(skillAnimParam.ParamHash, 0, crossFadeDuration);
            _trigger.OnAnimationEnd += StopSkill;
            _trigger.OnDamageCast += CastDamage;

            _attackCoroutine = null;
        }

        public void CastDamage()
        {
            if (_target != null)
                CastDamage(_target);
        }
        
        
        private void CastDamage(GameObject target)
        {
            if (_damageCaster == null)
                return;

            Vector3 origin = _firePoint != null ? _firePoint.position : _enemy.transform.position + Vector3.up;
            Vector3 targetPoint = GetTargetPoint(target);
            Vector3 direction = targetPoint - origin;

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

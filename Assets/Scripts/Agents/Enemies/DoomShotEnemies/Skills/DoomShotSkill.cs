using System.Collections;
using Agents.CombatSystem;
using Agents.Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Enemies.DoomShotEnemies.Skills
{
public class DoomShotSkill : AbstractEnemySkill
    {
        [SerializeField] private Transform _aimTarget;
        [SerializeField] private Transform _firePoint;
        private EnemyDamageCaster _damageCaster;
        
        [SerializeField] private AnimParamSO skillAnimParam;
        [SerializeField] private float aimSpeed = 10f;
        [SerializeField] private int shotCount = 3;
        [SerializeField] private float fallbackDamageDelay = 0.35f;
        [SerializeField] private float fallbackEndDelay = 1.2f;

        private GameObject _target;
        private Coroutine _attackCoroutine;
        private Coroutine _fallbackCoroutine;
        private bool _hasCastedDamage;
        private bool _animationEnd;
        
        private AgentTriggerModule _trigger;

        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);

            if (_aimTarget == null)
                _aimTarget = _enemy.transform.Find("Renderer/Rig 1/HeadAim/Target");
            if(_firePoint == null)
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
            
            if (_fallbackCoroutine != null)
            {
                StopCoroutine(_fallbackCoroutine);
                _fallbackCoroutine = null;
            }
            
            _target = null;
            if (_trigger != null)
            {
                _trigger.OnAnimationEnd -= HandleAnimationEnd;
                _trigger.OnDamageCast -= CastDamage;
            }
            base.StopSkill();
        }

        private IEnumerator AimAndCastRoutine()
        {
            if (_target == null)
                yield break;

            int count = Mathf.Max(1, shotCount);
            for (int i = 0; i < count; i++)
            {
                _hasCastedDamage = false;
                _animationEnd = false;
                
                while (_target != null && _aimTarget != null)
                {
                    Vector3 targetPoint = GetTargetPoint(_target);
                    _aimTarget.position = Vector3.MoveTowards(_aimTarget.position, targetPoint, aimSpeed * Time.deltaTime);
                    if (Vector3.Distance(_aimTarget.position, targetPoint) <= 0.1f)
                        break;
                    
                    yield return null;
                }

                if (_target == null || _renderer == null || skillAnimParam == null)
                {
                    Debug.LogWarning($"{nameof(DoomShotSkill)} attack canceled. Target:{_target != null}, Renderer:{_renderer != null}, SkillAnim:{skillAnimParam != null}", this);
                    _attackCoroutine = null;
                    StopSkill();
                    yield break;
                }
                
                _renderer.PlayClip(skillAnimParam.ParamHash, 0, 0);
                if (_trigger != null)
                {
                    _trigger.OnAnimationEnd -= HandleAnimationEnd;
                    _trigger.OnDamageCast -= CastDamage;
                    _trigger.OnAnimationEnd += HandleAnimationEnd;
                    _trigger.OnDamageCast += CastDamage;
                }
                
                if (_fallbackCoroutine != null)
                    StopCoroutine(_fallbackCoroutine);
                _fallbackCoroutine = StartCoroutine(FallbackDamageRoutine());

                while (!_animationEnd && _target != null)
                    yield return null;

                if (_fallbackCoroutine != null)
                {
                    StopCoroutine(_fallbackCoroutine);
                    _fallbackCoroutine = null;
                }

                if (_trigger != null)
                {
                    _trigger.OnAnimationEnd -= HandleAnimationEnd;
                    _trigger.OnDamageCast -= CastDamage;
                }
            }

            _attackCoroutine = null;
            StopSkill();
        }

        private void HandleAnimationEnd()
        {
            _animationEnd = true;
        }

        private void CastDamage()
        {
            if (_target == null)
                return;
            
            if (_hasCastedDamage)
                return;
            
            _hasCastedDamage = true;
            CastDamage(_target);
        }
        
        private IEnumerator FallbackDamageRoutine()
        {
            yield return new WaitForSeconds(fallbackDamageDelay);

            if (IsUsing && !_hasCastedDamage)
                CastDamage();

            float endDelay = Mathf.Max(0f, fallbackEndDelay - fallbackDamageDelay);
            yield return new WaitForSeconds(endDelay);

            if (IsUsing && !_animationEnd)
                _animationEnd = true;
            
            _fallbackCoroutine = null;
        }
        
        private void CastDamage(GameObject target)
        {
            if (_damageCaster == null)
                return;

            Vector3 origin = _firePoint != null ? _firePoint.position : _enemy.transform.position + Vector3.up;
            Vector3 targetPoint = GetTargetPoint(target);
            Vector3 direction = targetPoint - origin;

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

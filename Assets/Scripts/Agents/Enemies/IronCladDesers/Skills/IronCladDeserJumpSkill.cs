using System.Collections;
using Agents.CombatSystem;
using Agents.Module;
using Systems.AnimationSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Agents.Enemies.IronCladDesers.Skills
{
    public class IronCladDeserJumpSkill : AbstractEnemySkill
    {
        [SerializeField] private AnimParamSO skillAnimParam;
        [SerializeField] private float jumpDistance;
        [SerializeField] private float spacing;
        
        private GameObject _target;
        
        private AgentTriggerModule _trigger;
        private Rigidbody _rigid;
        private Coroutine _skillCoroutine;
        
        public override void InitializeSkill(ISkillModule skillModule)
        {
            base.InitializeSkill(skillModule);
            _trigger = _enemy.Trigger;
            _rigid = _enemy.GetComponent<Rigidbody>();
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

            _enemy.Renderer.PlayClip(skillAnimParam.ParamHash, 0);
            
            if (_trigger != null)
            {
                _trigger.OnDamageCast -= HandleDamageCast;
                _trigger.OnDamageCast += HandleDamageCast;
            }
        }

        private void HandleDamageCast()
        {
            _skillCoroutine = StartCoroutine(Jump());
        }

        private IEnumerator Jump()
        {
            NavMeshAgent agent = _enemy.NavMovement.NavAgent;
            

            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            agent.updatePosition = false;

            _rigid.useGravity = false;

            if (_target.transform.position.z + spacing < jumpDistance + _enemy.HitPos.position.z)
            {
                //뒤로 가는거
            }
            else
            {
                //앞으로 가는거
            }
        }

        public override void StopSkill()
        {
            if (_skillCoroutine != null)
            {
                StopCoroutine(_skillCoroutine);
                _skillCoroutine = null;
            }
            
            _target = null;
            if (_trigger != null)
            {
                _trigger.OnAnimationEnd -= StopSkill;
            }
            base.StopSkill();
        }
    }
}
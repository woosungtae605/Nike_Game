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
            
            [Header("Jump")]
            [SerializeField] private float jumpUpPower;
            [SerializeField] private float jumpDistance;
            [SerializeField] private float jumpDuration;
            [SerializeField] private float spacing;

            [Header("Landing")] 
            [SerializeField] private float randMin;
            [SerializeField] private float randMax;
            
            private GameObject _target;
            
            private AgentTriggerModule _trigger;
            private Rigidbody _rigid;
            private Coroutine _skillCoroutine;
            private NavMeshAgent _jumpAgent;
            private bool _hasCachedMovementState;
            private bool _cachedAgentStopped;
            private bool _cachedAgentUpdatePosition;
            private bool _cachedUseGravity;
            
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
                CacheMovementState(agent);
                
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                agent.updatePosition = false;

                _rigid.useGravity = false;

                //처음 위로 움직임
                float startTime = Time.time;

                Vector3 startPos = _enemy.transform.position;
                Vector3 endPos = _target.transform.position + Vector3.up * jumpDistance;

                while (jumpDuration > Time.time - startTime)
                {
                    float currentRatio = (Time.time - startTime) / jumpDuration;
                    _enemy.transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * jumpDistance, currentRatio);
                    
                    yield return null;
                }

                _enemy.transform.position = endPos;

                Vector3 goToPos;
                
                float rand = Random.Range(randMin, randMax);

                Vector3 enemyForward = _enemy.transform.forward;
                // 착륙하는거
                if (_target.transform.position.z + spacing < jumpDistance + _enemy.HitPos.position.z + rand) //후진
                {
                    goToPos =  _enemy.transform.position - enemyForward * jumpDistance + enemyForward * rand;
                }
                else //전진
                {
                    goToPos =  _target.transform.position - enemyForward * spacing + enemyForward * rand;
                }
                
                goToPos.y = _target.transform.position.y;
                
                Vector3 fallStartPos = goToPos + Vector3.up * jumpUpPower;
                _enemy.transform.position = fallStartPos;
                
                startTime = Time.time;
                
                while (jumpDuration > Time.time - startTime)
                {
                    float currentRatio = (Time.time - startTime) / jumpDuration;
                    _enemy.transform.position = Vector3.Lerp(fallStartPos, goToPos, currentRatio);
                    yield return null;
                }
                _enemy.transform.position = goToPos;
                
                agent.Warp(goToPos);
                RestoreMovementState();
                
                _enemy.SetGotoLeft(!_enemy.GotoLeft);
                
                StopSkill();
            }


            private void CacheMovementState(NavMeshAgent agent)
            {
                _jumpAgent = agent;
                _cachedAgentStopped = agent.isStopped;
                _cachedAgentUpdatePosition = agent.updatePosition;
                _cachedUseGravity = _rigid != null && _rigid.useGravity;
                _hasCachedMovementState = true;
            }

            private void RestoreMovementState()
            {
                if (!_hasCachedMovementState)
                    return;

                if (_jumpAgent != null)
                {
                    _jumpAgent.isStopped = _cachedAgentStopped;
                    _jumpAgent.updatePosition = _cachedAgentUpdatePosition;
                }

                if (_rigid != null)
                    _rigid.useGravity = _cachedUseGravity;

                _hasCachedMovementState = false;
                _jumpAgent = null;
            }

            public override void StopSkill()
            {
                RestoreMovementState();

                if (_skillCoroutine != null)
                {
                    StopCoroutine(_skillCoroutine);
                    _skillCoroutine = null;
                }
                
                _target = null;
                if (_trigger != null)
                {
                    _trigger.OnDamageCast -= HandleDamageCast;
                }
                base.StopSkill();
            }
        }
    }
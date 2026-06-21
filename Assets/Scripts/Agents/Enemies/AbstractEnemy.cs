    using Agents.CombatSystem;
    using Agents.Enemies.DoomShotEnemies.Events;
    using Agents.Enemies.Module;
    using Agents.Module;
    using Gamelib.ObjectPool.Runtime;
    using Systems.AnimationSystems;
    using Unity.Behavior;
    using UnityEngine;

    namespace Agents.Enemies
    {
        public abstract class AbstractEnemy : Agent, IPoolable
        {
            [field: SerializeField] public EnemyDataSO EnemyDataSo { get; private set; }
            [field: SerializeField] public Transform HitPos { get; private set; }
            
            public PoolItemSo PoolItem { get; set; }
            
            public GameObject GameObject => this != null ? gameObject : null;

            public IRenderer Renderer { get; private set; }
            public INavMovement NavMovement { get; private set; }
            public ISkillModule SkillModule { get; private set; }
            public BehaviorGraphAgent BTAgent { get; private set; }
            public AgentTriggerModule Trigger { get; private set; }
            public EnemySkillModule EnemySkillModule { get; private set; }
            public GunLineEffectModule GunLineEffectModule { get; private set; }
            public Agents.Enemies.Module.EnemyHitEffectModule HitEffectModule { get; private set; }
            public Agents.Enemies.Module.EnemyDeathEffectModule DeathEffectModule { get; private set; }
            
            public StateChannel StateChannel { get; private set; }
            
            private EnemyManager _enemyManager;
            private bool _hasDefaultNavAgentSettings;
            private bool _defaultNavAgentUpdatePosition;
            private bool _defaultNavAgentUpdateRotation;

            public bool GotoLeft { get; private set; } = false;

            protected override void InitializeComponents()
            {
                base.InitializeComponents();
                Renderer = GetModule<IRenderer>();
                NavMovement = GetModule<INavMovement>();
                BTAgent = GetComponent<BehaviorGraphAgent>();
                SkillModule = GetModule<ISkillModule>();
                Trigger = GetModule<AgentTriggerModule>();
                EnemySkillModule = GetModule<EnemySkillModule>();
                GunLineEffectModule = GetModule<GunLineEffectModule>();
                HitEffectModule = GetModule<Agents.Enemies.Module.EnemyHitEffectModule>();
                DeathEffectModule = GetModule<Agents.Enemies.Module.EnemyDeathEffectModule>();
            }

            protected override void AfterInitComponents()
            {
                base.AfterInitComponents();
                CaptureDefaultNavAgentSettings();
            }

            public override void ApplyDamage(DamageData damageData)
            {
                base.ApplyDamage(damageData);
                HitEffectModule?.Play(damageData, HitPos);
            }

            public void SetManager(EnemyManager enemyManager)
            {
                _enemyManager = enemyManager;
            }
            
            protected void ChangeState(EnemyState state)
            {
                StateChannel?.SendEventMessage(state);
            }
            
            public void SetGotoLeft(bool value)
            {
                Debug.Log($"[Enemy] SetGotoLeft {name}: {GotoLeft} -> {value}", this);
                GotoLeft = value;
            }
             
            public void SetVariableValue<T>(string variableName, T value)
            {
                Debug.Assert(!string.IsNullOrEmpty(variableName), "변수 이름은 비어있으면 안됩니다.");

                if (BTAgent.GetVariable<T>(variableName, out BlackboardVariable<T> variable))
                {
                    variable.Value = value;
                }
            }

            public bool GetVariable<T>(string variableName, out BlackboardVariable<T> variable)
            {
                Debug.Assert(!string.IsNullOrEmpty(variableName), "변수 이름은 비어있으면 안됩니다.");
                return BTAgent.GetVariable<T>(variableName, out variable);
            }

            private void CaptureDefaultNavAgentSettings()
            {
                if (NavMovement?.NavAgent == null)
                    return;

                _defaultNavAgentUpdatePosition = NavMovement.NavAgent.updatePosition;
                _defaultNavAgentUpdateRotation = NavMovement.NavAgent.updateRotation;
                _hasDefaultNavAgentSettings = true;
            }

            public virtual void ResetItem()
            {
                GotoLeft = false;
                EnemySkillModule?.StopCurrentSkill();
                HealthModule.ChangeHealth(EnemyDataSo.MaxHp);
                
                HealthModule.OnDeath -= HandleDeath;
                HealthModule.OnDeath += HandleDeath;
                
                if (GetVariable("StateChannel", out BlackboardVariable<StateChannel> channel))
                {
                    StateChannel = channel.Value;
                }
            }

            public virtual void PrepareSpawn(Vector3 spawnPosition)
            {
                transform.position = spawnPosition;
                ResetMovementState(spawnPosition);
                ResetAnimationState();
                ChangeState(EnemyState.IDLE);
                RestartBehaviorGraph();
            }

            private void ResetMovementState(Vector3 spawnPosition)
            {
                if (NavMovement?.NavAgent != null)
                {
                    var navAgent = NavMovement.NavAgent;

                    if (!navAgent.enabled)
                        navAgent.enabled = true;

                    navAgent.speed = EnemyDataSo.Speed;
                    navAgent.isStopped = false;

                    if (_hasDefaultNavAgentSettings)
                    {
                        navAgent.updatePosition = _defaultNavAgentUpdatePosition;
                        navAgent.updateRotation = _defaultNavAgentUpdateRotation;
                    }

                    if (navAgent.isOnNavMesh)
                    {
                        navAgent.ResetPath();
                        navAgent.velocity = Vector3.zero;
                        navAgent.Warp(spawnPosition);
                    }
                }

                Rigidbody rigid = GetComponent<Rigidbody>();
                if (rigid == null)
                    return;

                rigid.useGravity = true;
                rigid.linearVelocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
            }

            private void ResetAnimationState()
            {
                if (Renderer is NavAgentRendererModule navAgentRenderer)
                    navAgentRenderer.ResetRendererState();

                if (Renderer?.Animator == null)
                    return;

                Renderer.Animator.enabled = true;
                Renderer.Animator.speed = 1f;
                Renderer.Animator.Rebind();
                Renderer.Animator.Update(0f);
            }

            private void RestartBehaviorGraph()
            {
                if (BTAgent == null)
                    return;

                BTAgent.enabled = false;
                BTAgent.enabled = true;
            }
            protected virtual void HandleDeath()
            {
                HealthModule.OnDeath -= HandleDeath;
                PlayDeathEffect();

                if (_enemyManager != null)
                    _enemyManager.NotifyEnemyDead(this);
            }

            protected void PlayDeathEffect()
            {
                DeathEffectModule?.Play(transform);
            }
        }
    }

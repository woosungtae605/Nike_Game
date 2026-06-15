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
            
            public StateChannel StateChannel { get; private set; }
            
            private EnemyManager _enemyManager;

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

            public virtual void ResetItem()
            {
                GotoLeft = false;
                HealthModule.ChangeHealth(EnemyDataSo.MaxHp);
                
                HealthModule.OnDeath -= HandleDeath;
                HealthModule.OnDeath += HandleDeath;
                
                if (GetVariable("StateChannel", out BlackboardVariable<StateChannel> channel))
                {
                    StateChannel = channel.Value;
                }
            }
            
            protected virtual void HandleDeath()
            {
                HealthModule.OnDeath -= HandleDeath;

                if (_enemyManager != null)
                    _enemyManager.NotifyEnemyDead(this);
            }
        }
    }

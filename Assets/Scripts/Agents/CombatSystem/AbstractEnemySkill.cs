using System;
using Agents.Enemies;
using Agents.Enemies.Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.CombatSystem
{
    public abstract class AbstractEnemySkill : MonoBehaviour, ISkill
    {
        public event Action OnSkillEnd;
     
        [field: SerializeField] public SkillDataSO SkillData { get; private set; }

        protected AbstractEnemy _enemy;
        protected EnemySkillModule EnemySkillModule;
        protected IRenderer _renderer;
        protected float _lastUseTime;
        
        public bool IsUsing { get; private set; }
        public float NormalizedCooldown => Mathf.Approximately(SkillData.cooldown, 0)
            ? 1f
            : Mathf.Clamp01((Time.time - _lastUseTime) / SkillData.cooldown);
        
        public virtual void InitializeSkill(ISkillModule skillModule)
        {
            EnemySkillModule = skillModule as EnemySkillModule;
            Debug.Assert(EnemySkillModule != null, "스킬은 반드시 플레이어 스킬 모듈의 자식이어야 함.");
            _enemy = EnemySkillModule.Enemy;
            _renderer = _enemy.GetModule<IRenderer>();
            Debug.Assert(_renderer != null, "에너미는 렌더러 모듈을 가져야 합니다.");
            IsUsing = false;
        }

        public void LastTimeChange()
        {
            _lastUseTime = Time.time;
        }
        
        public abstract bool CanUseSkill(GameObject target = null);

        public virtual void UseSkill(GameObject target = null)
        {
            IsUsing = true;
        }
        
        public virtual void StopSkill()
        {
            IsUsing = false;
            _lastUseTime = Time.time;
            OnSkillEnd?.Invoke();
        }

    }
}
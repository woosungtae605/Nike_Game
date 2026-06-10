using System.Collections.Generic;
using System.Linq;
using Agents.CombatSystem;
using Agents.Players;
using Module;
using UnityEngine;

namespace Agents.Enemies.Module
{
    public class EnemySkillModule : MonoBehaviour, IModule, ISkillModule
    {
        public ModuleOwner Owner { get; private set; }
        public AbstractEnemy Enemy { get; private set; }
        public event System.Action OnCurrentSkillEnd;

        private Dictionary<int, ISkill> _skillDict;
        
        public Dictionary<int, ISkill> SkillDict => _skillDict;
        private ISkill _currentSkill;
        
        public void Initialize(ModuleOwner owner)
        {
            Owner = owner;
            Enemy = owner as AbstractEnemy;
            Debug.Assert(Enemy != null, "스킬 모듈은 Agent의 자식이어야 합니다.");

            _skillDict = GetComponentsInChildren<ISkill>()
                .ToDictionary(skill => skill.SkillData.skillIndex);
            foreach (ISkill skill in _skillDict.Values)
            {
                skill.InitializeSkill(this);
            }

        }
        
        public bool CanUseSkill(int skillIndex, GameObject target = null)
        {
            if (_currentSkill is { IsUsing: true })
                return false;
            if (_skillDict.TryGetValue(skillIndex, out ISkill skill))
            {
                return skill.CanUseSkill(target);
            }

            return false;
        }
        
        public void UseSkill(int skillIndex, GameObject target = null)
        {
            if (_skillDict.TryGetValue(skillIndex, out ISkill skill))
            {
                if (_currentSkill != null)
                {
                    _currentSkill.OnSkillEnd -= HandleCurrentSkillEnd;
                }
                _currentSkill = skill;
                _currentSkill.OnSkillEnd += HandleCurrentSkillEnd;
                skill.UseSkill(target);
            }
        }

        public void StopCurrentSkill()
        {
            _currentSkill?.StopSkill();
        }
        
        private void HandleCurrentSkillEnd()
        {
            _currentSkill.OnSkillEnd -= HandleCurrentSkillEnd;
            _currentSkill = null;
            InvokeSkillEnd();
        }

        public void InvokeSkillEnd()
        {
            OnCurrentSkillEnd?.Invoke();
        }
    }
}

using Agents.CombatSystem;
using Agents.Module;
using Module;

namespace Agents
{
    public class Agent : ModuleOwner, IDamageable
    {
        public HealthModule HealthModule { get; private set; }
        public ActionDataModule ActionData { get; private set; }
        

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            HealthModule = GetModule<HealthModule>();
            ActionData = GetModule<ActionDataModule>();
        }
        
        public void ApplyDamage(DamageData damageData)
        {
            if (ActionData != null)
            {
                ActionData.HitNormal = damageData.HitNormal;
                ActionData.HitPoint = damageData.HitPoint;
                ActionData.Attacker = damageData.Attacker;
                ActionData.Weakness = damageData.Weakness;
            }
            HealthModule.ApplyDamage(damageData.Damage);
        }
    }
}
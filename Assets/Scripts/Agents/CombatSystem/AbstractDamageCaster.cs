using Agents.Players.Gun.GunData;
using Module;
using UnityEngine;

namespace Agents.CombatSystem
{
    public abstract class AbstractDamageCaster : MonoBehaviour
    {
        public ModuleOwner CasterOwner { get; private set; }

        public virtual void InitCaster(ModuleOwner owner)
        {
            CasterOwner = owner;
        }
 
        public abstract void CastDamage(Vector3 position, Vector3 direction, DamageData damageData);
    }
}
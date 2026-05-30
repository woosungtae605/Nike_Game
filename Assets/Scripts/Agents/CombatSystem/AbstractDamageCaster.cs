using Agents.Players.Gun.GunData;
using Module;
using UnityEngine;

namespace Agents.CombatSystem
{
    public abstract class AbstractDamageCaster : MonoBehaviour
    {
        public Agent CasterOwner { get; private set; }

        public virtual void InitCaster(Agent owner)
        {
            CasterOwner = owner;
        }
 
        public abstract void RayCastDamage(Vector3 position, Vector3 direction, DamageData damageData);
        public abstract void SphereCastDamage(Vector3 position, Vector3 direction, DamageData damageData, float radius);
        public abstract void BoxCastDamage(Vector3 position, Vector3 direction, DamageData damageData, Vector3 halfExtents);
    }
}
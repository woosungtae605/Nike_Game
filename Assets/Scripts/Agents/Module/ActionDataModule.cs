using Module;
using UnityEngine;

namespace Agents.Module
{
    public class ActionDataModule : MonoBehaviour, IModule
    {
        public Vector3 HitPoint { get; set; }
        public Vector3 HitNormal { get; set; }
        public float HitDistance { get; set; }
        public ModuleOwner Attacker { get; set; }

        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }
    }
}
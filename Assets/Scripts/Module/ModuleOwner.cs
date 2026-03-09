using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Module
{
    public abstract class ModuleOwner : MonoBehaviour
    {
        protected Dictionary<Type, IModule> _moduleDict;
        protected virtual void Awake()
        {
            _moduleDict = GetComponentsInChildren<IModule>().ToDictionary(module => module.GetType());
            InitializeComponents();
            AfterInitComponents();
        }


        private void InitializeComponents()
        {
            foreach (IModule module in _moduleDict.Values)
            {
                module.Initialize(this);
            }
        }
        private void AfterInitComponents()
        {
            foreach (IAfterInitModule afterModule in _moduleDict.Values.OfType<IAfterInitModule>())
            {
                afterModule.AfterInit();
            }
        }

        public T GetModule<T>()
        {
            if (_moduleDict.TryGetValue(typeof(T), out IModule module))
            {
                return (T) module;
            }

            IModule findModule = _moduleDict.Values.FirstOrDefault(moduleType => moduleType is T);
            if (findModule != null && findModule is T castedModule)
            {
                return castedModule;
            }
            
            return default;
        }
    }
}
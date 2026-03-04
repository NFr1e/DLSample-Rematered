using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Framework
{
    public class ModulesManager
    {
        private readonly List<IModule> _modules = new();
        private readonly Dictionary<Type, IModule> _typeMap = new();

        private bool _isInitialized = false;

        public void Register<T>(T module) where T : IModule
        {
            if (_isInitialized) return;

            if (_typeMap.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"{module} Has been registerred");
                return;
            }

            _modules.Add(module);
            _typeMap[typeof(T)] = module;

            Debug.Log($"[{GetType().Name}]: Module \'<color=orange>[{module.GetType().Name}]</color>\' Has been \'<color=red>REGISTERED</color>\'");
        }

        public void Init()
        {
            _modules.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            HandleModuleRequires();

            foreach (var module in _modules)
            {

                Debug.Log($"[{GetType().Name}]: Module \'<color=orange>[{module.GetType().Name}]</color>\' Has been '<color=cyan>INITIALIZED</color>'");
                module.OnInit();
            }
            _isInitialized = true;
        }

        public void Update(float deltaTime)
        {
            if (!_isInitialized) return;

            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].OnUpdate(deltaTime);
            }
        }
        public void Dispose()
        {
            foreach (var module in _modules)
            {
                module.OnShutdown();
            }
            _modules?.Clear();
        }

        private void HandleModuleRequires()
        {
            foreach (var module in _modules)
            {
                var interfaces = module.GetType().GetInterfaces();

                foreach (var @interface in interfaces)
                {
                    if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IModuleRequire<>))
                    {
                        Type targetType = @interface.GetGenericArguments()[0];

                        if (_typeMap.TryGetValue(targetType, out var dependency))
                        {
                            MethodInfo setMethod = @interface.GetMethod("SetModule");
                            setMethod.Invoke(module, new object[] { dependency });
                        }
                        else
                        {
                            Debug.LogWarning($"\'<color=red>[ModuleManager]</color> {module.GetType().Name} Need {targetType.Name}£¬but it's noy registered\'");
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DLSample.Facility
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly Dictionary<Type, Action> _onServiceReady = new();

        public void Register<TService>(TService service) where TService : class
        {
            var type = typeof(TService);

            if (_services.ContainsKey(type)) return;
            _services[type] = service;

            NotifyServiceReady<TService>();
        }
        public void Unregister<TService>()
        {
            _services.Remove(typeof(TService));
        }

        public T Get<T>() where T : class
        {
            if(_services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }
            else
            {
                Debug.LogError($"Service {typeof(T)} is not regiterred");
            }

            return null;
        }

        public bool TryGet<TService>(out TService service) where TService : class
        {
            var type = typeof(TService);
            if (_services.TryGetValue(type, out var s))
            {
                service = s as TService;
                return true;
            }

            service = null;
            return false;
        }

        public void WhenServicesReady(Action callback, params Type[] types)
        {
            if (types == null || types.Length == 0)
            {
                callback?.Invoke();
                return;
            }

            var pending = new HashSet<Type>(types);

            void OnOneServiceReady()
            {
                if (pending.Count == 0) return;

                pending.RemoveWhere(t => _services.ContainsKey(t));

                if (pending.Count == 0)
                {
                    callback?.Invoke();
                }
            }

            foreach (var type in pending.ToArray())
            {
                if (_services.ContainsKey(type))
                {
                    OnOneServiceReady();
                }
                else
                {
                    if (!_onServiceReady.ContainsKey(type))
                        _onServiceReady[type] = null;

                    _onServiceReady[type] += OnOneServiceReady;
                }
            }
        }

        public void NotifyServiceReady<TService>() where TService : class
        {
            var type = typeof(TService);

            if (_onServiceReady.TryGetValue(type, out var callbacks))
            {
                callbacks?.Invoke();
                _onServiceReady.Remove(type);
            }
        }

        public void Dispose()
        {
            _services?.Clear();
            _onServiceReady?.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Facility.EnityFramework
{
    public class EntityPool<T> where T : Component, IPoolabelEntity
    {
        private readonly GameObject _prefab;
        private readonly Transform _container;

        private readonly Queue<T> _pooledObjects = new();
        private readonly List<T> _unpooledObjects = new();

        private readonly int _maxCapacity;

        private bool _prewarmed = false;
        private int _currentSize = 0;

        public EntityPool(GameObject prefab, int maxCapacity = 0, Transform container = default)
        {
            _prefab = prefab;
            _container = container;

            _pooledObjects ??= new();
            _unpooledObjects ??= new();

            _maxCapacity = maxCapacity < 0 ? 0 : maxCapacity;
            _currentSize = 0;

            _prewarmed = false;

            if (!_prefab.TryGetComponent<T>(out _))
            {
                throw new ArgumentNullException(nameof(prefab));
            }
        }

        public void Prewarm(int count)
        {
            if (_prewarmed) return;

            count = Mathf.Clamp(count, 0, _maxCapacity);

            for (int i = 0; i < count; ++i)
            {
                var instance = CreateInstance();
                _pooledObjects.Enqueue(instance);
                instance.OnEnpool();
            }

            _prewarmed = true;
        }

        public virtual T Get()
        {
            if (!_prewarmed)
            {
                Debug.LogWarning($"[EntityPool<{typeof(T).Name}>] has not been prewarmed!");
                Prewarm(_maxCapacity);
            }

            T instance;

            if (_pooledObjects.Count > 0)
            {
                instance = _pooledObjects.Dequeue();
            }
            else if (_maxCapacity <= 0 || _currentSize < _maxCapacity)
            {
                instance = CreateInstance();
            }
            else
            {
                Return(_unpooledObjects[0]);
                instance = _pooledObjects.Dequeue();
            }

            instance.OnDepool();
            _unpooledObjects.Add(instance);

            return instance;
        }

        public virtual void Return(T instance)
        {
            if (instance == null)
                return;

            _unpooledObjects.Remove(instance);
            _pooledObjects.Enqueue(instance);
            instance.OnEnpool();
        }

        public virtual void ReturnAll()
        {
            foreach(var e in _unpooledObjects.ToArray())
            {
                Return(e);
            }
        }
        public virtual void Dispose()
        {
            foreach (var instance in _pooledObjects)
            {
                if(instance)
                    GameObject.Destroy(instance.gameObject);
            }

            _pooledObjects.Clear();
            _unpooledObjects.Clear();

            _currentSize = 0;
            _prewarmed = false;
        }

        private T CreateInstance()
        {
            var go = GameObject.Instantiate(_prefab, _container);

            if (!go.TryGetComponent<T>(out var component))
            {
                GameObject.Destroy(go);
                throw new InvalidOperationException($"Prefab missing required component {typeof(T)}");
            }

            _currentSize++;
            return component;
        }
    }
}
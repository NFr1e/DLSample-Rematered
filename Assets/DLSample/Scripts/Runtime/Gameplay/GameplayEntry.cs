using UnityEngine;
using DLSample.Framework;
using DLSample.Facility;
using DLSample.Facility.Events;
using System.Collections.Generic;
using DLSample.Gameplay.Behaviours;

namespace DLSample.Gameplay
{
    public class GameplayEntry : MonoBehaviour
    {
        #region InstanceGetter
        private static GameplayEntry _instance;
        public static GameplayEntry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<GameplayEntry>();

                    if (_instance == null)
                    {
                        _instance = CreateInstance();
                    }
                }

                _instance.Awake();
                return _instance;
            }
        }

        private static GameplayEntry CreateInstance()
        {
            return new GameObject
            {
                name = typeof(GameplayEntry).Name + "_LazyLoad",
                hideFlags = HideFlags.NotEditable,
            }.AddComponent<GameplayEntry>();
        }
        #endregion

        public EventBus EventBus { get; private set; }
        public AsyncEventBus AsyncEventBus { get; private set; }
        public ModulesManager ModulesManager { get; private set; }
        public ServiceLocator ServiceLocator { get; private set; }

        private readonly List<GameplayObject> _gameplayObjects = new();

        private bool _initialized = false;
        private bool _started = false;
        private bool _disposed = false;

        public bool IsInitialized => _initialized;

        private void Awake() => OnInit();
        private void Start() => OnStart();
        private void Update() => OnUpdate();
        private void OnDestroy() => OnDispose();

        #region Lifecycle
        private void OnInit()
        {
            if (_initialized) return;

            CreateFacilities();
            _initialized = true;
        }

        private void OnStart()
        {
            InitGameplayObjects();

            ModulesManager.Init();
            ModulesManager.Start();

            _started = true;
        }

        private void OnUpdate()
        {
            if (_started)
            {
                ModulesManager.Update(Time.deltaTime);
            }
        }

        private void OnDispose()
        {
            if (_disposed) return;

            EventBus.Dispose();
            AsyncEventBus.Dispose();
            ModulesManager.Dispose();
            ServiceLocator.Dispose();

            _disposed = true;
            _started = false;
        }

        #region Internals
        private void CreateFacilities()
        {
            EventBus = new EventBus();
            AsyncEventBus = new AsyncEventBus();
            ServiceLocator = new ServiceLocator();
            ModulesManager = new ModulesManager();
        }
        private void InitGameplayObjects()
        {
            foreach (var obj in _gameplayObjects)
            {
                if (obj != null)
                {
                    obj.DoStart();
                }
            }
        }
        #endregion

        #endregion

        #region PublicAPI
        public void RegisterGameplayObject(GameplayObject gameplayObject)
        {
            if (!_started)
            {
                if (_gameplayObjects.Contains(gameplayObject)) return;

                _gameplayObjects.Add(gameplayObject);
            }
            else
            {
                gameplayObject.DoStart();
            }
        }
        #endregion
    }
}

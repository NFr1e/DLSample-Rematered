using UnityEngine;
using DLSample.Shared;
using DLSample.Gameplay.Phase;
using DLSample.Gameplay.Stream;
using DLSample.Facility.Events;
using DLSample.Facility;
using DLSample.Framework;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DLSample.Gameplay.Behaviours
{
    public class GameplayManagerComponent : GameplayObject
    {
#if ODIN_INSPECTOR
        [Title("Configs")]
#endif
        [SerializeField] private LevelDataScriptable levelData;

#if ODIN_INSPECTOR
        [Title("Player")]
#endif
        [SerializeField] private GameplayPlayerMove mainPlayer;

#if ODIN_INSPECTOR
        [Title("Stream")]
#endif
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        private GameplayFSM _fsm;
        private GameplayStateHandler _stateHandler;

        private BacktrackablesHandler _backtrackHandler;
        private CheckpointHandler _checkpointHandler;

        private GameplayTimer _timer;
        private GameplayTimerDirector _timerDirector;

        private GameplayPlayerController _playerController;
        private GameplayInputHandler _inputHandler;

        private GameplaySoundtrackPlayer _soundtrackPlayer;
        private GameplaySoundtrackDirector _soundtrackDirector;

        private GameplayResulter _progressCounter;

        private GameplayInitPipeline _initializer;

        private EventBus eventBus;
        private ServiceLocator serviceLocator;
        private ModulesManager modulesManager;

        protected override void OnInit()
        {
            eventBus = GameplayEntry.Instance.EventBus;
            serviceLocator = GameplayEntry.Instance.ServiceLocator;
            modulesManager = GameplayEntry.Instance.ModulesManager;

            _fsm = new GameplayFSM();
            _stateHandler = new GameplayStateHandler(eventBus, _fsm);

            _backtrackHandler = new BacktrackablesHandler();
            _checkpointHandler = new CheckpointHandler(eventBus);

            _timer = new GameplayTimer();
            _timerDirector = new GameplayTimerDirector(eventBus, _timer, _backtrackHandler);

            _playerController = new GameplayPlayerController(eventBus, mainPlayer, _stateHandler, _checkpointHandler, _backtrackHandler);
            _inputHandler = new GameplayInputHandler(eventBus, _playerController);

            _soundtrackPlayer = new GameplaySoundtrackPlayer(audioClip, audioSource);
            _soundtrackDirector = new GameplaySoundtrackDirector(eventBus, _soundtrackPlayer, _backtrackHandler);

            _progressCounter = new GameplayResulter(eventBus, levelData, _timer);

            serviceLocator.Register<BacktrackablesHandler>(_backtrackHandler);
            serviceLocator.Register<CheckpointHandler>(_checkpointHandler);
            serviceLocator.Register<GameplayTimer>(_timer);
            serviceLocator.Register<GameplayResulter>(_progressCounter);
            serviceLocator.Register<LevelDataScriptable>(levelData);
        }
        protected override void OnStart()
        {
            modulesManager.Register(_stateHandler);
            modulesManager.Register(_backtrackHandler);
            modulesManager.Register(_checkpointHandler);
            modulesManager.Register(_timerDirector);
            modulesManager.Register(_playerController);
            modulesManager.Register(_inputHandler);
            modulesManager.Register(_soundtrackDirector);
            modulesManager.Register(_progressCounter);

            CreateInitPipeline();
        }
        protected override void OnExit()
        {
            serviceLocator.Unregister<CheckpointHandler>();
            serviceLocator.Unregister<GameplayTimer>();

            _fsm = null;
            _stateHandler = null;
            _backtrackHandler = null;
            _checkpointHandler = null;
            _playerController = null;
            _inputHandler = null;
            _soundtrackPlayer = null;
            _soundtrackDirector = null;

            _initializer = null;
        }

        private void CreateInitPipeline()
        {
            _initializer = new GameplayInitPipeline(
                eventBus, 
                _playerController, mainPlayer,
                levelData, _progressCounter);

            modulesManager.Register(_initializer);
        }
    }
}

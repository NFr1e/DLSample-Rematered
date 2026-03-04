using UnityEngine;
using DLSample.Shared;
using DLSample.Gameplay.Phase;
using DLSample.Gameplay.Stream;

namespace DLSample.Gameplay.Behaviours
{
    public class GameplayManagerComponent : GameplayComponentBase
    {
        [SerializeField] private LevelDataScriptable levelData;

        [SerializeField] private GameplayPlayerMove mainPlayer;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        private GameplayFSM _fsm;
        private GameplayStateHandler _stateHandler;

        private BacktrackablesHandler _backtrackHandler;
        private CheckpointHandler _checkpointHandler;

        private GameplayPlayerController _playerController;
        private GameplayInputHandler _inputHandler;

        private GameplaySoundtrackPlayer _soundtrackPlayer;
        private GameplaySoundtrackDirector _soundtrackDirector;

        private GameplayInitPipeline _initializer;

        protected override void OnInit()
        {
            _fsm = new GameplayFSM();
            _stateHandler = new GameplayStateHandler(EventBus, _fsm);

            _backtrackHandler = new BacktrackablesHandler();
            _checkpointHandler = new CheckpointHandler(EventBus);

            _playerController = new GameplayPlayerController(EventBus, mainPlayer, _stateHandler, _checkpointHandler, _backtrackHandler);
            _inputHandler = new GameplayInputHandler(EventBus, _playerController);

            _soundtrackPlayer = new GameplaySoundtrackPlayer(audioClip, audioSource);
            _soundtrackDirector = new GameplaySoundtrackDirector(EventBus, _soundtrackPlayer, _backtrackHandler);

            ModulesManager.Register(_stateHandler);
            ModulesManager.Register(_backtrackHandler);
            ModulesManager.Register(_playerController);
            ModulesManager.Register(_inputHandler);
            ModulesManager.Register(_soundtrackDirector);

            CreateInitPipeline();
        }
        protected override void OnExit()
        {
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
                EventBus, 
                _playerController, mainPlayer);

            ModulesManager.Register(_initializer);
        }
    }
}

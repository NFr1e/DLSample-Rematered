using Cysharp.Threading.Tasks;
using DLSample.App;
using DLSample.Facility.Events;
using DLSample.Framework;
using DLSample.Gameplay.Behaviours;
using DLSample.Gameplay.Phase;
using DLSample.Shared;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DLSample.Gameplay
{
    public class StairController : IModule
    {
        public int Priority => DLSampleConsts.Gameplay.PRIORITY_STAIR_CONTROLLER;

        private readonly StairComponent _stair;
        private readonly Transform _player;
        private readonly Vector3 _playerOriginRotation;

        private GameInput _gameInput;
        private EventBus _evtBus;

        private GameplayStateBase _currentState;

        private readonly GameplayEventParams.WaitingGameRequest _waitingGameRequest = new();

        public StairController(StairComponent stair, Transform player, Vector3 playerOriginalRotation)
        {
            _stair = stair;
            _player = player;
            _playerOriginRotation = playerOriginalRotation;
        }

        public void OnInit()
        {
            _gameInput = AppEntry.GameInput;
            _evtBus = GameplayEntry.Instance.EventBus;

            _evtBus.Subscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
            _gameInput.Gameplay.PlayerInput.performed += OnPlayerInputed;
            _gameInput.App.Cancel.performed += OnCancelInputed;

            _evtBus.Invoke(this, _waitingGameRequest);

            _player.eulerAngles = _playerOriginRotation; // 避免状态竞争所以出此下策将角度初始化写在这...
        }
        public void OnShutdown()
        {
            _evtBus.Unsubscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
            _gameInput.Gameplay.PlayerInput.performed -= OnPlayerInputed;
            _gameInput.App.Cancel.performed -= OnCancelInputed;
        }
        public void OnUpdate(float deltaTime) { }

        private void OnStateChange(GameplayEventParams.GameplayStateChangeCtx ctx)
        {
            _currentState = ctx.CurrentState;
        }

        private async void OnPlayerInputed(InputAction.CallbackContext ctx)
        {
            await UniTask.Yield();
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            if (_currentState is GameplayStates.WaitingState)
            {
                _stair.Land();
            }
        }
        private async void OnCancelInputed(InputAction.CallbackContext ctx)
        {
            await UniTask.Yield();
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            if(_currentState is GameplayStates.PreparingState or GameplayStates.WaitingState)
            {
                _stair.Rise();
            }
        }
    }
}

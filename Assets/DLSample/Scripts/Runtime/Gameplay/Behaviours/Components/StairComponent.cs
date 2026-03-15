using UnityEngine;
using DG.Tweening;
using DLSample.Facility.Events;

namespace DLSample.Gameplay.Behaviours
{
    public class StairComponent : GameplayObject
    {
        [SerializeField] private Transform stairTransform;
        [SerializeField] private GameplayPlayerMove player;
        [SerializeField] private Vector3 landPosition = new(0, -2, 0);
        [SerializeField] private GameObject landEffect;

        private StairController _controller;

        private EventBus _eventBus;
        private readonly GameplayEventParams.PrepareGameRequest _prepareGameRequest = new();
        private readonly GameplayEventParams.WaitingGameRequest _waitingGameRequest = new();

        private Transform _playerOriginalParent;
        private Vector3 _originalPosition;
        private Vector3 _playerOriginEuler;

        private Tween stairTween;

        private bool _isLanding = false;
        private bool _isRising = false;

        protected override void OnInit()
        {
            _originalPosition = stairTransform.transform.position;
            _playerOriginEuler = player.PlayerParams.Directions.StartRotation().eulerAngles - new Vector3(0, -90, 0);
            _playerOriginalParent = player.transform.parent;

            _controller = new StairController(this, player.transform, _playerOriginEuler);
            _eventBus = GameplayEntry.Instance.EventBus;
        }
        protected override void OnStart()
        {
            GameplayEntry.Instance.ModulesManager.Register(_controller);
        }

        public void Land()
        {
            if (_isLanding) return;

            _isRising = false;
            _isLanding = true;

            player.transform.SetParent(stairTransform);

            stairTween?.Kill();

            Destroy(Instantiate(landEffect, player.transform.position, Quaternion.identity), 1f);

            stairTween = DOTween.Sequence()
                .Join(stairTransform.DOMove(landPosition, 0.8f).SetEase(Ease.InOutQuad))
                .Join(player.transform.DORotateQuaternion(player.PlayerParams.Directions.StartRotation(), 0.8f))
                .OnComplete(OnLanded);
        }
        public void Rise()
        {
            if (_isRising) return;

            _isLanding = false;
            _isRising = true;

            _eventBus.Invoke(this, _waitingGameRequest);

            player.transform.SetParent(stairTransform);

            stairTween?.Kill();

            stairTween = DOTween.Sequence()
                .Join(stairTransform.DOMove(_originalPosition, 0.8f))
                .Join(player.transform.DORotate(_playerOriginEuler, 0.8f))
                .OnComplete(OnRised);
        }

        private void OnLanded()
        {
            player.transform.SetParent(_playerOriginalParent);
            player.transform.rotation = player.PlayerParams.Directions.StartRotation();

            _eventBus.Invoke(this, _prepareGameRequest);

            _isLanding = false;
        }
        private void OnRised()
        {
            _isRising = false;
        }
    }
}

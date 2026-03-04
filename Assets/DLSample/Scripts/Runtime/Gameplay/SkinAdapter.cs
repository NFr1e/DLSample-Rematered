using DLSample.Gameplay.Behaviours;
using DLSample.Gameplay.Behaviours.Skin;
using UnityEngine;

namespace DLSample.Gameplay.Skin
{
    /// <summary>
    /// 持有当前SkinBehaviour实例（通过SkinChanger注入），通过委托实现应用皮肤效果
    /// </summary>
    public class SkinAdapter
    {
        private readonly IPlayerMove _player;
        private readonly Transform _headContainer;

        protected SkinBehaviourBase _currentSkinBehaviour;

        public SkinAdapter(IPlayerMove player, Transform headContainer)
        {
            _player = player;
            _headContainer = headContainer;
        }

        public virtual void Init()
        {
            _player.OnStartMove += OnStartMove;
            _player.OnStopMove += OnStopMove;
            _player.OnMoving += OnPlayerMoving;
            _player.OnTurn += OnPlayerTurn;
            _player.OnLand += OnPlayerLand;
        }
        public virtual void Dispose()
        {
            _player.OnStartMove -= OnStartMove;
            _player.OnStopMove -= OnStopMove;
            _player.OnMoving -= OnPlayerMoving;
            _player.OnTurn -= OnPlayerTurn;
            _player.OnLand -= OnPlayerLand;
        }

        public void SetCurrentSkin(SkinBehaviourBase skinBehaviour)
        {
            _currentSkinBehaviour = skinBehaviour;

            if(_currentSkinBehaviour)
                _currentSkinBehaviour.SetHeadContainer(_headContainer);
        }

        private void OnStartMove(PlayerMovingArgs arg)
        {
            if (_currentSkinBehaviour != null) 
                _currentSkinBehaviour.OnStartMove(arg);
        }

        private void OnStopMove(PlayerMovingArgs arg)
        {
            if(_currentSkinBehaviour != null)
                _currentSkinBehaviour.OnStopMove(arg);
        }

        private void OnPlayerMoving(PlayerMovingArgs arg)
        {
            if(_currentSkinBehaviour != null)
                _currentSkinBehaviour.OnPlayerMoving(arg);
        }

        private void OnPlayerLand(PlayerMovingArgs arg)
        {
            if(_currentSkinBehaviour != null)
                _currentSkinBehaviour.OnPlayerLand(arg);
        }

        private void OnPlayerTurn(PlayerMovingArgs arg)
        {
            if(_currentSkinBehaviour != null)
                _currentSkinBehaviour.OnPlayerTurn(arg);
        }
    }
}

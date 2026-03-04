using DLSample.Gameplay.Behaviours;
using UnityEngine;

namespace DLSample.Gameplay.Skin
{
    public class GameplaySkinAdapter : SkinAdapter
    {
        public PlayerDamager _damager;

        public GameplaySkinAdapter(IPlayerMove player, Transform headContainer, PlayerDamager damager) : base(player, headContainer) 
        { 
            _damager = damager;
        }

        public override void Init()
        {
            base.Init();
            _damager.OnDie += OnPlayerDie;
        }

        public override void Dispose()
        {
            base.Dispose();
            _damager.OnDie -= OnPlayerDie;
        }

        private void OnPlayerDie(PlayerEventsParams.PlayerDieArg arg)
        {
            if(_currentSkinBehaviour != null)
                _currentSkinBehaviour.OnPlayerDie(arg);
        }
    }
}

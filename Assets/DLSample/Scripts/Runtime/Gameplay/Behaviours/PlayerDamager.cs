using System;
using UnityEngine;
using DLSample.Facility.Events;
using DLSample.Shared;


namespace DLSample.Gameplay.Behaviours
{
    public enum PlayerDiecause
    {
        None,
        Obstacle,
        Drown,
        Border
    }

    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class PlayerDamager : GameplayObject
    {
        public GameplayPlayerMove player;

        public LayerMask obstacleLayer;
        public LayerMask drownLayer;
        public LayerMask borderLayer;

        private EventBus _evtBus;
        private PlayerEventsParams.PlayerDieArg _dieArg = new();

        public event Action<PlayerEventsParams.PlayerDieArg> OnDie;

        protected override void OnInit()
        {
            _evtBus = GameplayEntry.Instance.EventBus;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(LayerHelper.IsLayer(other.gameObject, drownLayer))
            {
                RequestDamage(PlayerDiecause.Drown);
            }
            if(LayerHelper.IsLayer(other.gameObject, borderLayer))
            {
                RequestDamage(PlayerDiecause.Border);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(LayerHelper.IsLayer(collision.gameObject, obstacleLayer))
            {
                RequestDamage(PlayerDiecause.Obstacle);
                return;
            }
        }

        public void RequestDamage(PlayerDiecause diecause)
        {
            _dieArg.DieCause = diecause;
            _dieArg.MovingArgs = player.MovingArgs;

            _evtBus.Invoke(this, _dieArg);
            OnDie?.Invoke(_dieArg);
        }
    }
}

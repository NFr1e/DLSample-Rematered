using UnityEngine;
using DLSample.Facility.Events;
using System;

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
    public class PlayerDamager : GameplayComponentBase
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
        protected override void OnExit() { }

        private void OnTriggerEnter(Collider other)
        {
            if(IsLayer(other.gameObject, drownLayer))
            {
                RequestDamage(PlayerDiecause.Drown);
            }
            if(IsLayer(other.gameObject, borderLayer))
            {
                RequestDamage(PlayerDiecause.Border);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(IsLayer(collision.gameObject, obstacleLayer))
            {
                RequestDamage(PlayerDiecause.Obstacle);
                return;
            }
        }

        public void RequestDamage(PlayerDiecause diecause)
        {
            _dieArg.dieCause = diecause;
            _dieArg.movingArgs = player.MovingArgs;

            _evtBus.Invoke(this, _dieArg);
            OnDie?.Invoke(_dieArg);
        }
        private bool IsLayer(GameObject go, LayerMask mask)
        {
            return (mask.value & (1 << go.layer)) != 0;
        }
    }
}

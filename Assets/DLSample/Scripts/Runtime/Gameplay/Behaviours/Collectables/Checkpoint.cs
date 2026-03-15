using UnityEngine;
using DLSample.Gameplay.Stream;

namespace DLSample.Gameplay.Behaviours
{
    public class Checkpoint : GameplayObject
    {
        [SerializeField] protected double checkTime = 0;

        [SerializeField] private Transform mainPlayerRespawnTransform;
        [SerializeField] private bool visulize = false;

        protected bool _consumed = false;

        private GameplayTimer _timer;
        protected GameplayTimer.TickEvent _checkTickEvent;

        public Transform MainPlayerTransform => mainPlayerRespawnTransform;

        public double CheckTime => checkTime;

        protected override void OnStart()
        {
            _checkTickEvent = new(checkTime, Check);

            _timer = GameplayEntry.Instance.ServiceLocator.Get<GameplayTimer>();
            _timer.RegisterTickEvent(_checkTickEvent);
        }
        protected override void OnExit()
        {
            _timer?.UnregisterTickEvent(_checkTickEvent);
        }

        protected virtual void Check()
        {
            if (GameplayEntry.Instance.ServiceLocator.TryGet<CheckpointHandler>(out var cpHnadler))
            {
                cpHnadler.Check(this);
            }
        }
        public virtual void Consume()
        {
            _consumed = true;
        }

        private void OnDrawGizmos()
        {
            if (mainPlayerRespawnTransform && visulize)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(mainPlayerRespawnTransform.position + Vector3.up * 2, 0.2f);
                Gizmos.DrawLine(mainPlayerRespawnTransform.position + Vector3.up * 2, mainPlayerRespawnTransform.position + mainPlayerRespawnTransform.rotation * Vector3.forward + Vector3.up * 2);
                Gizmos.DrawCube(mainPlayerRespawnTransform.position, Vector3.one);
            }
        }
    }
}

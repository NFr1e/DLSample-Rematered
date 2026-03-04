using UnityEngine;
using DLSample.Gameplay.Stream;

namespace DLSample.Gameplay.Behaviours
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] protected double checkTime = 0;

        [SerializeField] private Transform mainPlayerRespawnTransform;
        [SerializeField] private bool visulize = false;

        protected bool _consumed = false;

        private GameplayTimer _timer;
        protected GameplayTimer.TickEvent _checkTickEvent;

        public Transform MainPlayerTransform => mainPlayerRespawnTransform;

        public double CheckTime => checkTime;

        private void Start()
        {
            Init();
        }
        private void OnDestroy()
        {
            Dispose();
        }

        private void Init()
        {
            if (_timer is null)
            {
                GameplayEntry.Instance.ServiceLocator.TryGet<GameplayTimer>(out _timer);
            }

            _timer?.RegisterTickEvent(_checkTickEvent);
        }
        private void Dispose()
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

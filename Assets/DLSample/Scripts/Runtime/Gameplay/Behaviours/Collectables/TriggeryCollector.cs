using DLSample.App;
using DLSample.Facility.Events;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class TriggeryCollector : MonoBehaviour, ICollector
    {
        private EventBus _evtBus;
        private OnCollectEventArgs _onCollectEventArgs;

        private void Awake()
        {
            _evtBus = AppEntry.EventBus;

            _onCollectEventArgs = new OnCollectEventArgs()
            {
                collector = this,
            };    
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<ICollectable>(out var collectable))
            {
                Collect(collectable);
            }
        }
        public void Collect(ICollectable collectable)
        {
            collectable.Collect();

            _onCollectEventArgs.collectable = collectable;
            _evtBus?.Invoke(this, _onCollectEventArgs);
        }
    }
}

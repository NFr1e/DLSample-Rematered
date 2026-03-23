using DLSample.Facility.Events;
using DLSample.Shared;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class TriggeryCollector : GameplayObject, ICollector
    {
        [SerializeField] private LayerMask excludeLayers;

        private EventBus _evtBus;
        private OnCollectEventArgs _onCollectEventArgs;

        protected override void OnStart()
        {
            _evtBus = GameplayEntry.Instance.EventBus;

            _onCollectEventArgs = new OnCollectEventArgs()
            {
                collector = this,
            };    
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<ICollectable>(out var collectable) && !LayerHelper.IsLayer(other.gameObject, excludeLayers))
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

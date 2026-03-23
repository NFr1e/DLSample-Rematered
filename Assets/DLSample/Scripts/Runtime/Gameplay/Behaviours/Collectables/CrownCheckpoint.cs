using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public class CrownCheckpoint : Checkpoint, ICollector
    {
        [SerializeField] private Crown crown;
        public Crown Crown => crown;

        private OnCollectEventArgs _onCollect = new();

        protected override void Check()
        {
            base.Check();

            crown.Collect();

            _onCollect.collector = this;
            _onCollect.collectable = crown;
            GameplayEntry.Instance.EventBus.Invoke(this, _onCollect);
        }
        public override void Consume()
        {
            if (_consumed) return;

            base.Consume();
            crown.Consume();
        }
    }
}

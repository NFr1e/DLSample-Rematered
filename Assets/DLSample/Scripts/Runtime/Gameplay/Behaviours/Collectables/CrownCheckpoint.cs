using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public class CrownCheckpoint : Checkpoint
    {
        [SerializeField] private Crown crown;
        public Crown Crown => crown;

        protected override void Check()
        {
            base.Check();

            crown.Collect();
        }
        public override void Consume()
        {
            if (_consumed) return;

            base.Consume();
            crown.Consume();
        }
    }
}

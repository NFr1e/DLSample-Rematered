using DLSample.Facility.Events;

namespace DLSample.Gameplay.Behaviours
{
    public interface ICollector
    {
        public struct OnCollectEventArgs : IEventArg
        {
            public ICollectable collectable;
            public ICollector collector;
        }
        public interface ICollector
        {
            void Collect(ICollectable collectable);
        }
    }
}

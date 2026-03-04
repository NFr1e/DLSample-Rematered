using System;

namespace DLSample.Gameplay.Behaviours
{
    public interface ICollectable
    {
        string TypeId { get; }
        bool IsCollected { get; }
        void Collect();

        event Action OnCollect;
    }
}

using System;
using System.Collections.Generic;

namespace DLSample.Facility.Events
{
    public class EventBus
    {
        private readonly Dictionary<Type, EventPool> _eventsDic = new();
        private readonly object _lock = new();

        public void Subscribe<TArg>(Action<TArg> action) where TArg : IEventArg
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_lock)
            {
                var eventType = typeof(TArg);

                if (!_eventsDic.TryGetValue(eventType, out var eventPool))
                {
                    eventPool = new EventPool();
                    _eventsDic.Add(eventType, eventPool);
                }

                eventPool.AddSubscriber(action);
            }
        }

        public void Unsubscribe<TArg>(Action<TArg> action) where TArg : IEventArg
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_lock)
            {
                var eventType = typeof(TArg);
                if (_eventsDic.TryGetValue(eventType, out var eventPool))
                {
                    eventPool.RemoveSubscriber(action);
                }
            }
        }

        public void Invoke<TArg>(object sender, TArg args) where TArg : IEventArg
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            lock (_lock)
            {
                var eventType = typeof(TArg);

                if (!_eventsDic.TryGetValue(eventType, out var eventPool)) return;

                eventPool.Trigger(sender, args);
            }
        }

        public void ClearAllEvents()
        {
            lock (_lock)
            {
                foreach (var pool in _eventsDic.Values)
                {
                    pool.Clear();
                }
                _eventsDic.Clear();
            }
        }

        public void Dispose()
        {
            ClearAllEvents();
        }
    }
}

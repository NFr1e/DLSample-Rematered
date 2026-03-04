using System;
using System.Collections.Generic;

namespace DLSample.Facility.Events
{
    public class EventPool
    {
        private readonly List<Delegate> _subscribers = new();
        private readonly object _lock = new();

        public void AddSubscriber<TArg>(Action<TArg> action) where TArg : IEventArg
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_lock)
            {
                if (!_subscribers.Contains(action))
                {
                    _subscribers.Add(action);
                }
            }
        }

        public void RemoveSubscriber<TArg>(Action<TArg> action) where TArg : IEventArg
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_lock)
            {
                if (_subscribers.Contains(action))
                {
                    _subscribers.Remove(action);
                }
            }
        }

        public void Trigger<TArg>(object sender, TArg args) where TArg : IEventArg
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            Delegate[] copySubscribers;
            lock (_lock)
            {
                copySubscribers = _subscribers.ToArray();
            }

            foreach (var subscriber in copySubscribers)
            {
                try
                {
                    (subscriber as Action<TArg>)?.Invoke(args);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[EventBus] 触发 {typeof(TArg).Name} 事件时订阅者执行异常: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _subscribers.Clear();
            }
        }
    }
}

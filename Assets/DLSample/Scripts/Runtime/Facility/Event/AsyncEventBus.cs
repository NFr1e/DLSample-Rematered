using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace DLSample.Facility.Events
{
    public class AsyncEventBus
    {
        private readonly Dictionary<Type, AsyncEventPool> _eventsDic = new();
        private readonly object _lock = new();

        /// <summary>
        /// 订阅异步事件
        /// 注意：action 必须是 async 方法或返回 Task 的方法
        /// </summary>
        public void Subscribe<TArg>(Func<TArg, UniTask> action) where TArg : IEventArg
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            lock (_lock)
            {
                var eventType = typeof(TArg);

                if (!_eventsDic.TryGetValue(eventType, out var eventPool))
                {
                    eventPool = new AsyncEventPool();
                    _eventsDic.Add(eventType, eventPool);
                }

                eventPool.AddSubscriber(action);
            }
        }

        /// <summary>
        /// 取消订阅异步事件
        /// </summary>
        public void Unsubscribe<TArg>(Func<TArg, UniTask> action) where TArg : IEventArg
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

        /// <summary>
        /// 触发异步事件
        /// 调用者需要使用 await 等待所有订阅者执行完毕
        /// </summary>
        public async UniTask InvokeAsync<TArg>(object sender, TArg args) where TArg : IEventArg
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            AsyncEventPool eventPool;
            lock (_lock)
            {
                var eventType = typeof(TArg);
                if (!_eventsDic.TryGetValue(eventType, out eventPool))
                {
                    return;
                }
            }

            await eventPool.TriggerAsync(sender, args);
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

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DLSample.Facility.Events
{
    public class AsyncEventPool
    {
        // 存储异步委托
        private readonly List<object> _subscribers = new();
        private readonly object _lock = new();

        /// <summary>
        /// 添加异步订阅者
        /// </summary>
        public void AddSubscriber<TArg>(Func<TArg, UniTask> action) where TArg : IEventArg
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

        /// <summary>
        /// 移除异步订阅者
        /// </summary>
        public void RemoveSubscriber<TArg>(Func<TArg, UniTask> action) where TArg : IEventArg
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

        /// <summary>
        /// 触发异步事件
        /// 并发执行所有订阅者，并等待它们全部完成
        /// </summary>
        public async UniTask TriggerAsync<TArg>(object sender, TArg args) where TArg : IEventArg
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            // 快照复制，防止遍历过程中集合被修改
            object[] copySubscribers;
            lock (_lock)
            {
                copySubscribers = _subscribers.ToArray();
            }

            if (copySubscribers.Length == 0) return;

            var tasks = new List<UniTask>(copySubscribers.Length);

            foreach (var subscriber in copySubscribers)
            {
                try
                {
                    if (subscriber is Func<TArg, UniTask> asyncAction)
                    {
                        // 启动任务但不立即 await，以便并发执行
                        tasks.Add(asyncAction(args));
                    }
                }
                catch (Exception ex)
                {
                    // 捕获同步阶段抛出的异常（例如在构建 Task 时）
                    UnityEngine.Debug.LogError($"[AsyncEventPool] 订阅者启动异常 ({typeof(TArg).Name}): {ex.Message}");
                }
            }

            if (tasks.Count > 0)
            {
                try
                {
                    // 等待所有任务完成
                    await UniTask.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    // Task.WhenAll 会抛出聚合异常中的第一个，或者如果是单个任务失败则直接抛出
                    // 这里统一捕获，防止未观察到的异常导致程序崩溃
                    Debug.LogError($"[AsyncEventPool] 事件执行异常 ({typeof(TArg).Name}): {ex.Message}");
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

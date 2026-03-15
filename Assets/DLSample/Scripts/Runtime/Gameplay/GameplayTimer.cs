using System;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Gameplay.Stream
{
    public class GameplayTimer : IStreamPlayer
    {
        public bool IsPlaying { get; private set; } = false;
        public double CurrentTime { get; private set; } = 0;

        private double _lastProcessTime = 0;

        public void Play()
        {
            if (IsPlaying) return;
            IsPlaying = true;
        }

        public void Stop()
        {
            if (!IsPlaying) return;
            IsPlaying = false;
        }

        public void Seek(double timeSecond)
        {
            timeSecond = Math.Max(0.0, timeSecond);
            CurrentTime = timeSecond;

            _lastProcessTime = timeSecond;
        }

        public void Tick(float deltaTime)
        {
            if (IsPlaying)
            {
                double startTime = _lastProcessTime;
                CurrentTime += deltaTime;

                ProcessTickEvents(startTime, CurrentTime);
                _lastProcessTime = CurrentTime;
            }
        }

        #region TickEventSystem

        private readonly List<TickEvent> _tickEvents = new();

        public class TickEvent
        {
            public double Time { get; }
            public Action Callback { get; }

            public TickEvent(double time, Action callback)
            {
                Time = time;
                Callback = callback;
            }
        }

        public TickEvent RegisterTickEvent(TickEvent tickEvent)
        {
            Debug.Log($"TickEvent[{tickEvent.Time},{tickEvent.Callback.Method.Name}] Registered");

            int index = _tickEvents.BinarySearch(tickEvent, Comparer<TickEvent>.Create((a, b) => a.Time.CompareTo(b.Time)));

            if (index < 0)
                index = ~index;

            _tickEvents.Insert(index, tickEvent);
            return tickEvent;
        }

        public bool UnregisterTickEvent(TickEvent tickEvent)
        {
            return _tickEvents.Remove(tickEvent);
        }

        private void ProcessTickEvents(double lastTime, double currentTime)
        {
            int count = _tickEvents.Count;
            if (count == 0) return;

            int left = 0;
            int right = count - 1;
            int startIndex = count;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (_tickEvents[mid].Time > lastTime)
                {
                    startIndex = mid;
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            for (int i = startIndex; i < count; i++)
            {
                var evt = _tickEvents[i];

                if (evt.Time > currentTime) break;

                try
                {
                    evt.Callback?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        #endregion
    }
}

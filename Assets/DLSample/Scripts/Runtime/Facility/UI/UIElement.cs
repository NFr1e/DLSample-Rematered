using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace DLSample.Facility.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        [Serializable]
        public class UIElementCallbacks
        {
            public UnityEvent onLoad;
            public UnityEvent onLoaded;
            public UnityEvent onUnload;
            public UnityEvent onUnloaded;
            public UnityEvent onPause;
            public UnityEvent onPaused;
            public UnityEvent onResume;
            public UnityEvent onResumed;
        }

        public UIElementCallbacks Callbacks = new();

        public bool IsActive => isActive;
        private bool isActive = false;
        protected bool isDestroyed = false;

        private CancellationTokenSource loadCts = new(), unloadCts = new(), pauseCts = new(), resumeCts = new();

        public async void Load()
        {
            isActive = true;

            loadCts?.Cancel();
            loadCts = new();

            Callbacks.onLoad?.Invoke();

            await OnLoadAsync(loadCts.Token);
            OnLoaded();
        }
        public async void Unload()
        {
            isActive = false;

            unloadCts?.Cancel();
            unloadCts = new();

            Callbacks.onUnload?.Invoke();

            await OnUnloadAsync(unloadCts.Token);
            OnUnloaded();
        }
        public async void Pause()
        {
            if (!isActive) return;
            isActive = false;

            pauseCts?.Cancel();
            pauseCts = new();

            Callbacks.onPause?.Invoke();

            await OnPauseAsync(pauseCts.Token);
            OnPaused();
        }
        public async void Resume()
        {
            if (isActive) return;
            isActive = true;

            resumeCts?.Cancel();
            resumeCts = new();

            Callbacks.onResume?.Invoke();

            await OnResumeAsync(resumeCts.Token);
            OnResumed();
        }

        public void Update()
        {
            if (isActive)
            {
                OnUpdate();
            }
        }

        protected virtual void OnLoaded()
        {
            Callbacks.onLoaded?.Invoke();
        }
        protected virtual void OnUnloaded()
        {
            Callbacks.onUnloaded?.Invoke();

            if (!isDestroyed)
            {
                isDestroyed = true;

                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
        }
        protected virtual void OnPaused()
        {
            Callbacks.onPaused?.Invoke();
        }
        protected virtual void OnResumed()
        {
            Callbacks.onResumed?.Invoke();
        }
        protected virtual void OnUpdate()
        {

        }

        #region Async Methods
        public virtual UniTask OnLoadAsync(CancellationToken token = default)
        {
            return UniTask.CompletedTask;
        }
        public virtual UniTask OnUnloadAsync(CancellationToken token = default)
        {
            return UniTask.CompletedTask;
        }
        public virtual UniTask OnPauseAsync(CancellationToken token = default)
        {
            return UniTask.CompletedTask;
        }
        public virtual UniTask OnResumeAsync(CancellationToken token = default)
        {
            return UniTask.CompletedTask;
        }
        #endregion
    }
}

using System;
using UnityEngine;
using DLSample.Facility;

namespace DLSample.Gameplay.Behaviours
{
    public class Gem : GameplayObject, ICollectable, IBacktrackable
    {
        [SerializeField] private Renderer mRenderer;
        [SerializeField] private GameObject collectEffect;
        [SerializeField] private float effectLifetime = 10;

        public event Action OnCollect;
        public string TypeId => "Collectables.Gem";
        public bool IsCollected { get; private set; } = false;

        private GameObject _currentEffectInstance;
        private BacktrackablesHandler _backtrackablesHandler;

        private void Awake()
        {
            IsCollected = false;
        }
        protected override void OnInit()
        {
            _backtrackablesHandler = GameplayEntry.Instance.ServiceLocator.Get<BacktrackablesHandler>();

            RegisterBacktrack();
        }
        protected override void OnStart()
        {

        }
        protected override void OnExit()
        {
            UnregisterBacktrack();
        }

        public void Collect()
        {
            if (IsCollected) return;

            OnCollected();

            OnCollect?.Invoke();
            IsCollected = true;
        }
        private void OnCollected()
        {
            mRenderer.enabled = false;

            if (_currentEffectInstance)
            {
                Destroy(_currentEffectInstance);
            }
            _currentEffectInstance = Instantiate(collectEffect, transform.position, Quaternion.identity, transform);
            Destroy(_currentEffectInstance, effectLifetime);
        }

        #region Backtrack
        public int BacktrackPriority => 5;
        public double BacktrackTime { get; set; }

        public void GetBacktrackState() { }

        public void Backtrack()
        {
            IsCollected = false;
            mRenderer.enabled = true;

            Destroy(_currentEffectInstance);
        }

        private void RegisterBacktrack()
        {
            _backtrackablesHandler?.Register(this);
        }
        private void UnregisterBacktrack()
        {
            _backtrackablesHandler?.Unregister(this);
        }
        #endregion
    }
}

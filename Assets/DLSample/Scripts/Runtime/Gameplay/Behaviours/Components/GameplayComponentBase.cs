using UnityEngine;
using DLSample.Framework;
using DLSample.Facility;
using DLSample.Facility.Events;

namespace DLSample.Gameplay.Behaviours
{
    public abstract class GameplayComponentBase : MonoBehaviour
    {
        protected GameplayEntry GameplayEntry;

        protected EventBus EventBus => GameplayEntry.EventBus;
        protected ModulesManager ModulesManager => GameplayEntry.ModulesManager;
        protected ServiceLocator ServiceLocator => GameplayEntry.ServiceLocator;

        private void Awake()
        {
            GameplayEntry = GameplayEntry.Instance;
            GameplayEntry.RegisterComponent(this);
        }
        private void OnDestroy()
        {
            GameplayEntry.UnregisterComponent(this);
        }

        public void OnEnterGameplay()
        {
            OnInit();
        }
        public void OnExitGameplay()
        {
            OnExit();
        }

        protected abstract void OnInit();
        protected abstract void OnExit();
    }
}

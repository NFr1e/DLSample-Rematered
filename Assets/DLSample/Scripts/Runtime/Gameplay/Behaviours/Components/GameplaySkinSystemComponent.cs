using DLSample.Gameplay.Skin;
using DLSample.Shared;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public class GameplaySkinSystemComponent : GameplayObject
    {
        [SerializeField] private SkinDataScriptable skinsData;
        [SerializeField] private Transform skinsContainer;

        private SkinChanger _changer;
        private SkinsHandler _handler;

        protected override void OnInit()
        {
            _changer = new SkinChanger(skinsData, skinsContainer);
            _handler = new SkinsHandler(_changer);

            GameplayEntry.Instance.ModulesManager.Register(_changer);
            GameplayEntry.Instance.ModulesManager.Register(_handler);

            GameplayEntry.Instance.ServiceLocator.Register<SkinChanger>(_changer);
        }

        protected override void OnExit() 
        {
            GameplayEntry.Instance.ServiceLocator.Unregister<SkinChanger>();

            _changer = null;
            _handler = null;
        }
    }
}

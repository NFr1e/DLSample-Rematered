using DLSample.Gameplay.Skin;
using DLSample.Shared;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public class GameplaySkinSystemComponent : GameplayComponentBase
    {
        [SerializeField] private SkinDataScriptable skinsData;
        [SerializeField] private Transform skinsContainer;

        private SkinChanger _changer;
        private SkinsHandler _handler;

        protected override void OnInit()
        {
            _changer = new SkinChanger(skinsData, skinsContainer);
            _handler = new SkinsHandler(_changer);

            ModulesManager.Register(_changer);
            ModulesManager.Register(_handler);

            ServiceLocator.Register<SkinChanger>(_changer);
        }

        protected override void OnExit() 
        {
            ServiceLocator.Unregister<SkinChanger>();

            _changer = null;
            _handler = null;
        }
    }
}

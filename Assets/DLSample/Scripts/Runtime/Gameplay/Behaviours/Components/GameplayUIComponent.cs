using UnityEngine;
using DLSample.Shared.UI;
using DLSample.Facility.UI;

namespace DLSample.Gameplay.Behaviours
{
    public class GameplayUIComponent : GameplayObject
    {
        [SerializeField] private UIPanelsDataScriptable panelsConfig;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private Transform uiContainer;

        [SerializeField] private GameplayUIMapper gameplayUIMapper;

        private UIElementManager _uiManager;
        private GameplayUIHandler _handler;

        protected override void OnInit()
        {
            _uiManager ??= new();
            _handler = new(GameplayEntry.Instance.EventBus, GameplayEntry.Instance.ServiceLocator, _uiManager, gameplayUIMapper);

            _uiManager.SetupConfigs(panelsConfig);
            _uiManager.SetupCamera(uiCamera);
            _uiManager.SetupContainers(uiContainer, uiContainer);
        }
        protected override void OnStart()
        {
            GameplayEntry.Instance.ModulesManager.Register(_handler);
        }
    }
}

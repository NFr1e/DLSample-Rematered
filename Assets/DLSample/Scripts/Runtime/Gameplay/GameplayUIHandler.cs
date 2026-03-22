using DLSample.Facility;
using DLSample.Facility.UI;
using DLSample.Facility.Events;
using DLSample.Gameplay.Phase;
using DLSample.Framework;
using DLSample.Shared;
using DLSample.Shared.UI;

namespace DLSample.Gameplay
{
    public class GameplayUIHandler : IModule
    {
        public int Priority => DLSampleConsts.Gameplay.PRIORITY_UI_HANDLER;

        private readonly EventBus _evtBus;
        private readonly ServiceLocator _serviceLocator;
        private readonly UIElementManager _uiManager;
        private readonly GameplayUIMapper _mapper;

        private CheckpointHandler _checkpointHandler;

        public GameplayUIHandler(EventBus eventBus, ServiceLocator serviceLocator, UIElementManager uiManager, GameplayUIMapper mapper)
        {
            _evtBus = eventBus;
            _serviceLocator = serviceLocator;
            _uiManager = uiManager;
            _mapper = mapper;
        }
        public void OnInit()
        {
            _checkpointHandler = _serviceLocator.Get<CheckpointHandler>();

            _evtBus.Subscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
        }
        public void OnShutdown()
        {
            _evtBus.Unsubscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
        }
        public void OnUpdate(float _) { }

        private async void OnStateChange(GameplayEventParams.GameplayStateChangeCtx ctx) 
        {
            switch(ctx.CurrentState)
            {
                case GameplayStates.PreparingState:
                    _ = await _uiManager.OpenPanel(_mapper.PreparePanelId);
                    break;

                case GameplayStates.OverState:
                    if(_checkpointHandler is not null)
                    {
                        if(_checkpointHandler.IsCheckpointed)
                        {
                            _ = await _uiManager.OpenPanel(_mapper.RespawnPanelId);
                            return;
                        }
                    }

                    _ = await _uiManager.OpenPanel(_mapper.OverPanelId);
                    break;

                case GameplayStates.PauseState:
                    _ = await _uiManager.OpenPanel(_mapper.PausePanelId);
                    break;

                case GameplayStates.WaitingState:
                    break;

                default:
                    await _uiManager.CloseAllFullscreenPanel();
                    break;
            }
        }
    }
}

using DLSample.Facility.Events;
using DLSample.Framework;
using DLSample.Gameplay.Behaviours;
using DLSample.Shared;

namespace DLSample.Gameplay
{
    public class GameplayInitPipeline : IModule
    {
        public int Priority => DLSampleConsts.Gameplay.PRIORITY_INITIALIZER;

        private readonly EventBus _evtBus;
        private readonly GameplayEventParams.PrepareGameRequest _prepareGameRequest = new();

        private readonly GameplayPlayerMove _mainPlayer;
        private readonly GameplayPlayerController _playerController;

        public GameplayInitPipeline(
            EventBus evtBus,
            GameplayPlayerController playerController, GameplayPlayerMove mainPlayer) 
        { 
            _evtBus = evtBus;
            _playerController = playerController;
            _mainPlayer = mainPlayer;
        }

        public void OnInit()
        {
            _playerController.AddPlayer(_mainPlayer);

            _evtBus.Invoke(this, _prepareGameRequest);
        }
        public void OnShutdown() { }
        public void OnUpdate(float _) { }
    }
}

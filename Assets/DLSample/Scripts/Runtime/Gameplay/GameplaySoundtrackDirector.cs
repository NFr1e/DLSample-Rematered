using Cysharp.Threading.Tasks;
using DLSample.Facility.Events;
using DLSample.Framework;
using DLSample.Gameplay.Phase;
using DLSample.Shared;

namespace DLSample.Gameplay.Stream
{
    public class GameplaySoundtrackDirector : IModule, ISyncable, IBacktrackable
    {
        public int Priority => DLSampleConsts.Gameplay.PRIORITY_SOUNDTRACK_DIRECTOR;

        private readonly GameplaySoundtrackPlayer _soundtrackPlayer;
        private readonly BacktrackablesHandler _backtracksHandler;

        private readonly EventBus _evtBus;

        private bool _synced = false;

        public int BacktrackPriority => DLSampleConsts.Gameplay.BACKTRACK_PRIORITY_SOUNDTRACK_DIRECTOR;
        
        public GameplaySoundtrackDirector(EventBus eventBus, GameplaySoundtrackPlayer player, BacktrackablesHandler backtrackHandler)
        {
            _evtBus = eventBus;
            _soundtrackPlayer = player;
            _backtracksHandler = backtrackHandler;
        }

        public void OnInit()
        {
            _soundtrackPlayer.Init();

            SubscribeEvents();
            _backtracksHandler?.Register(this);
        }

        public void OnShutdown()
        {
            UnsubscribeEvents();
            _backtracksHandler?.Unregister(this);

            _soundtrackPlayer.Dispose();
        }
        private void SubscribeEvents()
        {
            _evtBus?.Subscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
        }
        private void UnsubscribeEvents()
        {
            _evtBus?.Unsubscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
        }
        private void OnStateChange(GameplayEventParams.GameplayStateChangeCtx ctx)
        {
            switch(ctx.CurrentState)
            {
                case GameplayStates.GamingState:
                    Play();
                    break;
                case GameplayStates.PauseState:
                    Stop();
                    break;
                case GameplayStates.OverState:
                    Fadeout();
                    break;
            }
        }

        private async void Play()
        {
            if(!_synced)
                await SyncDelay();

            _soundtrackPlayer?.Play();
        }
        private void Stop()
        {
            _soundtrackPlayer?.Stop();
        }
        private void Fadeout()
        {
            _soundtrackPlayer?.Fadeout();
        }
        public async UniTask SyncDelay()
        {
            _synced = true;
            await UniTask.Delay(0);
        }

        public void Backtrack()
        {
            _soundtrackPlayer.Seek(_backtracksHandler.CurrentBacktrackTime);
        }
    }
}

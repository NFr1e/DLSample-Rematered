using Cysharp.Threading.Tasks;
using DLSample.App;
using DLSample.Gameplay.Phase;
using DLSample.Facility.Events;

namespace DLSample.Gameplay.Stream
{
    public class GameplayTimeLineDirector : ISyncable
    {
        private readonly IStreamPlayer _soundtrackPlayer;

        private EventBus _evtBus;
        private bool _synced = false;

        public GameplayTimeLineDirector(IStreamPlayer player)
        {
            _soundtrackPlayer = player;
        }

        public void Init()
        {
            _evtBus = GameplayEntry.Instance.EventBus;

            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
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
            switch (ctx.CurrentState)
            {
                case GameplayStates.GamingState:
                    Play();
                    break;
                case GameplayStates.PauseState:
                    Stop();
                    break;
            }
        }

        private async void Play()
        {
            if (!_synced)
                await SyncDelay();

            _soundtrackPlayer?.Play();
        }
        private void Stop()
        {
            _soundtrackPlayer?.Stop();
        }

        public async UniTask SyncDelay()
        {
            _synced = true;
            await UniTask.Delay(0);
        }
    }
}

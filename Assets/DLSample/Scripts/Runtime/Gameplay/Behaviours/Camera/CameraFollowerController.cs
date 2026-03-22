using DLSample.Gameplay.Phase;
using DLSample.Facility.Events;
using DLSample.Framework;
using DLSample.Shared;

namespace DLSample.Gameplay.Behaviours
{
    public class CameraFollowerController : IModule, IBacktrackable, IModuleRequire<BacktrackablesHandler>
    {
        public int Priority { get; set; }

        private readonly EventBus _evtBus;

        private CameraFollower _follower;
        private BacktrackablesHandler _backtrackHandler;

        private bool _follow = false;

        public CameraFollowerController(EventBus eventBus)
        {
            _evtBus = eventBus;
        }

        public void OnInit()
        {
            RegisterEvents();

            _backtrackHandler?.Register(this);
        }
        public void OnShutdown()
        {
            UnregisterEvents();

            _backtrackHandler?.Unregister(this);
        }
        public void OnUpdate(float deltaTime)
        {
            if (_follower && _follow)
            {
                _follower.Follow();
            }
        }

        private void RegisterEvents()
        {
            _evtBus?.Subscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
        }

        private void UnregisterEvents()
        {
            _evtBus?.Unsubscribe<GameplayEventParams.GameplayStateChangeCtx>(OnStateChange);
        }

        private void OnStateChange(GameplayEventParams.GameplayStateChangeCtx ctx)
        {
            if (ctx.CurrentState is not GameplayStates.GamingState)
                _follow = false;
            else _follow = true;
        }

        public void ChangeFollower(CameraFollower follower)
        {
            if(follower != null)
                _follower = follower;
        }

        #region Backtrack
        public int BacktrackPriority => DLSampleConsts.Gameplay.BACKTRACK_PRIORITY_CAMERA_FOLLOWER;
        public void Backtrack()
        {
            _follower.FocusTarget();
        }
        #endregion

        #region ModuleRequire
        public void SetModule(BacktrackablesHandler backtrackableHandler)
        {
            _backtrackHandler = backtrackableHandler;
        }
        #endregion
    }
}

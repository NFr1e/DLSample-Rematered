using DLSample.Facility.Events;
using DLSample.Gameplay.Phase;

namespace DLSample.Gameplay
{
    public struct GameplayEventParams
    {
        public struct OnEnterGameplayMsg : IEventArg { }
        public struct OnExitGameplayMsg : IEventArg { }

        public struct WaitingGameRequest : IEventArg { }
        public struct PrepareGameRequest : IEventArg { }
        public struct StartGameRequest : IEventArg { }
        public struct PauseGameRequest : IEventArg { }
        public struct RespawnGameRequest : IEventArg { }
        public struct BacktrackGameRequest : IEventArg { }

        public struct GameplayStateChangeCtx : IEventArg
        {
            public GameplayStateBase CurrentState { get; set; }
            public GameplayStateBase PrevState { get; set; }
        }
    }
}

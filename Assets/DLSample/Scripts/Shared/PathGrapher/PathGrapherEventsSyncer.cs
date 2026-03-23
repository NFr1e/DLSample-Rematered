using UnityEngine;
using DLSample.Gameplay;
using DLSample.Gameplay.Stream;
using DLSample.Gameplay.Behaviours;
using System.Collections.Generic;

namespace DLSample.Editor.PathGrapher
{
    public class PathGrapherEventsSyncer : GameplayObject
    {
        [SerializeField] private PathGrapherAsset pathGrapherAsset;

        private GameplayTimer _gameplayTimer;
        private readonly List<GameplayTimer.TickEvent> _tickEvents = new();

        protected override void OnStart()
        {
            _gameplayTimer = GameplayEntry.Instance.ServiceLocator.Get<GameplayTimer>();

            foreach (var evt in pathGrapherAsset.pathData.globalEvents)
            {
                _tickEvents.Add(new GameplayTimer.TickEvent(evt.GlobalTime, ResolveEvent(evt).Trigger));
            }
            foreach (var tEvt in _tickEvents)
            {
                _gameplayTimer.RegisterTickEvent(tEvt);
            }
        }
        protected override void OnExit()
        {
            foreach(var tEvt in _tickEvents)
            {
                _gameplayTimer?.UnregisterTickEvent(tEvt);
            }
        }

        private IGameplayEvent ResolveEvent(IPathEvent evt)
        {
            IGameplayEvent gameplayEvt;

            switch (evt)
            {
                case SpeedChangeEvent s:
                    var speedEvt = new PlayerEvents.SpeedChangeEvent
                    {
                        InvokeTime = s.GlobalTime,
                        Speed = s.newSpeed
                    };

                    gameplayEvt = speedEvt;
                    break;

                case GravityChangeEvent g:
                    var gravityEvt = new PlayerEvents.GravityChangeEvent
                    {
                        InvokeTime = g.GlobalTime,
                        Gravity = g.newGravity
                    };

                    gameplayEvt = gravityEvt;
                    break;

                case ForceTurnEvent t:
                    var turnEvent = new PlayerEvents.ForceTurnEvent
                    {
                        InvokeTime = t.GlobalTime,
                    };

                    gameplayEvt = turnEvent;
                    break;

                case DirectionChangeEvent d:
                    var directionEvt = new PlayerEvents.DirectionChangeEvent
                    {
                        InvokeTime = d.GlobalTime,
                        Directions = d.newDirections
                    };

                    gameplayEvt = directionEvt;
                    break;

                case TeleportEvent t:
                    var tpEvt = new PlayerEvents.TeleportEvent
                    {
                        InvokeTime = t.GlobalTime,
                        Position = t.targetPosition,
                    };

                    gameplayEvt = tpEvt;
                    break;

                case JumpEvent j:
                    var jpEvt = new PlayerEvents.JumpEvent
                    {
                        InvokeTime = j.GlobalTime,
                        Velocity = j.velocity
                    };
                    gameplayEvt = jpEvt;
                    break;
                default:
                    gameplayEvt = null;
                    break;
            }

            return gameplayEvt;
        }
    }
}

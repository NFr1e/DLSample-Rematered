using System;
using UnityEngine;
using DLSample.Shared;

namespace DLSample.Editor.PathGrapher
{
    public interface IPathEvent
    {
        double GlobalTime { get; set; }
    }

    [Serializable] public abstract class PointPathEvent : IPathEvent
    {
        [SerializeField] private double _globalTime;
        public double GlobalTime { get => _globalTime; set => _globalTime = value; }
    }

    [Serializable] public abstract class SegmentPathEvent : IPathEvent
    {
        [SerializeField] private double _startTime;
        [SerializeField] private double _endTime;

        public double GlobalTime { get => _startTime; set => _startTime = value; }
        public double StartTime { get => _startTime; set => _startTime = value; }
        public virtual double EndTime { get => _endTime; set => _endTime = value; }
    }

    #region PointEvents

    [Serializable]
    public class ForceTurnEvent : PointPathEvent
    {

    }

    [Serializable]
    public class SpeedChangeEvent : PointPathEvent
    {
        public float newSpeed;
    }

    [Serializable]
    public class GravityChangeEvent : PointPathEvent
    {
        public Vector3 newGravity;
    }

    [Serializable]
    public class DirectionChangeEvent : PointPathEvent
    {
        public PlayerDirections newDirections = new();
    }
    #endregion

    #region SegmentEvents
    [Serializable]
    public class TeleportEvent : SegmentPathEvent
    {
        public Vector3 targetPosition;

        public override double EndTime { get => base.StartTime + 0.0001f; }
    }

    [Serializable]
    public class JumpEvent : SegmentPathEvent
    {
        public Vector3 velocity;
    }
    #endregion
}
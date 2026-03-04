using DLSample.Shared;
using System;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public struct PlayerMovingArgs
    {
        public PlayerParams Params;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public bool IsGrounded;
        public bool IsMoving;
    }

    public interface IPlayerMove
    {
        void StartMove();
        void StopMove();
        void Inputed();

        bool IsMoving { get; }

        event Action<PlayerMovingArgs> OnStartMove;
        event Action<PlayerMovingArgs> OnStopMove;
        event Action<PlayerMovingArgs> OnMoving;
        event Action<PlayerMovingArgs> OnTurn;
        event Action<PlayerMovingArgs> OnLand;
    }
}

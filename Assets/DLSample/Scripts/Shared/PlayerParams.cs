using System;
using UnityEngine;

namespace DLSample.Shared
{
    [Serializable]
    public class PlayerParams
    {
        [SerializeField] private float moveSpeed = 12;
        [SerializeField] private bool forceGrounded = false;
        [SerializeField] private bool useGravity = true;
        [SerializeField] private Vector3 localGravity = new(0, -9.81f, 0);
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float checkGroundDist = 0.75f;
        [SerializeField] private PlayerDirections directions = new();

        public float MoveSpeed => moveSpeed;
        public bool ForceGrounded => forceGrounded;
        public bool UseGravity => useGravity;
        public Vector3 LocalGravity => localGravity;
        public LayerMask GroundLayer => groundLayer;
        public float CheckGroundDist => checkGroundDist;
        public PlayerDirections Directions => directions;

        public void SetSpeed(float speed)
        {
            moveSpeed = speed;
        }
        public void SetForceGrounded(bool force)
        {
            forceGrounded = force;
        }
        public void SetUseGravity(bool use)
        {
            useGravity = use;
        }
        public void SetLocalGravity(Vector3 gravity)
        {
            localGravity = gravity;
        }
        public void SetDirection(PlayerDirections direction)
        {
            directions = direction;
            directions.Reset();
        }
    }
}

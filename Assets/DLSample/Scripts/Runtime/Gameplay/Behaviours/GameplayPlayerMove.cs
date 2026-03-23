using System;
using UnityEngine;
using DLSample.Shared;

namespace DLSample.Gameplay.Behaviours
{
    public class GameplayPlayerMove : GameplayObject, IPlayerMove
    {
        [SerializeField] private PlayerParams playerParams;

        public event Action<PlayerMovingArgs> OnStartMove;
        public event Action<PlayerMovingArgs> OnStopMove;
        public event Action<PlayerMovingArgs> OnMoving;
        public event Action<PlayerMovingArgs> OnTurn;
        public event Action<PlayerMovingArgs> OnLand;

        private bool _isMoving = false;
        private bool _isColliding = false;
        private bool _isGrounded = false;
        private Vector3 _dropVelocity = Vector3.zero;

        private PlayerMovingArgs _movingArgs = new();

        #region Properties
        public bool IsMoving => _isMoving;
        public PlayerMovingArgs MovingArgs => _movingArgs;
        public PlayerParams PlayerParams => playerParams;
        #endregion

        private void Update()
        {
            if (_isMoving)
            {
                Move();
            }
            CheckGround();
        }
        private void FixedUpdate()
        {
            Drop();
        }
        private void OnCollisionEnter(Collision collision)
        {
            _isColliding = true;
        }
        private void OnCollisionStay(Collision collision)
        {
            _isColliding = true;
        }
        private void OnCollisionExit(Collision collision)
        {
            _isColliding = false;
        }

        public void Ready()
        {
            Freeze();
            transform.rotation = playerParams.Directions.StartRotation();
        }

        public void StartMove()
        {
            if (_isMoving) return;

            _isMoving = true;

            UpdateMovingCtx();
            OnStartMove?.Invoke(_movingArgs);
        }

        public void StopMove()
        {
            if (!_isMoving) return;

            _isMoving = false;

            UpdateMovingCtx();
            OnStopMove?.Invoke(_movingArgs);
        }

        public void Freeze()
        {
            _isMoving = false;
            _isGrounded = false;

            _dropVelocity = Vector3.zero;
        }

        private void Move()
        {
            transform.Translate(playerParams.MoveSpeed * Time.deltaTime * Vector3.forward, Space.Self);

            UpdateMovingCtx();
            OnMoving?.Invoke(_movingArgs);
        }

        private void Drop()
        {
            if (!IsMoving) return;

            if (!_isGrounded && playerParams.UseGravity)
            {
                _dropVelocity += playerParams.LocalGravity * Time.deltaTime;
            }

            transform.Translate(_dropVelocity * Time.deltaTime);
        }

        private void Land()
        {
            OnLand?.Invoke(_movingArgs);

            _dropVelocity = Vector3.zero;
            UpdateMovingCtx();
        }

        public void Turn()
        {
            transform.rotation = playerParams.Directions.MoveNext();

            UpdateMovingCtx();
            OnTurn?.Invoke(_movingArgs);
        }

        private void UpdateMovingCtx()
        {
            _movingArgs.Params = playerParams;
            _movingArgs.Position = transform.position;
            _movingArgs.Rotation = transform.rotation;
            _movingArgs.Velocity = _dropVelocity;
            _movingArgs.IsGrounded = _isGrounded;
            _movingArgs.IsMoving = _isMoving;
        }

        public void Inputed()
        {
            if (IsMoving && _isGrounded)
            {
                Turn();
            }
        }

        private void CheckGround()
        {
            bool wasGround = _isGrounded;

            _isGrounded = playerParams.ForceGrounded
                        || (_isColliding && Physics.Raycast(transform.position, -transform.up, playerParams.CheckGroundDist, playerParams.GroundLayer));

            if (!wasGround && _isGrounded) Land();
        }

        #region Setters
        public void SetVelocity(Vector3 velocity)
        {
            _dropVelocity = velocity;
        }
        public void SetGrounded(bool grounded)
        {
            _isGrounded = grounded;
        }
        #endregion
    }
}

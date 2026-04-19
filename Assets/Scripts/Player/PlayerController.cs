using System;
using System.Collections;
using TP.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TP.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private float _dashForce = 15f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldown = 1.5f;

        private Rigidbody _rb;
        private Animator _anim;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _dashAction;

        public Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _canDash = true;
        private bool _isDashing = false;
        private bool _isDashPressed = false;

        private StateMachine _stateMachine;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();

            _moveAction = _playerInput.actions["Move"];
            _lookAction = _playerInput.actions["Look"];
            _dashAction = _playerInput.actions["Dash"];

            // State machine setup
            _stateMachine = new StateMachine();

            var idleState = new IdleState(this, _anim);
            var locomotionState = new LocomotionState(this, _anim);
            var dashState = new DashState(this, _anim);

            At(idleState, locomotionState, new FuncPredicate(() => _moveInput.magnitude > 0.1f));
            At(locomotionState, idleState, new FuncPredicate(() => _rb.linearVelocity.magnitude < 0.1f && _moveInput.magnitude < 0.1f));

            At(idleState, dashState, new FuncPredicate(() => _isDashPressed));
            At(locomotionState, dashState, new FuncPredicate(() => _isDashPressed));

            At(dashState, locomotionState, new FuncPredicate(() => !_isDashing && _moveInput.magnitude > 0.1f));
            At(dashState, idleState, new FuncPredicate(() => !_isDashing && _moveInput.magnitude < 0.1f));


            _stateMachine.SetState(idleState);
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        private void Update()
        {
            _moveInput = _moveAction.ReadValue<Vector2>();
            _lookInput = _lookAction.ReadValue<Vector2>();

            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void HandleMove()
        {
            if (_isDashing) return;

            Vector3 moveDirection = (Vector3.back * _moveInput.y + Vector3.left * _moveInput.x);

            // Apply movement
            if (moveDirection.magnitude > 0.1f)
            {
                _rb.linearVelocity = new Vector3(moveDirection.x * _moveSpeed, _rb.linearVelocity.y, moveDirection.z * _moveSpeed);

                Quaternion targetRot = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.fixedDeltaTime);

                /*if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)
                {
                    transform.rotation = targetRot;
                }*/
            }
            else
            {
                // Slow to a stop if no input
                _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
            }
        }

        private void HandleLook()
        {
            // noop
        }

        private void HandleDash(InputAction.CallbackContext context)
        {
            if (!_canDash) return;

            _isDashPressed = true;
        }

        public IEnumerator DashCoroutine()
        {
            _isDashing = true;
            _isDashPressed = false;
            _canDash = false;

            float originalDrag = _rb.linearDamping;
            _rb.linearDamping = 0;

            Vector3 dashDir;
            if (_moveInput.sqrMagnitude > 0)
            {
                dashDir = (Vector3.back * _moveInput.y + Vector3.left * _moveInput.x).normalized;
            }
            else
            {
                dashDir = transform.forward;
            }

            _rb.linearVelocity = dashDir * _dashForce;

            yield return new WaitForSeconds(_dashDuration);

            _rb.linearVelocity = Vector3.zero;
            _isDashing = false;
            _rb.linearDamping = originalDrag;

            yield return new WaitForSeconds(_dashCooldown - _dashDuration);
            _canDash = true;
        }

        private void OnEnable()
        {
            _dashAction.performed += HandleDash;
        }
        private void OnDisable()
        {
            _dashAction.performed -= HandleDash;
        }

        public void TakeHit(HitData hit)
        {
            Debug.LogWarning("Player took hit");
        }
    }
}

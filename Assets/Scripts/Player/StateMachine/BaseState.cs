using TP.Player;
using UnityEngine;

namespace TP.Player
{
    public class BaseState : IState
    {
        protected readonly PlayerController _player;
        protected readonly Animator _animator;

        protected static readonly int _idleHash = Animator.StringToHash("Idle");
        protected static readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int _dashHash = Animator.StringToHash("Dash");

        protected const float _crossFadeDuration = 0.1f;

        protected BaseState(PlayerController player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnEnter()
        {
            // noop
        }

        public virtual void OnExit()
        {
            // noop
        }

        public virtual void Update()
        {
            // noop
        }
    }
}

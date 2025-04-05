using UnityEditor.Animations;
using UnityEngine;

namespace TP.Player
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            _animator.CrossFadeInFixedTime(_locomotionHash, _crossFadeDuration);
        }

        public override void Update()
        {
            _animator.SetFloat("LocoSpeed", _player._moveInput.magnitude);
        }

        public override void FixedUpdate()
        {
            _player.HandleMove();
            
        }
    }
}

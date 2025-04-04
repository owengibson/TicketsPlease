using UnityEngine;

namespace TP.Player
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            _animator.CrossFade(_locomotionHash, _crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            _player.HandleMove();
        }
    }
}

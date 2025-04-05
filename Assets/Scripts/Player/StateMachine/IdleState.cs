using UnityEngine;

namespace TP.Player
{
    public class IdleState : BaseState
    {
        public IdleState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            _animator.CrossFadeInFixedTime(_idleHash, _crossFadeDuration);
        }
    }
}

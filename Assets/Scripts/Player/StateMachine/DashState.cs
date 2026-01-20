using UnityEngine;

namespace TP.Player
{
    public class DashState : BaseState
    {
        public DashState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            _animator.CrossFadeInFixedTime(_locomotionHash, _crossFadeDuration);

            _player.StartCoroutine(_player.DashCoroutine());
        }
    }
}

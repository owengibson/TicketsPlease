using UnityEngine;

namespace TP.Player
{
    public class DashState : BaseState
    {
        public DashState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            _player.StartCoroutine(_player.DashCoroutine());
            //_animator.CrossFade(_dashHash, _crossFadeDuration);
        }
    }
}

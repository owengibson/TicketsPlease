using TP.Combat;
using TP.Combat.Weapons;
using UnityEngine;

namespace TP.Player
{
    public class CombatAnimatorBridge : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
        }

        public void PlayAction(CombatActionDefinition action)
        {
            if (_animator == null || action == null || string.IsNullOrWhiteSpace(action.AnimationTrigger))
                return;

            _animator.SetTrigger(action.AnimationTrigger);
        }

        public void ApplyWeaponAnimator(WeaponInstance weapon)
        {
            if (_animator == null)
                return;

            var overrideController = weapon?.Definition?.AnimatorOverrideController;
            if (overrideController != null)
                _animator.runtimeAnimatorController = overrideController;
        }

        public void PlayWeaponSwap()
        {
            if (_animator != null)
                _animator.SetTrigger("WeaponSwap");
        }
    }
}

using TP.Combat;
using TP.Combat.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TP.Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponLoadout _loadout;
        [SerializeField] private CombatActionExecutor _actionExecutor;
        [SerializeField] private CombatAnimatorBridge _animatorBridge;
        [SerializeField] private PlayerInput _playerInput;

        private InputAction _mainAttackAction;
        private InputAction _offHandAction;
        private InputAction _swapWeaponsAction;

        public CombatState CurrentState { get; private set; } = CombatState.Idle;
        public bool IsActionLocked { get; private set; }

        private void Awake()
        {
            if (_loadout == null)
                _loadout = GetComponent<PlayerWeaponLoadout>();

            if (_actionExecutor == null)
                _actionExecutor = GetComponent<CombatActionExecutor>();

            if (_animatorBridge == null)
                _animatorBridge = GetComponent<CombatAnimatorBridge>();

            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();

            if (_playerInput != null)
            {
                _mainAttackAction = _playerInput.actions.FindAction("MainAttack", false);
                _offHandAction = _playerInput.actions.FindAction("OffHand", false);
                _swapWeaponsAction = _playerInput.actions.FindAction("SwapWeapons", false);
            }
        }

        private void OnEnable()
        {
            if (_loadout != null)
                _loadout.EquippedWeaponChanged += HandleEquippedWeaponChanged;

            if (_mainAttackAction != null)
                _mainAttackAction.performed += HandleMainAttack;

            if (_offHandAction != null)
                _offHandAction.performed += HandleOffHand;

            if (_swapWeaponsAction != null)
                _swapWeaponsAction.performed += HandleSwapWeapons;
        }

        private void OnDisable()
        {
            if (_loadout != null)
                _loadout.EquippedWeaponChanged -= HandleEquippedWeaponChanged;

            if (_mainAttackAction != null)
                _mainAttackAction.performed -= HandleMainAttack;

            if (_offHandAction != null)
                _offHandAction.performed -= HandleOffHand;

            if (_swapWeaponsAction != null)
                _swapWeaponsAction.performed -= HandleSwapWeapons;
        }

        public bool TryUseMainAction()
        {
            if (_loadout == null)
                return false;

            WeaponInstance sourceWeapon = _loadout.EquippedWeapon;
            CombatActionDefinition action = sourceWeapon?.GetCurrentMainAction(Time.time);
            if (!CanStartAction(action, sourceWeapon, true))
                return false;

            if (!StartAction(action, sourceWeapon))
                return false;

            sourceWeapon.MarkMainActionUsed(action, Time.time);
            return true;
        }

        public bool TryUseOffHandAction()
        {
            if (_loadout == null)
                return false;

            WeaponInstance sourceWeapon = _loadout.InactiveWeapon;
            CombatActionDefinition action = sourceWeapon?.OffHandAction;
            if (!CanStartAction(action, sourceWeapon, false))
                return false;

            if (!StartAction(action, sourceWeapon))
                return false;

            sourceWeapon.MarkOffHandActionUsed(action, Time.time);
            return true;
        }

        public bool TrySwapWeapons()
        {
            if (_loadout == null || IsActionLocked || CurrentState == CombatState.Attacking || CurrentState == CombatState.Recovering)
                return false;

            if (!_loadout.SwapWeapons())
                return false;

            _animatorBridge?.PlayWeaponSwap();
            return true;
        }

        private bool CanStartAction(CombatActionDefinition action, WeaponInstance sourceWeapon, bool isMainAction)
        {
            if (CurrentState == CombatState.Disabled || sourceWeapon == null || action == null || _actionExecutor == null || _actionExecutor.IsExecuting)
                return false;

            if (IsActionLocked && !action.CanBeCancelled)
                return false;

            if (CurrentState == CombatState.Recovering && !action.CanInterruptRecovery)
                return false;

            return isMainAction
                ? sourceWeapon.CanUseMainAction(action)
                : sourceWeapon.CanUseOffHandAction(action);
        }

        private bool StartAction(CombatActionDefinition action, WeaponInstance sourceWeapon)
        {
            if (!_actionExecutor.ExecuteAction(gameObject, sourceWeapon, action, OnActionFinished))
                return false;

            OnActionStarted(action, sourceWeapon);
            return true;
        }

        private void OnActionStarted(CombatActionDefinition action, WeaponInstance sourceWeapon)
        {
            CurrentState = CombatState.Attacking;
            IsActionLocked = action.LockActions;
            _animatorBridge?.PlayAction(action);
        }

        private void OnActionFinished(CombatActionDefinition action, WeaponInstance sourceWeapon)
        {
            CurrentState = CombatState.Idle;
            IsActionLocked = false;
        }

        private void HandleEquippedWeaponChanged(WeaponInstance weapon)
        {
            _animatorBridge?.ApplyWeaponAnimator(weapon);
        }

        private void HandleMainAttack(InputAction.CallbackContext context) => TryUseMainAction();

        private void HandleOffHand(InputAction.CallbackContext context) => TryUseOffHandAction();

        private void HandleSwapWeapons(InputAction.CallbackContext context) => TrySwapWeapons();
    }
}

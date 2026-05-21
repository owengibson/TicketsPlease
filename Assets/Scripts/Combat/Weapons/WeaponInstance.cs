using TP.Combat;
using UnityEngine;

namespace TP.Combat.Weapons
{
    [System.Serializable]
    public class WeaponInstance
    {
        public WeaponDefinition Definition;
        public WeaponRuntimeState RuntimeState = new WeaponRuntimeState();

        public bool HasDefinition => Definition != null;
        public MainActionSet MainActionSet => Definition != null ? Definition.MainActionSet : null;
        public CombatActionDefinition OffHandAction => Definition != null ? Definition.OffHandAction : null;

        public void EnsureRuntimeState()
        {
            RuntimeState ??= new WeaponRuntimeState();
        }

        public void Tick(float deltaTime)
        {
            EnsureRuntimeState();
            RuntimeState.Tick(deltaTime);
        }

        public CombatActionDefinition GetCurrentMainAction(float currentTime)
        {
            EnsureRuntimeState();
            var set = MainActionSet;
            if (set == null)
                return null;

            if (set.ComboResetTime > 0f && currentTime - RuntimeState.LastMainActionTime > set.ComboResetTime)
                RuntimeState.CurrentComboIndex = 0;

            return set.GetAction(RuntimeState.CurrentComboIndex);
        }

        public bool CanUseMainAction(CombatActionDefinition action)
        {
            EnsureRuntimeState();
            return HasDefinition
                   && action != null
                   && !RuntimeState.IsMainActionOnCooldown()
                   && RuntimeState.HasCharges(action.ChargeCost)
                   && RuntimeState.HasAmmo(action.AmmoCost);
        }

        public bool CanUseOffHandAction(CombatActionDefinition action)
        {
            EnsureRuntimeState();
            return HasDefinition
                   && action != null
                   && !RuntimeState.IsOffHandOnCooldown()
                   && RuntimeState.HasCharges(action.ChargeCost)
                   && RuntimeState.HasAmmo(action.AmmoCost);
        }

        public void MarkMainActionUsed(CombatActionDefinition action, float currentTime)
        {
            EnsureRuntimeState();
            if (action == null)
                return;

            RuntimeState.MainActionCooldownRemaining = action.CooldownSeconds;
            RuntimeState.SpendResources(action.ChargeCost, action.AmmoCost);
            RuntimeState.LastMainActionTime = currentTime;

            int actionCount = MainActionSet != null ? MainActionSet.Count : 0;
            RuntimeState.CurrentComboIndex = actionCount > 0
                ? (RuntimeState.CurrentComboIndex + 1) % actionCount
                : 0;
        }

        public void MarkOffHandActionUsed(CombatActionDefinition action, float currentTime)
        {
            EnsureRuntimeState();
            if (action == null)
                return;

            RuntimeState.OffHandCooldownRemaining = action.CooldownSeconds;
            RuntimeState.SpendResources(action.ChargeCost, action.AmmoCost);
            RuntimeState.LastOffHandActionTime = currentTime;
        }
    }
}

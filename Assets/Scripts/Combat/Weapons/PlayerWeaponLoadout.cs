using System;
using UnityEngine;

namespace TP.Combat.Weapons
{
    public class PlayerWeaponLoadout : MonoBehaviour
    {
        public WeaponInstance SlotA = new WeaponInstance();
        public WeaponInstance SlotB = new WeaponInstance();
        public WeaponSlot EquippedSlot = WeaponSlot.SlotA;

        public event Action<WeaponInstance> EquippedWeaponChanged;

        public WeaponInstance EquippedWeapon => GetWeaponInSlot(EquippedSlot);

        public WeaponInstance InactiveWeapon => GetWeaponInSlot(EquippedSlot == WeaponSlot.SlotA ? WeaponSlot.SlotB : WeaponSlot.SlotA);

        private void Awake()
        {
            SlotA?.EnsureRuntimeState();
            SlotB?.EnsureRuntimeState();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        public void Tick(float deltaTime)
        {
            SlotA?.Tick(deltaTime);
            SlotB?.Tick(deltaTime);
        }

        public WeaponInstance GetWeaponInSlot(WeaponSlot slot)
        {
            return slot == WeaponSlot.SlotA ? SlotA : SlotB;
        }

        public WeaponInstance GetEquippedWeapon() => EquippedWeapon;

        public WeaponInstance GetInactiveWeapon() => InactiveWeapon;

        public bool HasWeaponInSlot(WeaponSlot slot)
        {
            return GetWeaponInSlot(slot)?.Definition != null;
        }

        public bool CanSwap()
        {
            return HasWeaponInSlot(WeaponSlot.SlotA) && HasWeaponInSlot(WeaponSlot.SlotB);
        }

        public bool SwapWeapons()
        {
            if (!CanSwap())
                return false;

            EquippedSlot = EquippedSlot == WeaponSlot.SlotA ? WeaponSlot.SlotB : WeaponSlot.SlotA;
            EquippedWeaponChanged?.Invoke(EquippedWeapon);
            return true;
        }

        public bool TryValidateLoadout(out string reason)
        {
            if (SlotA?.Definition == null)
            {
                reason = "SlotA has no weapon definition.";
                return false;
            }

            if (SlotB?.Definition == null)
            {
                reason = "SlotB has no weapon definition.";
                return false;
            }

            reason = null;
            return true;
        }
    }
}

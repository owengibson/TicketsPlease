namespace TP.Combat.Weapons
{
    [System.Serializable]
    public class WeaponRuntimeState
    {
        public float MainActionCooldownRemaining;
        public float OffHandCooldownRemaining;
        public int CurrentCharges;
        public int CurrentAmmo;
        public int CurrentComboIndex;
        public float LastMainActionTime = float.NegativeInfinity;
        public float LastOffHandActionTime = float.NegativeInfinity;

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f)
                return;

            MainActionCooldownRemaining = System.Math.Max(0f, MainActionCooldownRemaining - deltaTime);
            OffHandCooldownRemaining = System.Math.Max(0f, OffHandCooldownRemaining - deltaTime);
        }

        public bool IsMainActionOnCooldown() => MainActionCooldownRemaining > 0f;

        public bool IsOffHandOnCooldown() => OffHandCooldownRemaining > 0f;

        public bool HasCharges(int chargeCost) => chargeCost <= 0 || CurrentCharges >= chargeCost;

        public bool HasAmmo(int ammoCost) => ammoCost <= 0 || CurrentAmmo >= ammoCost;

        public void SpendResources(int chargeCost, int ammoCost)
        {
            if (chargeCost > 0)
                CurrentCharges = System.Math.Max(0, CurrentCharges - chargeCost);

            if (ammoCost > 0)
                CurrentAmmo = System.Math.Max(0, CurrentAmmo - ammoCost);
        }

        public void ResetCombo()
        {
            CurrentComboIndex = 0;
            LastMainActionTime = float.NegativeInfinity;
        }
    }
}

using Sirenix.OdinInspector;
using TP.Combat;
using UnityEngine;

namespace TP.Combat.Weapons
{
    [CreateAssetMenu(menuName = "Combat/Weapon Definition")]
    public class WeaponDefinition : SerializedScriptableObject
    {
        public string WeaponId;
        public string DisplayName;
        public Sprite Icon;
        public GameObject EquippedVisualPrefab;

        public MainActionSet MainActionSet;
        public CombatActionDefinition OffHandAction;

        public float BaseDamageMultiplier = 1f;
        public float AttackSpeedMultiplier = 1f;

        public RuntimeAnimatorController AnimatorOverrideController;
    }
}

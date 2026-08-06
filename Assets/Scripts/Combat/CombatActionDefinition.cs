using Sirenix.OdinInspector;
using UnityEngine;

namespace TP.Combat
{
    /// <summary>
    /// Static design-time description of a combat action.
    /// E.g. punch, sword slash, fireball, shield bash, etc.
    /// </summary>
    [CreateAssetMenu(menuName = "Combat/Combat Action Definition")]
    public class CombatActionDefinition : SerializedScriptableObject
    {
        public string ActionId;
        public string DisplayName;
        //public CombatActionType ActionType;

        public string AnimationTrigger;
        [Min(0f)]
        public float StartupDuration;
        [Min(0f)]
        public float ActiveDuration;
        [Min(0f)]
        public float RecoveryDuration;

        public float BaseDamage;
        public float KnockbackForce;

        [Tooltip("Preferred runtime hitbox. Resolve this against child HitboxAuthoring components on the attacker.")]
        public string HitboxId;
        public HitShapeType FallbackHitShape = HitShapeType.Box;
        public Vector3 FallbackHitOffset = Vector3.forward;
        public Vector3 FallbackHitBoxSize = Vector3.one;
        public float FallbackHitRadius = 0.5f;
        public float FallbackHitCapsuleHeight = 1f;
        public LayerMask TargetLayers = ~0;
        public QueryTriggerInteraction QueryTriggerInteraction = QueryTriggerInteraction.Ignore;

        [Min(0f)]
        public float CooldownSeconds;
        [Min(0)]
        public int ChargeCost;
        [Min(0)]
        public int AmmoCost;

        public bool LockMovement = true;
        public bool LockActions = true;
        public bool CanRotateDuringAction;
        public bool CanBeCancelled;
        public bool CanStartInAir;
        public bool CanInterruptRecovery;
        public bool AllowMultipleHits;

        public GameObject HitVfxPrefab;
        public AudioClip UseSfx;
        public AudioClip HitSfx;
    }
}

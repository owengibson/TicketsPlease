using Sirenix.OdinInspector;
using UnityEngine;

namespace TP
{
    [CreateAssetMenu(menuName = "Combat/Combat Action Definition")]
    public class CombatActionDefinition : SerializedScriptableObject
    {
        public string ActionId;
        public string DisplayName;
        //public CombatActionType ActionType;

        public string AnimationTrigger;
        public float StartupDuration;
        public float ActiveDuration;
        public float RecoveryDuration;

        public float BaseDamage;
        public float KnockbackForce;


    }
}

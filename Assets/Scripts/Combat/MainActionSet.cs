using Sirenix.OdinInspector;
using UnityEngine;

namespace TP.Combat
{
    [CreateAssetMenu(menuName = "Combat/Main Action Set")]
    public class MainActionSet : SerializedScriptableObject
    {
        public string SetId;
        public CombatActionDefinition[] ComboActions = System.Array.Empty<CombatActionDefinition>();
        [Min(0f)]
        public float ComboResetTime = 0.8f;

        public int Count => ComboActions?.Length ?? 0;

        public CombatActionDefinition GetAction(int comboIndex)
        {
            if (ComboActions == null || ComboActions.Length == 0)
                return null;

            int safeIndex = Mathf.Clamp(comboIndex, 0, ComboActions.Length - 1);
            return ComboActions[safeIndex];
        }
    }
}

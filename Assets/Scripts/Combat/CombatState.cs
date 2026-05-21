using UnityEngine;

namespace TP.Combat
{
    public enum CombatState
    {
        Idle = 0,
        Attacking = 1,
        Recovering = 2,
        Blocking = 3,
        Staggered = 4,
        Disabled = 5
    }
}

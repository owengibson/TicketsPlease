using UnityEngine;
using TP.Combat.Weapons;

namespace TP.Combat
{
    public struct HitData
    {
        public GameObject Attacker;
        public GameObject Target;
        public float Damage;
        public Vector3 Origin;
        public Vector3 HitPoint;
        public Vector3 Direction;
        public float KnockbackForce;
        public CombatActionDefinition SourceAction;
        public WeaponDefinition SourceWeapon;
    }
}

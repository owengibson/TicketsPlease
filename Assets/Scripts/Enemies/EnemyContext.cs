using TP.Common;
using TP.Enemies;
using UnityEngine;

namespace TP
{
    [RequireComponent(typeof(EnemyMoveAction))]
    [RequireComponent(typeof(MeleeAttackAction))]
    [RequireComponent(typeof(HealthComponent))]
    public class EnemyContext : MonoBehaviour
    {
        public PlayerSensor PlayerSensor { get; private set; }
        public EnemyMoveAction Move { get; private set; }
        public MeleeAttackAction Attack { get; private set; }
        public HealthComponent Health { get; private set; }

        void Awake()
        {
            PlayerSensor = GetComponentInChildren<PlayerSensor>();
            Move = GetComponent<EnemyMoveAction>();
            Attack = GetComponent<MeleeAttackAction>();
            Health = GetComponent<HealthComponent>();
        }
    }
}

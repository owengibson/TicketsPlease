using TP.Common;
using TP.Enemies;
using UnityEngine;

namespace TP
{
    public class EnemyContext : MonoBehaviour
    {
        public Transform transformRef;
        public PlayerSensor playerSensor;
        public EnemyMoveAction move;
        public MeleeAttackAction attack;
        public HealthComponent health;

        void Awake()
        {
            transformRef = transform;
            playerSensor = GetComponentInChildren<PlayerSensor>();
            move = GetComponent<EnemyMoveAction>();
            attack = GetComponent<MeleeAttackAction>();
            health = GetComponent<HealthComponent>();
        }
    }
}

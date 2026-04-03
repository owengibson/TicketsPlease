using TP.Common;
using TP.Enemies;
using UnityEngine;

namespace TP
{
    public class EnemyContext : MonoBehaviour
    {
        public Transform transformRef;
        public PlayerSensor playerSensor;
        public AStarMoveAction move;
        public MeleeAttackAction attack;
        public HealthComponent health;

        void Awake()
        {
            transformRef = transform;
            playerSensor = GetComponentInChildren<PlayerSensor>();
            move = GetComponent<AStarMoveAction>();
            attack = GetComponent<MeleeAttackAction>();
            health = GetComponent<HealthComponent>();
        }
    }
}

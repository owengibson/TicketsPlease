using TP.Common;
using UnityEngine;

namespace TP.Enemies
{
    public class MeleeEnemyBrain: MonoBehaviour
    {
        [SerializeField] private PlayerSensor playerSensor;
        [SerializeField] private AStarMoveAction move;

        void Update()
        {
            if (playerSensor.target == null)
                return;

            move.SetDestination(playerSensor.target.position);
        }
    }
}

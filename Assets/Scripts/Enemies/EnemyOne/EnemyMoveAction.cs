using UnityEngine;
using Pathfinding;


namespace TP.Enemies
{
    public class EnemyMoveAction : MonoBehaviour
    {
        AIPath aStarPath;

        void Awake()
        {
            aStarPath = GetComponent<AIPath>();
        }

        public void SetDestination(Vector3 pos)
        {
            aStarPath.destination = pos;
        }
    }
}

using UnityEngine;
using Pathfinding;


namespace TP
{
    public class AStarMoveAction : MonoBehaviour
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

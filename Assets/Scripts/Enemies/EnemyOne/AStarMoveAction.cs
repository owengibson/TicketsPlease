using UnityEngine;
using Pathfinding;


namespace TP
{
    public class AStarMoveAction : MonoBehaviour
    {
        AIPath ai;

        void Awake()
        {
            ai = GetComponent<AIPath>();
        }

        public void SetDestination(Vector3 pos)
        {
            ai.destination = pos;
        }
    }
}

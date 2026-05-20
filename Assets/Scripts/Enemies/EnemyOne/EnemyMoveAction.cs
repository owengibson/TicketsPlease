using UnityEngine;
using Pathfinding;


namespace TP.Enemies
{
    public class EnemyMoveAction : MonoBehaviour
    {
        AIPath aStarPath;

        private void Awake()
        {
            aStarPath = GetComponent<AIPath>();
        }

        private void SetDestination(Vector3 pos)
        {
            aStarPath.destination = pos;
        }

        public void Chase(Transform target)
        {
            Resume();
            SetDestination(target.position);
        }

        public void Stop()
        {
            aStarPath.canMove = false;
        }

        public void Resume()
        {
            aStarPath.canMove = true;
        }

        public void SetSpeed(float moveSpeed)
        {
            aStarPath.maxSpeed = moveSpeed;
        }
    }
}
using UnityEngine;
using Pathfinding;

namespace TP.Enemies
{
    public class AstarEnemyAI : MonoBehaviour
    {
        public Transform targetPosition;

        private Seeker _seeker;
        private CharacterController _characterController;

        public Path path;

        public float speed = 2;

        public float nextWaypointDistance = 3;

        private int _currentWaypoint = 0;

        public bool reachedEndOfPath;

        void Start()
        {
            _seeker = GetComponent<Seeker>();
            _characterController = GetComponent<CharacterController>();

            _seeker.pathCallback += OnPathComplete;

            _seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        }

        public void OnDisable()
        {
            _seeker.pathCallback -= OnPathComplete;
        }

        public void OnPathComplete(Path p)
        {
            Debug.Log("Path Error: " + p.error);

            if (!p.error)
            {
                path = p;
                _currentWaypoint = 0;
            }
        }

        private void Update()
        {
            if (path == null) 
                return;

            reachedEndOfPath = false;

            float distanceToWaypoint;

            while (true) 
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[_currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    if (_currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        _currentWaypoint++;
                    }
                    else
                    {
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else break;
            }

            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

            Vector3 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;

            Vector3 velocity = dir * speed * speedFactor;

            _characterController.SimpleMove(velocity);
        }
    }
}

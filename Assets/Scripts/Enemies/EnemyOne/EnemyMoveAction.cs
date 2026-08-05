using UnityEngine;
using Pathfinding;


namespace TP.Enemies
{
    [RequireComponent(typeof(AIPath))]
    public class EnemyMoveAction : MonoBehaviour
    {
        public bool IsMoving => !_path.isStopped && _path.velocity.sqrMagnitude > _movingSpeedThreshold * _movingSpeedThreshold;
        public bool HasReachedDestination => _hasDestination && !_path.pathPending && _path.reachedDestination;
        public Vector3 Velocity => _path.velocity;

        [SerializeField] private float _minimumDestinationChange = 0.25f;
        [SerializeField] private float _movingSpeedThreshold = 0.05f;

        private AIPath _path;
        private Vector3 _lastDestination;
        private bool _hasDestination;

        private void Awake()
        {
            _path = GetComponent<AIPath>();
        }

        public void MoveTo(Vector3 destination)
        {
            _path.isStopped = false;

            float minimumChangeSquared = _minimumDestinationChange * _minimumDestinationChange;

            bool destinationChanged = !_hasDestination || (destination - _lastDestination).sqrMagnitude >= minimumChangeSquared;

            if (!destinationChanged)
                return;

            bool isFirstDestination = !_hasDestination;

            _hasDestination = true;
            _lastDestination = destination;
            _path.destination = destination;

            if (isFirstDestination && !_path.pathPending)
                _path.SearchPath();
        }

        public void Stop()
        {
            _path.isStopped = true;
        }

        public void Resume()
        {
            if (_hasDestination)
                _path.isStopped = false;
        }

        public void SetSpeed(float speed)
        {
            _path.maxSpeed = Mathf.Max(0f, speed);
        }

        public void Enable()
        {
            _path.canMove = true;
        }

        public void Disable()
        {
            _path.isStopped = true;
            _path.canMove = false;
        }
    }
}
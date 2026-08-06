using System;
using UnityEngine;

namespace TP.Common
{
    public class PlayerSensor : MonoBehaviour
    {
        public event Action<Transform> OnTargetAcquired;
        public event Action<Transform> OnTargetLost;

        public Transform Target { get; private set; }
        public bool IsAggroed => Target != null;

        [SerializeField] private float _aggroRadius = 80f;
        [SerializeField] private LayerMask _playerLayers;
        [SerializeField] private float _scanInterval = 0.1f;
        [SerializeField] private int _maximumHits = 16;

        private Collider[] _hitBuffer;
        private float _nextScanTime;

        private void Awake()
        {
            _hitBuffer = new Collider[_maximumHits];
        }

        private void OnEnable()
        {
            _nextScanTime = Time.time + UnityEngine.Random.Range(0, _scanInterval);
        }

        private void Update()
        {
            if (Time.time < _nextScanTime)
                return;

            _nextScanTime = Time.time + _scanInterval;
            Scan();
        }

        private void Scan()
        {
            if (Target != null)
                return;

            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _aggroRadius,
                _hitBuffer,
                _playerLayers,
                QueryTriggerInteraction.Ignore
            );

            Transform closestTarget = null;
            float closestDistanceSquared = float.PositiveInfinity;

            for (int i = 0; i < hitCount; i++)
            {
                Collider hit = _hitBuffer[i];

                if (hit == null)
                    continue;

                Transform candidate = GetTargetTransform(hit);

                float distanceSquared =
                    (candidate.position - transform.position).sqrMagnitude;

                if (distanceSquared >= closestDistanceSquared)
                    continue;

                closestDistanceSquared = distanceSquared;
                closestTarget = candidate;
            }

            if (closestTarget != null)
                SetTarget(closestTarget);
        }

        private static Transform GetTargetTransform(Collider hit)
        {
            return hit.attachedRigidbody != null ? hit.attachedRigidbody.transform : hit.transform.root;
        }

        private void SetTarget(Transform newTarget)
        {
            if (Target == newTarget)
                return;

            Transform previousTarget = Target;
            Target = newTarget;

            if (previousTarget != null)
                OnTargetLost?.Invoke(previousTarget);

            if (Target != null)
                OnTargetAcquired?.Invoke(Target);
        }

        public void ClearTarget()
        {
            SetTarget(null);
        }

        private void OnDisable()
        {
            SetTarget(null);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsAggroed ? Color.green : Color.red;

            Gizmos.DrawWireSphere(transform.position, _aggroRadius);
        }
    }
}

using UnityEngine;

namespace TP.Common
{
    public class PlayerSensor : MonoBehaviour
    {
        [SerializeField] private float _radius = 80f;

        [SerializeField] public LayerMask playerLayer;

        public Transform Target {  get; private set; }

        private void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, playerLayer);
            if (hits.Length == 0)
            {
                Target = null;
                return;
            }

            Transform closestTarget = hits[0].transform;
            float smallestDist = Vector3.Distance(closestTarget.position, transform.position);

            for (int i = 1; i < hits.Length; i++)
            {
                float dist = Vector3.Distance(hits[i].transform.position, transform.position);
                if (dist < smallestDist)
                {
                    smallestDist = dist;
                    closestTarget = hits[i].transform;
                }
            }
            Target = closestTarget;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
  
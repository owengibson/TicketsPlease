using UnityEngine;

namespace TP.Common
{
    public class PlayerSensor : MonoBehaviour
    {
        private float _radius = 80f;

        public LayerMask playerLayer;

        public Transform Target {  get; private set; }

        private void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, playerLayer);
            Target = hits.Length > 0 ? hits[0].transform : null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
  
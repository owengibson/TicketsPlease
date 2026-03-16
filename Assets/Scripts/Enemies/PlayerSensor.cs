using UnityEngine;

namespace TP.Common
{
    public class PlayerSensor : MonoBehaviour
    {
        private float _radius = 8.0f;

        public LayerMask playerLayer;

        public Transform target {  get; private set; }

        private void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, playerLayer);
            target = hits.Length > 0 ? hits[0].transform : null;
        }

    }
}

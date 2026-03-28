using UnityEngine;

namespace TP.Common
{
    public class PlayerSensor : MonoBehaviour
    {
        private float _radius = 8.0f;

        public LayerMask playerLayer;

        public Transform Target {  get; private set; }

        private void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, playerLayer);
            Target = hits.Length > 0 ? hits[0].transform : null;
        }

    }
}
  
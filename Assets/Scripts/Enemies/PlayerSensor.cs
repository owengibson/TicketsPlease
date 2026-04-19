using UnityEngine;

namespace TP.Common
{
    public class PlayerSensor : MonoBehaviour
    {
        [SerializeField] private float _radius = 80.0f;
        [SerializeField] private LayerMask _playerLayer;

        public Transform Target {  get; private set; }

        private void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _playerLayer);
            Target = hits.Length > 0 ? hits[0].transform : null;
        }

    }
}
  
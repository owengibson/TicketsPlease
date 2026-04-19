using TP.Common;
using UnityEngine;

namespace TP
{
    public class Hitbox : MonoBehaviour
    {
        public HitData HitData;
        private bool _active;

        public void Activate() => _active = true;
        public void Deactivate() => _active = false;

        void OnTriggerEnter(Collider other)
        {
            if (!_active) return;

            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeHit(HitData);
            }
        }
    }
}

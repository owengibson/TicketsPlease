using TP.Combat;
using UnityEngine;

namespace TP
{
    public class Hitbox : MonoBehaviour
    {
        public HitData hitData;
        bool active;

        public void Activate() => active = true;
        public void Deactivate() => active = false;

        void OnTriggerEnter(Collider other)
        {
            if (!active) return;

            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeHit(hitData);
            }
        }
    }
}

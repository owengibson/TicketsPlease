using UnityEngine;

namespace TP.Combat
{
    [DisallowMultipleComponent]
    public class HitboxAuthoring : MonoBehaviour
    {
        public string HitboxId;
        public Collider HitCollider;
        public Color DebugColor = new Color(1f, 0.2f, 0.05f, 0.35f);

        private void Reset()
        {
            HitCollider = GetComponent<Collider>();
            if (string.IsNullOrWhiteSpace(HitboxId))
                HitboxId = gameObject.name;
        }

        private void OnValidate()
        {
            if (HitCollider == null)
                HitCollider = GetComponent<Collider>();
        }

        private void OnDrawGizmosSelected()
        {
            if (HitCollider == null)
                return;

            Gizmos.color = DebugColor;
            Gizmos.matrix = HitCollider.transform.localToWorldMatrix;

            if (HitCollider is BoxCollider box)
                Gizmos.DrawWireCube(box.center, box.size);
            else if (HitCollider is SphereCollider sphere)
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            else if (HitCollider is CapsuleCollider capsule)
                Gizmos.DrawWireSphere(capsule.center, capsule.radius);
        }
    }
}

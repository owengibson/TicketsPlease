using System.Collections.Generic;
using TP.Combat;
using UnityEngine;

namespace TP.Combat
{
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private Collider hitboxCollider;
        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private GameObject owner;

        private HitData hitData;
        private readonly HashSet<IDamageable> alreadyHit = new();

        private void Awake()
        {
            if (hitboxCollider == null)
                hitboxCollider = GetComponent<Collider>();

            hitboxCollider.isTrigger = true;
            hitboxCollider.enabled = false;
        }

        public void Activate(HitData hit)
        {
            hitData = hit;
            alreadyHit.Clear();
            hitboxCollider.enabled = true;
        }

        public void Deactivate()
        {
            hitboxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayers) == 0)
                return;

            if (owner != null && other.transform.root.gameObject == owner)
                return;

            var damageable = other.GetComponentInParent<IDamageable>();
            if (damageable == null)
                return;

            if (alreadyHit.Contains(damageable))
                return;

            alreadyHit.Add(damageable);

            damageable.TakeHit(hitData);
        }
    }
}
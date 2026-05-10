using TP.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace TP.Common
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        public float maxHealth;
        private float _currentHealth;

        public bool isAlive;

        public UnityEvent onDeath;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            onDeath?.Invoke();
        }

        public void TakeHit(HitData hit)
        {
            TakeDamage(hit.Damage);
        }
    }
}

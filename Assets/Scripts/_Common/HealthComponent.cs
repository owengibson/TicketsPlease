using TP.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace TP.Common
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        public float maxHealth;
        private float _currentHealth;

        public bool isAlive = true;

        public UnityEvent onDeath;

        private void Awake()
        {
            _currentHealth = maxHealth;
            isAlive = _currentHealth > 0;
        }

        public void TakeHit(HitData hit)
        {
            ApplyDamage(Mathf.CeilToInt(hit.Damage));
        }

        public void TakeDamage(int damage)
        {
            ApplyDamage(damage);
        }

        public void ApplyDamage(int damage)
        {
            if (!isAlive || damage <= 0)
                return;

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
        }

        public bool IsDead() => !isAlive;

        private void Die()
        {
            if (!isAlive)
                return;

            isAlive = false;
            onDeath?.Invoke();
            isAlive = false;
        }
    }
}

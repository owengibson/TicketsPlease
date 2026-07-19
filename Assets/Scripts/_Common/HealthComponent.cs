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

        public UnityEvent onDamaged;
        public UnityEvent onDeath;

        protected virtual void Awake()
        {
            _currentHealth = maxHealth;
            isAlive = _currentHealth > 0;
        }

        public virtual void TakeHit(HitData hit)
        {
            ApplyDamage(Mathf.CeilToInt(hit.Damage));
        }

        public virtual void TakeDamage(int damage)
        {
            ApplyDamage(damage);
        }

        public void ApplyDamage(int damage)
        {
            TryApplyDamage(damage);
        }

        protected bool TryApplyDamage(int damage)
        {
            if (!isAlive || damage <= 0)
                return false;

            _currentHealth -= damage;
            onDamaged?.Invoke();

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }

            return true;
        }

        public bool IsDead() => !isAlive;

        private void Die()
        {
            if (!isAlive)
                return;

            isAlive = false;
            onDeath?.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace TP.Common
{
    public class HealthComponent : MonoBehaviour
    {
        public int maxHealth;
        private int _currentHealth;

        public bool isAlive;

        public UnityEvent onDeath;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
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
    }
}

using UnityEngine;

namespace TP.Common
{
    public class HealthComponent : MonoBehaviour
    {
        public int health;
        public bool isAlive;

        [SerializeField] int _maxHealth;

        private void Start()
        {
            health = _maxHealth;
        }

        public void OnDamageDealt(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            isAlive = false;
        }
    }
}

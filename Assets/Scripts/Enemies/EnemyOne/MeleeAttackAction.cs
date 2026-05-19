using System.Collections;
using UnityEngine;
using TP.Combat;

namespace TP.Enemies
{
    public class MeleeAttackAction : MonoBehaviour
    {
        EnemyContext ctx;

        public Hitbox hitbox;

        public float windup = 0.4f;
        public float activeTime = 0.2f;
        public float recovery = 0.5f;

        [SerializeField] private float damage = 10;

        bool isAttacking;

        void Awake()
        {
            ctx = GetComponent<EnemyContext>();
        }

        public void TryAttack()
        {
            if (!isAttacking)
                StartCoroutine(AttackRoutine());
        }

        IEnumerator AttackRoutine()
        {
            isAttacking = true;

            // WINDUP
            Debug.Log("windup");
            yield return new WaitForSeconds(windup);

            HitData hitData = new HitData
            {
                Attacker = gameObject,
                Damage = damage
            };

            // ACTIVE
            Debug.Log("active");
            hitbox.Activate(hitData);
            yield return new WaitForSeconds(activeTime);
            hitbox.Deactivate();

            // RECOVERY
            Debug.Log("recovery");
            yield return new WaitForSeconds(recovery);

            isAttacking = false;
        }
    }
}

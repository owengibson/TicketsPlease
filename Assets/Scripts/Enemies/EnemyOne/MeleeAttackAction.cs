using System.Collections;
using UnityEngine;

namespace TP.Enemies
{
    public class MeleeAttackAction : MonoBehaviour
    {
        EnemyContext ctx;

        public Hitbox hitbox;

        public float windup = 0.4f;
        public float activeTime = 0.2f;
        public float recovery = 0.5f;

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
            yield return new WaitForSeconds(windup);

            // ACTIVE
            hitbox.Activate();
            yield return new WaitForSeconds(activeTime);
            hitbox.Deactivate();

            // RECOVERY
            yield return new WaitForSeconds(recovery);

            isAttacking = false;
        }
    }
}

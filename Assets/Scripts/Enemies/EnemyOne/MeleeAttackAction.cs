using System.Collections;
using TP.Combat;
using UnityEngine;

namespace TP.Enemies
{
    public class MeleeAttackAction : MonoBehaviour
    {
        EnemyContext ctx;

        public Hitbox hitbox;
        public CombatActionExecutor actionExecutor;
        public CombatActionDefinition combatAction;

        public float windup = 0.4f;
        public float activeTime = 0.2f;
        public float recovery = 0.5f;

        [SerializeField] private float damage = 10;

        bool isAttacking;
        Coroutine attackRoutine;

        void Awake()
        {
            ctx = GetComponent<EnemyContext>();
            if (actionExecutor == null)
                actionExecutor = GetComponent<CombatActionExecutor>();
        }

        public void TryAttack()
        {
            if (!isAttacking)
                attackRoutine = StartCoroutine(AttackRoutine());
        }

        IEnumerator AttackRoutine()
        {
            isAttacking = true;

            if (actionExecutor != null && combatAction != null)
            {
                actionExecutor.ExecuteAction(gameObject, combatAction, (_, _) => isAttacking = false);
                yield break;
            }

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
            attackRoutine = null;
        }

        public void StopAttack()
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }

            if (hitbox != null)
                hitbox.Deactivate();

            isAttacking = false;
        }
    }
}

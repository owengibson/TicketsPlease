using TP.Common;
using UnityEngine;

namespace TP.Enemies
{
    public class MeleeEnemyBrain : MonoBehaviour
    {
        private EnemyContext ctx;

        public float attackRange = 3.0f;

        public enum EnemyState {Idle, Chase, Attack, Dead}
        public EnemyState state;

        void Awake()
        {
            ctx = GetComponent<EnemyContext>();
        }

        void Update()
        {
            var target = ctx.playerSensor.Target;
            float distToTarget = 0;

            if (target != null)
            {
                distToTarget = Vector3.Distance(ctx.transformRef.position, target.position);
            }
            else
            {
                state = EnemyState.Idle;
            }

            if (ctx.health.isAlive == false)
            {
                state = EnemyState.Dead;
            }

            switch (state)
            {
                case EnemyState.Idle:
                    ctx.move.Stop();

                    if (distToTarget > attackRange)
                    {
                        state = EnemyState.Chase;
                    }
                    break;

                case EnemyState.Chase:
                    ctx.move.Chase(target);

                    if (distToTarget <= attackRange)
                    {
                        state = EnemyState.Attack;
                    }

                    break;

                case EnemyState.Attack:
                    ctx.move.Stop();
                    ctx.attack.TryAttack();

                    if (distToTarget > attackRange)
                    {
                        state = EnemyState.Chase;
                    }

                    break;

                case EnemyState.Dead:
                    Debug.Log(name + " is dead");
                    break;

                default:
                    Debug.LogWarning(name = " has no state");
                    break;
            }
        }
    }
}

using TP.Common;
using UnityEngine;

namespace TP.Enemies
{
    public class MeleeEnemyBrain : MonoBehaviour
    {
        private EnemyContext ctx;

        public float attackRange = 3.0f;

        public enum EnemyState { Idle, Chase, Attack, Dead }
        public EnemyState state;

        private void Awake()
        {
            ctx = GetComponent<EnemyContext>();
        }

        private void Start()
        {
            ctx.Health.OnDeath += EnterDeathState;
        }

        private void Update()
        {
            var target = ctx.PlayerSensor.Target;
            float distToTarget = 0;

            if (target != null)
            {
                distToTarget = Vector3.Distance(transform.position, target.position);
            }
            else
            {
                state = EnemyState.Idle;
            }

            if (ctx.Health.isAlive == false)
            {
                state = EnemyState.Dead;
            }

            //TODO: extract this to C# classes
            switch (state)
            {
                case EnemyState.Idle:
                    ctx.Move.Stop();

                    if (target == null)
                        break;

                    if (distToTarget <= attackRange)
                    {
                        state = EnemyState.Attack;
                    }
                    else
                    {
                        state = EnemyState.Chase;
                    }
                    break;

                case EnemyState.Chase:
                    if (target == null)
                    {
                        ctx.Move.Stop();
                        state = EnemyState.Idle;
                        break;
                    }

                    if (distToTarget <= attackRange)
                    {
                        ctx.Move.Stop();
                        state = EnemyState.Attack;
                        break;
                    }

                    ctx.Move.MoveTo(target.position);
                    break;

                case EnemyState.Attack:
                    ctx.Move.Stop();
                    ctx.Attack.TryAttack();

                    if (distToTarget > attackRange)
                    {
                        state = EnemyState.Chase;
                    }

                    break;

                case EnemyState.Dead:
                    Debug.Log(name + " is dead");
                    ctx.Move.Disable();
                    break;

                default:
                    Debug.LogWarning(name + " has no state");
                    break;
            }
        }

        private void EnterDeathState()
        {
            ctx.Move.Stop();
            ctx.Attack.StopAttack();
            state = EnemyState.Dead;
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            ctx.Health.OnDeath -= EnterDeathState;
        }
    }
}

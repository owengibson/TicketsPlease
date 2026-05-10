using TP.Common;
using UnityEngine;

namespace TP.Enemies
{
    public class MeleeEnemyBrain : MonoBehaviour
    {
        private EnemyContext ctx;

        public float attackRange = 3.0f;

        void Awake()
        {
            ctx = GetComponent<EnemyContext>();
        }

        void Update()
        {
            var target = ctx.playerSensor.Target;

            if (target == null) return;

            float dist = Vector3.Distance(ctx.transformRef.position, target.position);
            Debug.Log("Distance: " + dist);

            if (dist > attackRange)
            {
                ctx.move.SetDestination(target.position);
            }
            else
            {
                ctx.move.SetDestination(ctx.transformRef.position);
                ctx.attack.TryAttack();
            }
        }
    }
}

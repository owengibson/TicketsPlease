using TP.Common;
using UnityEngine;

namespace TP.Enemies
{
    public class EnemyBrain : MonoBehaviour
    {
        EnemyContext ctx;

        public float attackRange = 2f;

        void Awake()
        {
            ctx = GetComponent<EnemyContext>();
        }

        void Update()
        {
            var target = ctx.playerSensor.Target;

            if (target == null)
                return;

            float dist = Vector3.Distance(
                ctx.transformRef.position,
                target.position
            );

            if (dist > attackRange)
            {
                ctx.move.SetDestination(target.position);
            }
            else
            {
                ctx.attack.TryAttack();
            }
        }
    }
}

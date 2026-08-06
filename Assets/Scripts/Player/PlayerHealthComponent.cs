using System.Collections;
using TP.Combat;
using TP.Common;
using UnityEngine;

namespace TP.Player
{
    public class PlayerHealthComponent : HealthComponent
    {
        [SerializeField, Min(0f)] private float _invulnerabilitySeconds = 0.5f;

        private bool _isInvulnerable;
        private Coroutine _invulnerabilityRoutine;

        public bool IsInvulnerable => _isInvulnerable;

        public override void TakeHit(HitData hit)
        {
            TryTakeDamage(Mathf.CeilToInt(hit.Damage));
        }

        public override void TakeDamage(int damage)
        {
            TryTakeDamage(damage);
        }

        private void TryTakeDamage(int damage)
        {
            if (_isInvulnerable || !TryApplyDamage(damage))
                return;

            if (isAlive && _invulnerabilitySeconds > 0f)
                StartInvulnerability();
        }

        private void StartInvulnerability()
        {
            if (_invulnerabilityRoutine != null)
                StopCoroutine(_invulnerabilityRoutine);

            _invulnerabilityRoutine = StartCoroutine(InvulnerabilityRoutine());
        }

        private IEnumerator InvulnerabilityRoutine()
        {
            _isInvulnerable = true;
            yield return new WaitForSeconds(_invulnerabilitySeconds);
            _isInvulnerable = false;
            _invulnerabilityRoutine = null;
        }

        private void OnDisable()
        {
            _isInvulnerable = false;
            _invulnerabilityRoutine = null;
        }
    }
}

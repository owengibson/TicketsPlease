using System;
using System.Collections;
using System.Collections.Generic;
using TP.Combat.Weapons;
using UnityEngine;

namespace TP.Combat
{
    public class CombatActionExecutor : MonoBehaviour
    {
        [SerializeField] private Transform _actionOrigin;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _hitBufferSize = 64;

        private Collider[] _hitBuffer;
        private Coroutine _currentRoutine;

        public bool IsExecuting => _currentRoutine != null;

        private void Awake()
        {
            _hitBuffer = new Collider[Mathf.Max(8, _hitBufferSize)];
        }

        public bool ExecuteAction(
            GameObject attacker,
            WeaponInstance sourceWeapon,
            CombatActionDefinition action,
            Action<CombatActionDefinition, WeaponInstance> onComplete = null)
        {
            if (IsExecuting || attacker == null || action == null)
                return false;

            _currentRoutine = StartCoroutine(ExecuteRoutine(attacker, sourceWeapon, action, onComplete));
            return true;
        }

        public bool ExecuteAction(
            GameObject attacker,
            CombatActionDefinition action,
            Action<CombatActionDefinition, WeaponInstance> onComplete = null)
        {
            return ExecuteAction(attacker, null, action, onComplete);
        }

        private IEnumerator ExecuteRoutine(
            GameObject attacker,
            WeaponInstance sourceWeapon,
            CombatActionDefinition action,
            Action<CombatActionDefinition, WeaponInstance> onComplete)
        {
            PlayActionStart(action);

            if (action.StartupDuration > 0f)
                yield return new WaitForSeconds(action.StartupDuration);

            var hitTargets = new HashSet<IDamageable>();
            float activeEndTime = Time.time + action.ActiveDuration;

            do
            {
                ProcessActiveHits(attacker, sourceWeapon, action, hitTargets);
                yield return null;
            }
            while (action.ActiveDuration > 0f && Time.time < activeEndTime);

            if (action.RecoveryDuration > 0f)
                yield return new WaitForSeconds(action.RecoveryDuration);

            _currentRoutine = null;
            onComplete?.Invoke(action, sourceWeapon);
        }

        private void ProcessActiveHits(
            GameObject attacker,
            WeaponInstance sourceWeapon,
            CombatActionDefinition action,
            HashSet<IDamageable> hitTargets)
        {
            Transform origin = _actionOrigin != null ? _actionOrigin : attacker.transform;
            if (!HitboxUtility.TryBuildQuerySpec(attacker.transform, origin, action, out var querySpec))
                return;

            int hitCount = HitDetectionUtility.QueryNonAlloc(querySpec, _hitBuffer);
            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = _hitBuffer[i];
                if (hitCollider == null || IsOwnedByAttacker(hitCollider, attacker))
                    continue;

                var damageable = hitCollider.GetComponentInParent<IDamageable>();
                if (damageable == null)
                    continue;

                if (!action.AllowMultipleHits && !hitTargets.Add(damageable))
                    continue;

                var hitData = BuildHitData(attacker, hitCollider, damageable, sourceWeapon, action, querySpec.Center);
                damageable.TakeHit(hitData);
                PlayHitEffects(action, hitData.HitPoint);
            }
        }

        private HitData BuildHitData(
            GameObject attacker,
            Collider hitCollider,
            IDamageable damageable,
            WeaponInstance sourceWeapon,
            CombatActionDefinition action,
            Vector3 origin)
        {
            Vector3 hitPoint = hitCollider.ClosestPoint(origin);
            GameObject target = damageable is Component targetComponent
                ? targetComponent.gameObject
                : hitCollider.gameObject;

            Vector3 direction = target.transform.position - attacker.transform.position;
            if (direction.sqrMagnitude <= Mathf.Epsilon)
                direction = attacker.transform.forward;

            float damage = action.BaseDamage;
            if (sourceWeapon?.Definition != null)
                damage *= sourceWeapon.Definition.BaseDamageMultiplier;

            return new HitData
            {
                Attacker = attacker,
                Target = target,
                Damage = damage,
                Origin = origin,
                HitPoint = hitPoint,
                Direction = direction.normalized,
                KnockbackForce = action.KnockbackForce,
                SourceAction = action,
                SourceWeapon = sourceWeapon?.Definition,
                SourceWeaponInstance = sourceWeapon
            };
        }

        private static bool IsOwnedByAttacker(Collider hitCollider, GameObject attacker)
        {
            return hitCollider.transform == attacker.transform || hitCollider.transform.IsChildOf(attacker.transform);
        }

        private void PlayActionStart(CombatActionDefinition action)
        {
            if (_animator != null && !string.IsNullOrWhiteSpace(action.AnimationTrigger))
                _animator.SetTrigger(action.AnimationTrigger);

            if (_audioSource != null && action.UseSfx != null)
                _audioSource.PlayOneShot(action.UseSfx);
        }

        private void PlayHitEffects(CombatActionDefinition action, Vector3 hitPoint)
        {
            if (action.HitVfxPrefab != null)
                Instantiate(action.HitVfxPrefab, hitPoint, Quaternion.identity);

            if (_audioSource != null && action.HitSfx != null)
                _audioSource.PlayOneShot(action.HitSfx);
        }
    }
}

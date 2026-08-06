using UnityEngine;

namespace TP.Combat
{
    public static class HitboxUtility
    {
        public static bool TryFindHitbox(Transform root, string hitboxId, out HitboxAuthoring hitbox)
        {
            hitbox = null;
            if (root == null || string.IsNullOrWhiteSpace(hitboxId))
                return false;

            var hitboxes = root.GetComponentsInChildren<HitboxAuthoring>(true);
            foreach (var candidate in hitboxes)
            {
                if (candidate != null && candidate.HitboxId == hitboxId)
                {
                    hitbox = candidate;
                    return true;
                }
            }

            return false;
        }

        public static bool TryBuildQuerySpec(Transform attackerRoot, Transform fallbackOrigin, CombatActionDefinition action, out HitQuerySpec spec)
        {
            spec = default;
            if (action == null)
                return false;

            if (TryFindHitbox(attackerRoot, action.HitboxId, out var hitbox))
                return TryBuildQuerySpec(hitbox, action, out spec);

            if (!string.IsNullOrWhiteSpace(action.HitboxId))
                Debug.LogWarning($"Combat action '{action.name}' could not resolve hitbox id '{action.HitboxId}'.", attackerRoot);

            return TryBuildFallbackQuerySpec(fallbackOrigin != null ? fallbackOrigin : attackerRoot, action, out spec);
        }

        public static bool TryBuildQuerySpec(HitboxAuthoring hitbox, CombatActionDefinition action, out HitQuerySpec spec)
        {
            spec = default;
            if (hitbox == null || hitbox.HitCollider == null || action == null)
                return false;

            spec.TargetLayers = action.TargetLayers;
            spec.QueryTriggerInteraction = action.QueryTriggerInteraction;

            if (hitbox.HitCollider is BoxCollider box)
            {
                var transform = box.transform;
                spec.ShapeType = HitShapeType.Box;
                spec.Center = transform.TransformPoint(box.center);
                spec.Rotation = transform.rotation;
                spec.HalfExtents = Vector3.Scale(box.size, Abs(transform.lossyScale)) * 0.5f;
                return true;
            }

            if (hitbox.HitCollider is SphereCollider sphere)
            {
                var transform = sphere.transform;
                spec.ShapeType = HitShapeType.Sphere;
                spec.Center = transform.TransformPoint(sphere.center);
                spec.Radius = sphere.radius * MaxComponent(Abs(transform.lossyScale));
                return true;
            }

            if (hitbox.HitCollider is CapsuleCollider capsule)
            {
                var transform = capsule.transform;
                spec.ShapeType = HitShapeType.Capsule;
                spec.Rotation = transform.rotation;
                spec.Radius = capsule.radius * GetCapsuleRadiusScale(transform.lossyScale, capsule.direction);
                GetCapsuleWorldPoints(capsule, out spec.CapsulePointA, out spec.CapsulePointB);
                spec.Center = (spec.CapsulePointA + spec.CapsulePointB) * 0.5f;
                return true;
            }

            Debug.LogWarning($"Unsupported hitbox collider type '{hitbox.HitCollider.GetType().Name}' on '{hitbox.name}'.", hitbox);
            return false;
        }

        private static bool TryBuildFallbackQuerySpec(Transform origin, CombatActionDefinition action, out HitQuerySpec spec)
        {
            spec = default;
            if (origin == null || action == null)
                return false;

            spec.ShapeType = action.FallbackHitShape;
            spec.Center = origin.TransformPoint(action.FallbackHitOffset);
            spec.Rotation = origin.rotation;
            spec.HalfExtents = Vector3.Scale(action.FallbackHitBoxSize, Abs(origin.lossyScale)) * 0.5f;
            spec.Radius = action.FallbackHitRadius * MaxComponent(Abs(origin.lossyScale));
            spec.TargetLayers = action.TargetLayers;
            spec.QueryTriggerInteraction = action.QueryTriggerInteraction;

            Vector3 axis = origin.forward;
            float halfLine = Mathf.Max(0f, action.FallbackHitCapsuleHeight * 0.5f - action.FallbackHitRadius);
            spec.CapsulePointA = spec.Center + axis * halfLine;
            spec.CapsulePointB = spec.Center - axis * halfLine;
            return true;
        }

        private static void GetCapsuleWorldPoints(CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB)
        {
            Transform transform = capsule.transform;
            Vector3 center = transform.TransformPoint(capsule.center);
            Vector3 scale = Abs(transform.lossyScale);
            Vector3 axis = capsule.direction switch
            {
                0 => transform.right,
                1 => transform.up,
                _ => transform.forward
            };

            float heightScale = capsule.direction switch
            {
                0 => scale.x,
                1 => scale.y,
                _ => scale.z
            };

            float radius = capsule.radius * GetCapsuleRadiusScale(transform.lossyScale, capsule.direction);
            float height = Mathf.Max(capsule.height * heightScale, radius * 2f);
            float halfLine = Mathf.Max(0f, height * 0.5f - radius);
            pointA = center + axis * halfLine;
            pointB = center - axis * halfLine;
        }

        private static float GetCapsuleRadiusScale(Vector3 scale, int direction)
        {
            scale = Abs(scale);
            return direction switch
            {
                0 => Mathf.Max(scale.y, scale.z),
                1 => Mathf.Max(scale.x, scale.z),
                _ => Mathf.Max(scale.x, scale.y)
            };
        }

        private static Vector3 Abs(Vector3 value) => new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));

        private static float MaxComponent(Vector3 value) => Mathf.Max(value.x, Mathf.Max(value.y, value.z));
    }
}

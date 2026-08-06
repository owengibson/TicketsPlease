using UnityEngine;

namespace TP.Combat
{
    public static class HitDetectionUtility
    {
        private static readonly Collider[] ResultsBuffer = new Collider[64];

        public static int QueryNonAlloc(in HitQuerySpec spec, Collider[] results)
        {
            if (results == null || results.Length == 0)
                results = ResultsBuffer;

            return spec.ShapeType switch
            {
                HitShapeType.Sphere => Physics.OverlapSphereNonAlloc(
                    spec.Center,
                    spec.Radius,
                    results,
                    spec.TargetLayers,
                    spec.QueryTriggerInteraction),

                HitShapeType.Box => Physics.OverlapBoxNonAlloc(
                    spec.Center,
                    spec.HalfExtents,
                    results,
                    spec.Rotation,
                    spec.TargetLayers,
                    spec.QueryTriggerInteraction),

                HitShapeType.Capsule => Physics.OverlapCapsuleNonAlloc(
                    spec.CapsulePointA,
                    spec.CapsulePointB,
                    spec.Radius,
                    results,
                    spec.TargetLayers,
                    spec.QueryTriggerInteraction),

                _ => 0
            };
        }
    }
}

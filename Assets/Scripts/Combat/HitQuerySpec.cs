using UnityEngine;

namespace TP.Combat
{
    public struct HitQuerySpec
    {
        public HitShapeType ShapeType;
        public Vector3 Center;
        public Quaternion Rotation;
        public Vector3 HalfExtents;
        public float Radius;
        public Vector3 CapsulePointA;
        public Vector3 CapsulePointB;
        public LayerMask TargetLayers;
        public QueryTriggerInteraction QueryTriggerInteraction;
    }
}

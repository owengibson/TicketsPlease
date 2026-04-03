using UnityEngine;

namespace TP.Common
{
    public interface IDamageable
    {
        void TakeHit(HitData hit);
    }
}

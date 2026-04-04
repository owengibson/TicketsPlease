using TP.Common;
using UnityEngine;

namespace TP.Combat
{
    public interface IDamageable
    {
        void TakeHit(HitData hit);
    }
}

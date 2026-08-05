using UnityEngine;

namespace TP
{
    public interface IEnemyState
    {
        void Enter();
        void Tick(float deltaTime);
        void Exit();
    }
}

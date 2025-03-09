using UnityEngine;

namespace Gameplay.Monster
{
    public interface ITarget
    {
        public Vector3 Position { get; }
        public Vector3 Forward { get; }
        public float Speed { get; }
        public bool IsDead { get; }
        public void DealDamage(int damage);
    }
}
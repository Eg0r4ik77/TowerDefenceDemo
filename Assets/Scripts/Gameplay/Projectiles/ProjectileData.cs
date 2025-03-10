using UnityEngine;

namespace Gameplay.Projectiles
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Projectile", fileName = "Projectile")]
    public class ProjectileData : ScriptableObject
    {
        // At high speed values, the standard frequency of physical calculations is not enough to correctly handle parabolic movement.
        // Decreasing fixed timestep solves the problem, but changing global values is not desirable.
        // It is possible to code a control of physics calculation frequency for projectiles.
        // At the moment I will just set a speed limit.
        [field: SerializeField, Range(10, 70)] public float Speed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
    }
}
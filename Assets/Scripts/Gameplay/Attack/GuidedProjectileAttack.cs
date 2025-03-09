using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Attack
{
    public class GuidedProjectileAttack : ProjectileAttack<GuidedProjectile>
    {
        public GuidedProjectileAttack(GuidedProjectile projectilePrefab,
            Pose shootingPose,
            Transform projectilesRoot,
            int maxProjectilesCount) 
            : base(projectilePrefab, shootingPose, projectilesRoot, maxProjectilesCount) {}

        protected override void InitializeProjectile(GuidedProjectile projectile)
        {
            base.InitializeProjectile(projectile);
            
            projectile.SetTarget(target);
        }
    }
}
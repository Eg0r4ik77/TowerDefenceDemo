using Gameplay.Projectiles;
using Infrastructure.Pools;
using UnityEngine;

namespace Gameplay.Attack
{
    public class ProjectileAttack<TProjectile> : AttackStrategy 
        where TProjectile : Projectile
    {
        protected readonly TProjectile projectilePrefab;
        protected readonly Pose shootingPose;

        private readonly Pool<TProjectile> _projectilesPool;

        protected ProjectileAttack(TProjectile projectilePrefab, Pose shootingPose, Transform projectilesRoot, int maxProjectilesCount)
        {
            this.shootingPose = shootingPose;
            this.projectilePrefab = projectilePrefab;

            _projectilesPool = new Pool<TProjectile>(projectilePrefab, projectilesRoot, maxProjectilesCount);
        }
        
        protected virtual void InitializeProjectile(TProjectile projectile)
        {
            projectile.transform.position = shootingPose.position;
            projectile.transform.rotation = shootingPose.rotation;
        }
        
        public override void Attack()
        {
            var projectile = _projectilesPool.Get();
            InitializeProjectile(projectile);
        }
    }
}
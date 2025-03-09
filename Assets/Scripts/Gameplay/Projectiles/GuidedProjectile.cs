using Gameplay.Monster;
using UnityEngine;

namespace Gameplay.Projectiles
{
	public class GuidedProjectile : Projectile
	{
		private ITarget _target;
		
		public void SetTarget(ITarget target)
		{
			_target = target;
		}
		
		protected override void Translate()
		{
			var direction = transform.forward;
			
			if (!_target.IsDead)
			{
				direction = (_target.Position - transform.position).normalized;
				transform.LookAt(_target.Position);
			}
		
			var translation = direction * (Speed * Time.fixedDeltaTime);
		
			transform.Translate (translation, Space.World);
		}
	}
}

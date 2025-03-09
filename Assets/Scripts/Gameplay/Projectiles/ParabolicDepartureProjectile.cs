using UnityEngine;

namespace Gameplay.Projectiles
{
	public class ParabolicDepartureProjectile : Projectile
	{
		private Vector3 _startPosition;
		private float _distanceBeforeDeparture;
		private Vector3 _velocity;
		
		public void Initialize(float angle, float distanceBeforeDeparture)
		{
			_startPosition = transform.position;
			
			_distanceBeforeDeparture = distanceBeforeDeparture;
			
			_velocity = (transform.forward * Mathf.Sin(angle * Mathf.Deg2Rad) -
			             transform.up * Mathf.Cos(angle * Mathf.Deg2Rad)) * Speed;
		}
		
		protected override void Translate()
		{
			if (Vector3.Distance(transform.position, _startPosition) <= _distanceBeforeDeparture)
			{
				TranslateLinearly();
				return;
			}
		
			TranslateParabolic();
		}
		
		private void TranslateLinearly()
		{
			transform.position += transform.forward * (Speed * Time.fixedDeltaTime);
		}
		
		private void TranslateParabolic()
		{
			_velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
			transform.position += _velocity * Time.fixedDeltaTime;
		}
	}
}

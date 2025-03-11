using Gameplay.Attack;
using UnityEngine;

namespace Gameplay.Projectiles
{
	public class ParabolicDepartureProjectile : Projectile
	{
		private Vector3 _startPosition;
		private Vector3 _velocity;
		private float _distanceBeforeDeparture;
		
		public void Initialize(float angle, float distanceBeforeDeparture)
		{
			_startPosition = transform.position;
			_velocity = (transform.forward * Mathf.Sin(angle) - transform.up * Mathf.Cos(angle)) * Speed;
			_distanceBeforeDeparture = distanceBeforeDeparture;
		}
		
		protected override void Translate()
		{
			if (Vector3.Distance(transform.position, _startPosition) < _distanceBeforeDeparture)
				TranslateLinearly();
			else
				TranslateParabolic();
		}
		
		private void TranslateLinearly()
		{
			rigidbody.MovePosition(rigidbody.position + transform.forward * (Speed * Time.fixedDeltaTime));
		}
		 
		private void TranslateParabolic()
		{
			_velocity += rigidbody.transform.up * (ParabolicPredictionAttack.GravityAcceleration * Time.fixedDeltaTime);
			rigidbody.MovePosition(rigidbody.position + _velocity * Time.fixedDeltaTime);
		}
	}
}

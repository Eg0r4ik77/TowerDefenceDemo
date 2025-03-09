using Gameplay.Attack;
using UnityEngine;

namespace Gameplay.Towers.CannonTower
{
	public class CannonTower : Tower
	{
		[SerializeField] private Transform _shootPoint;
		[SerializeField] private Transform _projectileDeparturePoint;
		
		protected override void InitializeData()
		{
			base.InitializeData();

			if (data is not CannonTowerData cannonTowerData)
				return;
			
			var shootingPose = new Pose(_shootPoint.position, _shootPoint.rotation);
			
			attackStrategy = new ParabolicPredictionAttack(cannonTowerData.ProjectilePrefab,
				shootingPose,
				transform,
				cannonTowerData.MaxProjectilesCountInPool,
				cannonTowerData.RotationSpeed,
				cannonTowerData.MinimumAngleDifference,
				transform,
				_projectileDeparturePoint);
		}
	}
}

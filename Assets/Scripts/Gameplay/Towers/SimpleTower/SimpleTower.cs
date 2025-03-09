using Gameplay.Attack;
using UnityEngine;

namespace Gameplay.Towers.SimpleTower
{
	public class SimpleTower : Tower
	{
		protected override void InitializeData()
		{
			base.InitializeData();

			if (data is not SimpleTowerData simpleTowerData)
				return;
			
			var shootingPose = new Pose(transform.position + Vector3.up * 1.5f, Quaternion.identity);
			
			attackStrategy = new GuidedProjectileAttack(simpleTowerData.ProjectilePrefab,
				shootingPose, 
				transform,
				simpleTowerData.MaxProjectilesCountInPool);
		}
	}
}
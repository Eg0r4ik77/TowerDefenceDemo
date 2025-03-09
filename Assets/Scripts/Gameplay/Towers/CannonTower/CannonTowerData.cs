using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Towers.CannonTower
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Tower/CannonTower", fileName = "CannonTower")]
    public class CannonTowerData : TowerData
    {
        [field: SerializeField] public ParabolicDepartureProjectile ProjectilePrefab { get; private set; }
        [field: SerializeField] public int MaxProjectilesCountInPool { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float MinimumAngleDifference { get; private set; }
    }
}
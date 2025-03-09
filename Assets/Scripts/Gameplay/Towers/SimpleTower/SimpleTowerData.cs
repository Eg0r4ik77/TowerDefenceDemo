using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Towers.SimpleTower
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Tower/SimpleTower", fileName = "SimpleTower")]
    public class SimpleTowerData : TowerData
    {
        [field: SerializeField] public GuidedProjectile ProjectilePrefab { get; private set; }
        [field: SerializeField] public int MaxProjectilesCountInPool { get; private set; }
    }
}
using UnityEngine;

namespace Gameplay.Monster.Spawn
{
    [CreateAssetMenu(menuName = "Data/Gameplay/System/EnemySpawner", fileName = "EnemySpawner")]
    public class MonsterSpawnerData : ScriptableObject
    {
        [field: SerializeField] public float Interval { get; private set; }
        [field: SerializeField] public Monster MonsterPrefab { get; private set; }
        [field: SerializeField] public int MaxMonsterCountInPool { get; private set; }
    }
}
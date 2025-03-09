using UnityEngine;

namespace Gameplay.Monster
{
    [CreateAssetMenu(menuName = "Data/Gameplay/Monster", fileName = "Monster")]
    public class MonsterData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float ReachDistance { get; private set; }
    }
}
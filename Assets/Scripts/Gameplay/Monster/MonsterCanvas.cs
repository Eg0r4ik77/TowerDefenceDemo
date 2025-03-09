using R3;
using TMPro;
using UnityEngine;

namespace Gameplay.Monster
{
    public class MonsterCanvas : MonoBehaviour
    {
        [SerializeField] private Monster _monster;
        [SerializeField] private TMP_Text _healthTextMesh;
        
        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }

        private void Start()
        {
            _monster.CurrentHealth.Subscribe(ViewHealth).AddTo(this);
        }

        private void ViewHealth(float health)
        {
            _healthTextMesh.text = $"{health}";
        }
    }
}
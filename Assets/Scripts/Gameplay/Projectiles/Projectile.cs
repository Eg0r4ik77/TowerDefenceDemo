using Gameplay.Monster;
using Infrastructure.Pools;
using R3;
using UnityEngine;

namespace Gameplay.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Projectile : MonoBehaviour, IPoolObject
    {
        [SerializeField] private ProjectileData _data;
        
        protected Rigidbody rigidbody;
        
        private int _damage;
        private float _lifeTime;
        private float _speed;

        private Subject<Unit> _destroyed = new();
        private float _spawnTime;
        
        public float StartSpeed => _data.Speed;
        public float Speed => _speed;
        public Observable<Unit> Released => _destroyed;

        protected abstract void Translate();

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _damage = _data.Damage;
            _lifeTime = _data.LifeTime;
            _speed = _data.Speed;
        }
        
        private void Update()
        {
            CheckProjectileLifetime();
        }

        private void FixedUpdate()
        {
            Translate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy();
                return;
            }

            if (other.TryGetComponent(out ITarget target))
            {
                target.DealDamage(_damage);
                Destroy();   
            }
        }

        private void CheckProjectileLifetime()
        {
            if (_spawnTime + _lifeTime > Time.time)
                return;
			
            Destroy();
        }
        
        public void Reset()
        {
            _spawnTime = Time.time;
        }
        
        private void Destroy()
        {
            _destroyed?.OnNext(Unit.Default);
        }
    }
}
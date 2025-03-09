using Infrastructure.Pools;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay.Monster
{
	public class Monster : MonoBehaviour, ITarget, IPoolObject
	{
		[SerializeField] private MonsterData _data;
		[SerializeField] private Transform _bottomPoint;
		
		[SerializeField] private float _speed;
		[SerializeField] private int _maxHealth;
		[SerializeField] private float _reachDistance;

		private readonly ReactiveProperty<float> _currentHealth = new();
		private readonly Subject<Unit> _died = new();
		
		private ITargetsContainer _targetsContext;
		
		private Transform[] _routePoints;
		private int _currentWayPointIndex;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value - Vector3.up * _bottomPoint.localPosition.y;
		}

		public Vector3 Forward => transform.forward;
		public float Speed => _speed;
		public Observable<Unit> Released => _died;
		public ReadOnlyReactiveProperty<float> CurrentHealth => _currentHealth;
		
		[Inject]
		private void Construct(ITargetsContainer targetsContext)
		{
			_targetsContext = targetsContext;
		}
		
		private void Initialize()
		{
			_speed = _data.Speed;
			_maxHealth = _data.MaxHealth;
			_currentHealth.Value = _maxHealth;
			_reachDistance = _data.ReachDistance;
		}
		
		private void Start()
		{
			Initialize();
		}
		
		private void FixedUpdate ()
		{
			TryMove();
		}
		
		public void SetRoute(Transform[] wayPoints)
		{
			_routePoints = wayPoints;
		}
		
		public void DealDamage(int damage)
		{
			_currentHealth.Value -= damage;

			if (_currentHealth.Value <= 0)
			{
				Die();
			}
		}
		
		public void Reset()
		{
			_currentHealth.Value = _maxHealth;
			_currentWayPointIndex = 0;
		}
		
		private void TryMove()
		{
			if (_currentWayPointIndex >= _routePoints.Length)
			{
				Die();
				return;
			}
			
			var point = _routePoints[_currentWayPointIndex];
			
			if (Vector3.Distance(_bottomPoint.position, point.position) <= _reachDistance)
			{
				transform.rotation = point.rotation;
				_currentWayPointIndex++;
				return;
			}

			var direction = (point.position - _bottomPoint.position).normalized;

			transform.position += direction * (_speed * Time.fixedDeltaTime);
		}

		private void Die()
		{
			_targetsContext.Remove(this);
			_died.OnNext(Unit.Default);	
		}
	}
}

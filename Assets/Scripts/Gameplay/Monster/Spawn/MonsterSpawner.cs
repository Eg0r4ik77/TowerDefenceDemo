using Infrastructure.Pools;
using UnityEngine;
using VContainer;

namespace Gameplay.Monster.Spawn
{
	public class MonsterSpawner : MonoBehaviour
	{
		[SerializeField] private Transform[] _routePoints;
		[SerializeField] private MonsterSpawnerData _data;
		[SerializeField] private Transform _spawnPoint;
		
		private IObjectResolver _objectResolver;
		private ITargetsContainer _targetsContainer;
		
		private float _interval;
		private Monster _monsterPrefab;
		private int _maxMonsterCount;
		
		private Pool<Monster> _monsterPool;
		private float _lastSpawnTime;
		
		[Inject]
		public void Construct(IObjectResolver objectResolver, ITargetsContainer targetsContainer)
		{
			_objectResolver = objectResolver;
			_targetsContainer = targetsContainer;
		}

		public void Initialize()
		{
			_interval = _data.Interval;
			_monsterPrefab = _data.MonsterPrefab;
			_maxMonsterCount = _data.MaxMonsterCountInPool;
		}
		
		public void Start()
		{
			Initialize();
			
			_monsterPool = new Pool<Monster>(_monsterPrefab, _spawnPoint, _maxMonsterCount);
			_lastSpawnTime = -_interval;
		}

		private void Update()
		{
			TrySpawn();
		}

		private void TrySpawn()
		{
			if (_lastSpawnTime + _interval > Time.time)
				return;
			
			_lastSpawnTime = Time.time;
			SpawnMonster();
		}
		
		private void SpawnMonster()
		{
			var monster = _monsterPool.Get();

			_objectResolver.Inject(monster);
			_targetsContainer.Add(monster);
			
			monster.Position = _spawnPoint.position;
			monster.transform.rotation = _spawnPoint.rotation;
			monster.SetRoute(_routePoints);
		}
	}
}

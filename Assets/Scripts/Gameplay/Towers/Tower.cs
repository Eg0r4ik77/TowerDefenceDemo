using System.Linq;
using Gameplay.Attack;
using Gameplay.Monster;
using UnityEngine;
using VContainer;

namespace Gameplay.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] protected TowerData data;

        protected AttackStrategy attackStrategy;
        
        private float _attackInterval;
        private float _range;
        private float _findTargetInterval;
        private bool _followTarget; 
        
        private ITargetsContainer _targetsContainer;
        private ITarget _target;
        
        private float _lastAttackTime;
        private float _lastFindTargetTime;
        
        [Inject]
        private void Construct(ITargetsContainer targetsContainer)
        {
            _targetsContainer = targetsContainer;
        }

        protected virtual void InitializeData()
        {
            _attackInterval = data.AttackTimeInterval;
            _range = data.Range;
            _findTargetInterval = data.FindTargetInterval;
            _followTarget = data.FollowTarget;
        }

        private void Start()
        {
            InitializeData();
            
            _lastAttackTime = -_attackInterval;
            _lastFindTargetTime = -_findTargetInterval;
        }

        private void Update()
        {
            if (_lastFindTargetTime + _findTargetInterval < Time.time)
            {
                FindTarget();
                _lastFindTargetTime = Time.time;
            }

            TryAttack();
        }

        private void FindTarget()
        {
            if (_followTarget && !ReferenceEquals(_target, null) && IsTargetInRange(_target))
                return;
            
            var sceneTargets = _targetsContainer.Targets;
            
            _target = sceneTargets
                .Where(target => target != null && IsTargetInRange(target))
                .OrderBy(target => Vector3.Distance(transform.position, target.Position))
                .FirstOrDefault();
            
            attackStrategy.SetTarget(_target);
        }
        
        private void TryAttack()
        {
            if (!attackStrategy.ReadyToAttack())
                return;
            
            if (_lastAttackTime + _attackInterval >= Time.time)
                return;
            
            attackStrategy.Attack();
            _lastAttackTime = Time.time;
        }
        
        private bool IsTargetInRange(ITarget target)
        {
            return Vector3.Distance(transform.position, target.Position) <= _range;
        }
    }
}
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Attack
{
   public class ParabolicPredictionAttack : ProjectileAttack<ParabolicDepartureProjectile>
    {
        private readonly float _rotationSpeed;
        private readonly float _minimumAngleDifference;
        private readonly float _distanceBeforeDeparture;
        private readonly Transform _towerTransform;
        private readonly Transform _departurePoint;
        
        private Vector3? _predictedPosition;

        public ParabolicPredictionAttack(ParabolicDepartureProjectile projectilePrefab,
            Pose shootingPose,
            Transform projectilesRoot,
            int maxProjectilesCount,
            float rotationSpeed,
            float minimumAngleDifference, 
            Transform towerTransform,
            Transform departurePoint) 
            : base(projectilePrefab, shootingPose, projectilesRoot, maxProjectilesCount)
        {
            _rotationSpeed = rotationSpeed;
            _minimumAngleDifference = minimumAngleDifference;
            _towerTransform = towerTransform;
            _departurePoint = departurePoint;
            _distanceBeforeDeparture = (_departurePoint.position - shootingPose.position).magnitude;
        }

        public override bool ReadyToAttack()
        {
            if (!base.ReadyToAttack())
                return false;
            
            _predictedPosition = CalculatePredictedPosition();
            
             var adjustedPredictedPosition = _predictedPosition.Value;
             adjustedPredictedPosition.y = _departurePoint.position.y;
            
             var angleBetweenCannonAndTarget =
                 Vector3.Angle(adjustedPredictedPosition - _departurePoint.position, _departurePoint.position - shootingPose.position);

             RotateTo(_predictedPosition.Value);
             
#if UNITY_EDITOR
             Debug.DrawLine(shootingPose.position, adjustedPredictedPosition, Color.red);
             Debug.DrawRay(shootingPose.position, (_departurePoint.position - shootingPose.position).normalized * 10,
                 Color.green);
#endif
             return angleBetweenCannonAndTarget < _minimumAngleDifference;
        }

        protected override void InitializeProjectile(ParabolicDepartureProjectile projectile)
        {
            base.InitializeProjectile(projectile);

            var shootingAngle = Vector3.Angle(-shootingPose.up, _predictedPosition.Value - shootingPose.position);
            
            projectile.transform.rotation = _departurePoint.rotation;
            projectile.Initialize(shootingAngle, _distanceBeforeDeparture);
        }
        
        private Vector3 CalculatePredictedPosition()
        {
            var g = Physics.gravity.y;
			     
            var projectileSpeed = projectilePrefab.StartSpeed;
            var targetSpeed = target.Speed;
        
            var deltaY = _departurePoint.position.y - target.Position.y;
            var deltaX = _departurePoint.position.x - target.Position.x;
        
            var shootingAngle = Vector3.Angle(-shootingPose.up, target.Position - _departurePoint.position);
        
            var sin = Mathf.Sin(shootingAngle * Mathf.Deg2Rad);
            var cos = Mathf.Cos(shootingAngle * Mathf.Deg2Rad);
        
            var flightTime = deltaY / (projectileSpeed * cos + g * deltaX / (2 * (targetSpeed - projectileSpeed * sin)));
			     
            var projectileDepartureTime = _distanceBeforeDeparture / projectileSpeed;
            flightTime += projectileDepartureTime;
			     
            var predictedPosition = target.Position + target.Forward * (target.Speed * flightTime);
        
            return predictedPosition;
        }
        
        private void RotateTo(Vector3 position)
        {
            var direction = (position - _towerTransform.position).normalized;
			     
            direction.y = 0;
        
            var rotation = Quaternion.LookRotation(direction);
            var degreesDelta = _rotationSpeed * Time.deltaTime;
        
            _towerTransform.rotation = Quaternion.RotateTowards(_towerTransform.rotation, rotation, degreesDelta);
        }
    }
}
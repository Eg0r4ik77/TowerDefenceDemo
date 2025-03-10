using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Attack
{
   public class ParabolicPredictionAttack : ProjectileAttack<ParabolicDepartureProjectile>
    {
        struct Prediction
        {
            public Vector3 Position;
            public float Angle;
        }
        
        public const float GravityAcceleration = -9.81f;
        private const float MinAngleDifference = 1f;
        
        private readonly float _rotationSpeed;
        private readonly float _distanceBeforeDeparture;
        private readonly Transform _towerTransform;
        private readonly Transform _departurePoint;
        
        private Prediction? _prediction;

        public ParabolicPredictionAttack(ParabolicDepartureProjectile projectilePrefab,
            Pose shootingPose,
            Transform projectilesRoot,
            int maxProjectilesCount,
            float rotationSpeed, 
            Transform towerTransform,
            Transform departurePoint) 
            : base(projectilePrefab, shootingPose, projectilesRoot, maxProjectilesCount)
        {
            _rotationSpeed = rotationSpeed; ;
            _towerTransform = towerTransform;
            _departurePoint = departurePoint;
            _distanceBeforeDeparture = (_departurePoint.position - shootingPose.position).magnitude;
        }

        public override bool ReadyToAttack()
        {
            if (!base.ReadyToAttack())
                return false;
            
            _prediction = CalculatePredictedPosition();

            if (_prediction == null)
                return false;
            
            var adjustedPredictedPosition = _prediction.Value.Position;
            adjustedPredictedPosition.y = _departurePoint.position.y;
            
            var angleBetweenCannonAndTarget =
                Vector3.Angle(adjustedPredictedPosition - _departurePoint.position, _departurePoint.forward);
            
             
#if UNITY_EDITOR
            Debug.DrawLine(_departurePoint.position, adjustedPredictedPosition, Color.red);
            Debug.DrawRay(_departurePoint.position, _departurePoint.forward * 10);
#endif

            RotateTo(_prediction.Value.Position);
            
            return angleBetweenCannonAndTarget < MinAngleDifference;
        }

        protected override void InitializeProjectile(ParabolicDepartureProjectile projectile)
        {
            base.InitializeProjectile(projectile);
            
            projectile.transform.rotation = _departurePoint.rotation;
            projectile.Initialize(_prediction.Value.Angle, _distanceBeforeDeparture);
        }
        
        private Prediction? CalculatePredictedPosition()
        {
            const float predictionSegment = 2f;
            const float predictionStep = 0.1f;
            
            var projectileSpeed = projectilePrefab.StartSpeed;
            var targetSpeed = target.Speed;
            
            var projectileDepartureTime = _distanceBeforeDeparture / projectileSpeed;
            
            for (float step = 0; step <= predictionSegment; step += predictionStep)
            {
                var nextTargetPosition = target.Position + step * target.Forward;
                
                var nextDeparturePosition = nextTargetPosition;
                nextDeparturePosition.y = shootingPose.position.y;
                
                var nextDepartureDirection = (nextDeparturePosition - shootingPose.position).normalized;
                nextDeparturePosition = shootingPose.position + nextDepartureDirection * _distanceBeforeDeparture;
                
                var time = step / targetSpeed - projectileDepartureTime;
                var xozTranslation = new Vector3(nextTargetPosition.x - nextDeparturePosition.x, 0, nextTargetPosition.z - nextDeparturePosition.z);
                
                var sin =  (xozTranslation.magnitude) / (projectileSpeed * time);
                var cos = (nextDeparturePosition.y - nextTargetPosition.y + GravityAcceleration * time * time / 2) 
                          / (projectileSpeed * time);
                
                if (sin >= 0 && sin <= 1f && cos >= 0 && cos <= 1f)
                {
                    var angle = cos == 0
                        ? Mathf.PI / 2
                        : Mathf.Atan(sin / cos);
                    
                    var prediction = new Prediction
                    {
                        Position = nextTargetPosition,
                        Angle = angle
                    };
                    
                    return prediction;
                }
            }

            return null;
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
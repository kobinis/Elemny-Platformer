//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using XnaUtils;

//namespace SolarConflict {
    
//    /// <summary>Movement logic for moving between random points near a target</summary>
//    /// <remarks>Moves in a straight line towards various points near the target, makes no effort to avoid the target or other objects</remarks>
//    [Serializable]
//    public class StaggerNearTarget : BaseUpdate {

//        /// <summary>If true, will perfectly copy the target's movement each frame before executing its own movements</summary>
//        /// <remarks>Reinitialized whenever this object is passed a new projectile (so won't work properly if you're using the same StaggerNearTarget object
//        /// for multiple concurrent projectiles)</remarks>
//        bool _boundToTargetFrame;

//        Vector2? _desiredRelativePosition;

//        float _desiredDistanceFromDesiredRelativePosition = 20f;
        
//        public float Force;

//        Projectile _lastProjectile;
//        GameObject _lastTarget;

//        public float MaxSpeed;

//        public float Radius;

//        public ProjectileTargetType Target;

//        /// <summary>If non-null, this is an upper bound on the time spent approaching a target point before randomly selecting another</summary>
//        public int? TargetChangeCooldownTime;

//        int _targetChangeCooldown;
//        Vector2? _targetLastPosition;
        
//        /// <param name="bindToTargetFrame">See _boundToTargetFrame</param>        
//        public StaggerNearTarget(ProjectileTargetType target, float radius, float force, float maxSpeed, bool bindToTargetFrame = false, int? targetChangeCooldownTime = null) {
//            Radius = radius;
//            Target = target;
//            MaxSpeed = maxSpeed;
//            Force = force;
//            _boundToTargetFrame = bindToTargetFrame;
//            TargetChangeCooldownTime = targetChangeCooldownTime;
//        }
//        public StaggerNearTarget(ProjectileTargetType target, float radius, bool bindToTargetFrame = false) : this(target, radius, 0f, 0f, bindToTargetFrame) { }
       
//        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine) {
//            var target = projectile.GetProjectileTarget(gameEngine, Target);

//            --_targetChangeCooldown;

//            if (!(target?.IsActive ?? false))
//                // No target
//                return;            

//            if (_boundToTargetFrame) {
//                // Match target's movements
//                if (projectile != _lastProjectile || target != _lastTarget)
//                    // New projectile or target, reinitialize
//                    _targetLastPosition = null;

//                _lastProjectile = projectile;
//                _lastTarget = target;
                                
//                _targetLastPosition = _targetLastPosition ?? target.Position;                

//                projectile.Position += target.Position - _targetLastPosition.Value;
//                _targetLastPosition = target.Position;
//            }

//            // Are we there yet? Or uninitialized? Or just tired of trying to get there?
//            var diff = target.Position + _desiredRelativePosition - projectile.Position;
//            var timedOut = (TargetChangeCooldownTime != null) && _targetChangeCooldown <= 0;

//            if ((!_desiredRelativePosition.HasValue) || timedOut|| diff.Value.LengthSquared() < _desiredDistanceFromDesiredRelativePosition * _desiredDistanceFromDesiredRelativePosition) {
//                // Yeah, or uninitialized. Find some new point to orbit
//                _desiredRelativePosition = gameEngine.Rand.PointInRing(target.Size, target.Size + Radius);
//                diff = target.Position + _desiredRelativePosition - projectile.Position;

//                _targetChangeCooldown = TargetChangeCooldownTime ?? int.MaxValue;
//            }
            
//            // Acceleration
//            projectile.ApplyForce(diff.Value.Normalized() * Force, MaxSpeed);

//            // Translation                        
//            projectile.Position += projectile.Velocity;
//        }
//    }
//}

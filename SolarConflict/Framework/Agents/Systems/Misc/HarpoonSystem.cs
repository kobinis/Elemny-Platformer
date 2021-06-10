using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using SolarConflict.Framework.Agents.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc {
    /// Harpoon has four states: seeking (functioning as an ordinary projectile), pulling (projectile is gone, GameObject which was hit is being pulled),
    /// retracting (projectile is back, but cannot hook stuff, and is being reeled back into the system), and inactive (no projectile, system might or might not be on cooldown)</remarks>
    [Serializable]
    class HarpoonSystem : AgentSystem {
        /// <summary>Distance at which a harpoon is considered fully retracted</summary>   
        const float HarpoonRetractionEpsilon = 50f;
        /// <summary>We'll retract the harpoon faster when it gets this close</summary>
        const float HarpoonRetractionNearDistance = 200f;
        const float HarpoonRetractionFarSpeed = 50f;
        const float HarpoonRetractionNearSpeed = 100f;


        public ActivationCheck ActivationCheck;

        private int _cooldown;
        public int CooldownTime;

        Projectile _harpoon;
        GameObject _harpoonedObject;

        public bool IsRetracting;
        /// <summary>Profile for the harpoon in its "seeking" state</summary>
        public ProjectileProfile HarpoonProfile;
        ProjectileProfile _retractingHarpoonProfile;

        public float MinTetherLength = 40;

        public float PullSpreedLimit;

        public int SeekDuration;
        /// <remarks>Could just give the active harpoon projectile an expiration time, instead, assume seeking's timed out if it's inactive. This field is just for clarity</remarks>
        int _seekTime;

        public float SpringConstant;

        public int TetherDuration;
        float _tetherLength;
        int _tetherTime;

        public Vector2 Velocity;

        /// <summary>Rate at which the tether gets shorter once something is harpooned (per frame)</summary>
        public float WinchRate;

        private Vector2 _originPoint;
        private Camera _camera;
        private SetPixel _pixel;
        private Sprite _harpoonSprite;

        public HarpoonSystem() {
            HarpoonProfile = MakeActiveHarpoonProfile();
            _retractingHarpoonProfile = MakeRetractingHarpoonProfile();
            _harpoonSprite = Sprite.Get("link128");
        }

        public override float GetCooldown() {
            return _cooldown;
        }

        public override AgentSystem GetWorkingCopy() {
            HarpoonSystem system = MemberwiseClone() as HarpoonSystem;
            system.IsRetracting = false;
            system._harpoon = null;
            system._harpoonedObject = null;
            system._pixel = null;
            return system;
        }

        ProjectileProfile MakeActiveHarpoonProfile() {
            // Just add damage and a collision handler to the retracting harpoon; active harpoon looks the same, but hooks on impact
            var result = MakeRetractingHarpoonProfile();

            result.CollisionSpec = new CollisionSpec(100f, 5f);
            result.CollusionUpdateList.Add(new HarpoonImpactHandler());

            return result;
        }

        ProjectileProfile MakeRetractingHarpoonProfile() {
            var result = new ProjectileProfile();
            result.DrawType = DrawType.Additive;
            result.TextureID = "link128";

            result.CollisionWidth = result.Sprite.Width - 5;
            result.InitSizeID = "15";

            result.Mass = 0.1f;            
            result.IsDestroyedOnCollision = false;
            result.IsDestroyedWhenParentDestroyed = true;

            result.CollusionUpdateList.Add(new HarpoonImpactHandler());

            return result;
        }

        public override void Reset() {
            _harpoon = null;
            _harpoonedObject = null;
            IsRetracting = false;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false) {
            _originPoint = initPosition;

            if (_harpoon != null) {
                if (!IsRetracting) {
                    // CASE I: Seeking (harpoon is out; isn't retracting)

                    // When the harpoon hits something, it makes it its parent. Next frame, we delete that projectile and make the magic happen
                    if ((_harpoon?.Parent ?? agent) != agent) {
                        // Hit detected
                        _harpoon.IsActive = false;

                        _harpoonedObject = _harpoon.Parent;

                        _tetherLength = (_harpoonedObject.Position - agent.Position).Length();
                        _tetherTime = TetherDuration;
                        _harpoon = null;
                        // TODO: relative position of tether in harpooned object
                    } else if (_seekTime <= 0) {
                        // Seeking timed out, or projectile died somehow. The latter shouldn't be possible, but may as well fail gracefully
                        // Begin retraction (discreetly replace projectile)
                        _harpoon.IsActive = false;
                        _harpoon = _retractingHarpoonProfile.Emit(gameEngine, agent, agent.FactionType, _harpoon.Position, _harpoon.Velocity, _harpoon.Rotation) as Projectile;
                        IsRetracting = true;
                    }

                    --_seekTime;
                    return false;
                }
            }

            if (_harpoonedObject != null) {
                // CASE II: Pulling (tether is out)

                if ((!_harpoonedObject.IsActive) || _tetherTime <= 0) {
                    // Time to break free, though. Begin retracting
                    IsRetracting = true;

                    var diff = (_harpoonedObject.Position - agent.Position);                                        
                    _harpoon = _retractingHarpoonProfile.Emit(gameEngine, agent, agent.FactionType, _harpoonedObject.Position, _harpoonedObject.Velocity,
                        (float)Math.Atan2(diff.Y, diff.X)) as Projectile;

                    _harpoonedObject = null;
                } else {
                    // Continue pulling
                    --_tetherTime;
                    _tetherLength -= WinchRate;
                    _tetherLength = Math.Max(_tetherLength, MinTetherLength);

                    var diff = (_harpoonedObject.Position - agent.Position);
                    var distance = diff.Length();
                    var direction = diff.Normalized();

                    if (distance > _tetherLength) {
                        // YOINK!
                        var force = SpringConstant * distance;
                        var massRatio = agent.Mass / _harpoonedObject.Mass;

                        agent.ApplyForce(direction * force, PullSpreedLimit);
                        _harpoonedObject.ApplyForce(direction * (-force), PullSpreedLimit);
                    }
                }

                return false;
            }

            if (IsRetracting && _harpoon != null) {
                // CASE III: Retracting
                var diff = agent.Position - _harpoon.Position;
                var distanceSquared = diff.LengthSquared();

                if (distanceSquared <= HarpoonRetractionEpsilon * HarpoonRetractionEpsilon) {
                    // Done retracting
                    IsRetracting = false;
                    _harpoon.IsActive = false;
                    _harpoon = null;                    
                } else {
                    // Keep retracting
                    // Are we really close to the harpoon? If so, pull faster
                    //var acceleration = distanceSquared < (HarpoonRetractionNearDistance * HarpoonRetractionNearDistance) ? HarpoonRetractionNearAcceleration : HarpoonRetractionFarAcceleration;
                    //_harpoon.Velocity += acceleration * diff.Normalized();
                    var speed = distanceSquared < (HarpoonRetractionNearDistance * HarpoonRetractionNearDistance) ? HarpoonRetractionNearSpeed : HarpoonRetractionFarSpeed;
                    _harpoon.Velocity = speed * diff.Normalized();
                }

                return false;
            }

            // CASE IV: Inactive
            if (_cooldown > 0)
                --_cooldown;
            // But not on cooldown. Consider activating
            else if ((ActivationCheck?.Check(agent, tryActivate) ?? true) && _harpoon == null && _harpoonedObject == null) {
                // Note that we set the cooldown immediately, but only start decrementing it once the harpoon is fully retracted
                _cooldown = CooldownTime;

                ActivationCheck?.DrainCost(agent);

                var rotatedVelocity = FMath.RotateVector(Velocity, initRotation);
                _harpoon = HarpoonProfile.Emit(gameEngine, agent, agent.FactionType, initPosition, agent.Velocity + rotatedVelocity, agent.Rotation) as Projectile;
                _seekTime = SeekDuration;

                return true;
            }                                    
           
            return false;            
        }
        
        public void DrawHarpoonPoint(SpriteBatch sb, Vector2 point, Color color) {
            if (_camera != null)
                sb.Draw(_harpoonSprite.Texture, point, null, color, 0, _harpoonSprite.Origin, 0.25f, SpriteEffects.None, 0);           
        }

        public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha) 
        {
            //SetPixel          
            _camera = camera;
            _pixel = _pixel ?? new SetPixel(DrawHarpoonPoint);

            if (_harpoon != null)
                GraphicsUtils.Line(camera.SpriteBatch,_originPoint, _harpoon.Position, Color.White, 128 / 2f, _pixel);
            if (_harpoonedObject != null)
                GraphicsUtils.Line(camera.SpriteBatch,_originPoint, _harpoonedObject.Position, Color.White, 128 / 2f, _pixel);
        }

    }


    [Serializable]
    public class HarpoonImpactHandler : BaseImpactUpdate {
        public override void Update(Projectile projectile, GameObject collidingObject, GameEngine gameEngine) {
            // When you collide with something, become its adoptive child
            projectile.Parent = collidingObject;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{
    [Serializable]
    public class Projectile : GameObject
    {
        public ProjectileProfile profile;
        public Color color;
        public float hitPoints;
        public float maxLifeTime;
        //public FactionType Faction;
        //public Vector2 origin;

        public override int Level => profile.Level;
        

        public override string Name
        {
            get
            {
                return profile.Name;
            }

            set
            {
            }
        }

        public override string Tag
        {
            get { return profile.Name; }
        }

        public override PointLight Light
        {
            get { return profile.Light; }            
        }

        public override float NormalizedLifetime()
        {
            return Lifetime / (float)maxLifeTime;
        }

        public override void Update(GameEngine gameEngine)
        {
            float normalizedLife = Lifetime / maxLifeTime;

            if (profile._colorLogic != null)
                profile._colorLogic.Update(this, normalizedLife, gameEngine);

            if (profile.MovementLogic != null)
            {
                profile.MovementLogic.Update(this, normalizedLife, gameEngine);
            }
            else
            {
                Position += Velocity;
                Velocity *= profile.VelocityInertia;
            }

            if (profile.RotationLogic != null)
            {
                profile.RotationLogic.Update(this, normalizedLife, gameEngine);
            }
            else
            {
                Rotation += RotationSpeed;
                RotationSpeed *= profile.RotationInertia;
            }

            if (profile.UpdateSize != null)
                profile.UpdateSize.Update(this, normalizedLife, gameEngine);

            foreach (BaseUpdate projectileUpdater in profile.UpdateList)
            {
                projectileUpdater.Update(this, normalizedLife, gameEngine);
            }

            if (profile.UpdateEmitter != null && Lifetime % profile.UpdateEmitterCooldownTime == 0 && !IsNotActive)
                profile.UpdateEmitter.Emit(gameEngine, this, this.GetFactionType(), this.Position, this.Velocity, this.Rotation);
            //Emit(this, gameEngine.addList, this.Position, this.Velocity, this.Rotation);                

            //ToDo: move???
            if (profile.IsDestroyedWhenParentDestroyed && this.Parent != null && this.Parent.IsNotActive)
            {
                IsNotActive = true;
            }

            if (hitPoints <= 0 & IsActive) //?????? maybe to move
            {
                if (profile.HitPointZeroEmiiter != null && IsActive)
                    profile.HitPointZeroEmiiter.Emit(gameEngine, this, this.GetFactionType(), this.Position, this.Velocity, this.Rotation);
                IsActive = false;
            }

            if (Lifetime >= maxLifeTime && !IsNotActive)
                OnTimeOut(gameEngine);

            Lifetime++;
        }

        public override void Draw(Camera camera)
        {
            if (!this.IsNotActive)
            {
#if DEBUG
               // if (DebugUtils.IsDebug && profile.DrawType != DrawType.Lit)
              //      camera.CameraCircle(Position, Size, Color.Multiply(DebugUtils.DebugColor, ParticleColor.A / 255f)); //Remove Multiply
#endif

                if (profile.Draw != null) //change
                    profile.Draw.Draw(camera, this);
                else
                {
                    if (profile.Sprite != null) //???
                        camera.CameraDraw(profile.Sprite, Position, Rotation, Size * profile.ScaleMult, color);
                    // else                  
                }

                foreach (var drawItem in profile.DrawList)
                {
                    drawItem.Draw(camera, this);
                }
            }
        }

        public override Sprite GetSprite()
        {
            return profile.Sprite;
        }

        void OnTimeOut(GameEngine gameEngine)
        {
            if (profile.TimeOutEmitter != null)
                profile.TimeOutEmitter.Emit(gameEngine, this, this.GetFactionType(), this.Position, this.Velocity, this.Rotation);
            IsNotActive = true;
        }

        //TODO: refactor 
        public override void ApplyCollision(GameObject collidingObj, GameEngine gameEngine)
        {
            if (this.Parent != collidingObj) //??
            {
                
                if (profile.IsEffectedByForce && collidingObj.Parent != this) //or Hit Parent
                {
                    switch (collidingObj.CollisionInfo.ForceType)
                    {
                        case ForceType.FromCenter:
                            Vector2 reletiveForce = (this.Position - collidingObj.Position);
                            if (reletiveForce != Vector2.Zero)
                            {
                                reletiveForce.Normalize();
                                Velocity += reletiveForce * collidingObj.CollisionInfo.Force / profile.Mass;
                            }                        
                            break;
                        case ForceType.Velocity:
                            //ToDo                                    
                            break;
                        case ForceType.Rotation:
                            Velocity += FMath.ToCartesian(collidingObj.CollisionInfo.Force, collidingObj.Rotation);
                            break;
                        case ForceType.Gravity:
                            Vector2 relPos = (this.Position - collidingObj.Position);
                            if (relPos != Vector2.Zero)
                            {
                                float lengthSquerd = relPos.LengthSquared();

                                Velocity += relPos * collidingObj.CollisionInfo.Force / (lengthSquerd + 1);
                            }
                            break;
                    }

                }

                if (collidingObj.CollisionInfo.Flags.HasFlag(CollisionSpecFlags.IsRotating) && profile.IsTurnedByForce && collidingObj.Parent != this)
                {
                    Vector2 r = collidingObj.Position - this.Position;
                    float torque = (r.X * collidingObj.Velocity.Y - r.Y * collidingObj.Velocity.X) * collidingObj.CollisionInfo.Force;
                    torque = Math.Min(Math.Abs(torque), 5) * Math.Sign(torque); //ToDo: maxTorque(5) needs to be a param?
                    RotationSpeed += torque / (Size * profile.RotationMass);
                }

                if (collidingObj.CollisionInfo.Flags.HasFlag(CollisionSpecFlags.AffectsAllies) || (collidingObj.GetFactionType() != GetFactionType()) )
                {
                    if (profile.ImpactEmitter != null && IsActive && (profile.ImpactEmitterCooldownTime == 0 || gameEngine.FrameCounter % profile.ImpactEmitterCooldownTime == 0)) //??
                        profile.ImpactEmitter.Emit(gameEngine, this, this.GetFactionType(), this.Position, this.Velocity, this.Rotation);

                    // Update Hitpoints !!!!!!!!!!!!!!!!!!!1
                    foreach (var impactValue in collidingObj.CollisionInfo.ImpactList) //add check effect allies 
                    {
                        if (impactValue.meterType == MeterType.Damage)// || impectValue.meterType == MeterType.Hitpoints)
                        {
                            switch (impactValue.impactType)
                            {
                                case ImpactType.Additive:
                                    hitPoints -= impactValue.amount;
                                    break;
                                case ImpactType.Max:
                                    hitPoints = Math.Min(hitPoints, hitPoints - impactValue.amount);
                                    break;
                                case ImpactType.Mult: //a way to implement a buff
                                    hitPoints *= impactValue.amount;
                                    break;
                                case ImpactType.Velocity:
                                    float relVelocity = (collidingObj.Velocity - this.Velocity).Length();
                                    hitPoints -= impactValue.amount * relVelocity;
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (impactValue.meterType == MeterType.Hitpoints)// || impectValue.meterType == MeterType.Hitpoints)
                        {
                            switch (impactValue.impactType)
                            {
                                case ImpactType.Additive:
                                    hitPoints += impactValue.amount;
                                    break;
                                case ImpactType.Max://fix //remove
                                    hitPoints = Math.Max(hitPoints, hitPoints + impactValue.amount);
                                    break;
                                case ImpactType.Mult: //a way to implament a buff
                                    hitPoints *= impactValue.amount;
                                    break;
                                case ImpactType.Velocity:
                                    float relVelocity = (collidingObj.Velocity - this.Velocity).Length();
                                    hitPoints += impactValue.amount * relVelocity;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                    if (profile.IsDestroyedOnCollision)
                        IsActive = false;
                }
            }



            foreach (var impactUpdate in profile.CollusionUpdateList)
            {
                impactUpdate.Update(this, collidingObj, gameEngine);
            }


            if (hitPoints <= 0 & IsActive) //?????? maybe to move
            {
                if (profile.HitPointZeroEmiiter != null && IsActive)
                    profile.HitPointZeroEmiiter.Emit(gameEngine, this, this.GetFactionType(), this.Position, this.Velocity, this.Rotation);
                IsActive = false;
            }

            //list of impact updaters
        }


        public override CollisionSpec CollisionInfo //can be more sefi
        {
            get { return profile.CollisionSpec; }
            set { }
        }


        public override CollisionType ListType
        {
            get { return profile.CollisionType; }
            set { }
        }

        public override DrawType DrawType
        {
            get { return profile.DrawType; }
            set { }
        }


        public float MaxLifeTime
        {
            get { return this.maxLifeTime; }
            set { this.maxLifeTime = value; }
        }


        public Color ParticleColor
        {
            get { return color; }
            set { color = value; }
        }

        public void SetColorAlpha(byte alpha)
        {
            color.A = alpha;
        }

        //public override pa

        public override FactionType GetFactionType()
        {
            switch (profile.FactionLogic)
            {
                case ProjectileProfile.FactionLogicType.Parent:
                    if (Parent != null)
                        return Parent.GetFactionType();
                    else
                        return profile.FactionType;                   
                case ProjectileProfile.FactionLogicType.Const:
                    return profile.FactionType;
                case ProjectileProfile.FactionLogicType.Param:
                    return (FactionType)((int)Param);
                default:
                    return profile.FactionType;
            }
           
        }

        public override string GetId()
        {
            return Id;
        }

        public GameObject GetProjectileTarget(GameEngine gameEngine, ProjectileTargetType targetType)
        {
            GameObject target = null;

            switch (targetType)
            {
                case ProjectileTargetType.None:
                    return null;               
                case ProjectileTargetType.Parent:
                    if (Parent != null && Parent.IsActive)
                        target = this.Parent;
                    else
                        target = null;
                    break;
                case ProjectileTargetType.AgentAncestor:
                    target = this.GetAgentAncestor();
                    break;
                case ProjectileTargetType.LivePrimeAncestor:
                    target = this.GetLivePrimeAncestor();
                    break;
                case ProjectileTargetType.PrimeAncestor:
                    target = this.GetPrimeAncestor();
                    break;
                case ProjectileTargetType.Enemy:
                    target = gameEngine.CollisionManager.FindClosestTarget(Position, profile.AggroRange, GameObjectType.PotentialTarget | GameObjectType.IsProjectileTarget, gameEngine.GetFaction(GetFactionType()));
                    break;
                case ProjectileTargetType.AnyPotentialTarget:
                    target = gameEngine.CollisionManager.FindClosestTarget(Position, profile.AggroRange, GameObjectType.PotentialTarget, null);
                    break;
                case ProjectileTargetType.AnyObject:
                    target = gameEngine.CollisionManager.FindClosestTarget(Position, profile.AggroRange, GameObjectType.All, null);
                    break;
                case ProjectileTargetType.SameFaction:
                    if (Parent != null)
                    {
                        target = gameEngine.CollisionManager.FindClosestTarget(Position, profile.AggroRange, GameObjectType.PotentialTarget, gameEngine.GetFaction(Parent.GetFactionType()));
                    }
                    break;
                case ProjectileTargetType.ParentTarget:
                    if (Parent != null)
                    {
                        target = Parent.GetTarget(gameEngine, TargetType.Enemy);
                    }
                    break;
                case ProjectileTargetType.AncestorTarget:
                    if (this.GetAgentAncestor() != null)
                    {
                        target = GetAgentAncestor().GetTarget(gameEngine, TargetType.Enemy);
                    }
                    break;
                case ProjectileTargetType.Natural:
                    target = gameEngine.CollisionManager.FindClosestTarget(Position, profile.AggroRange, GameObjectType.PotentialTarget, gameEngine.GetFaction(FactionType.Neutral));
                    break;
                case ProjectileTargetType.Self:
                    target = this;
                    break;
                case ProjectileTargetType.Player:
                    if(gameEngine.PlayerAgent != null)
                    {
                        target = gameEngine.PlayerAgent;
                    }
                    break;
                
                case ProjectileTargetType.PointerObject:
                    throw new NotImplementedException();
                    //break;
                default:
                    break;
            }

            return target;
        }

        //??? maybe needs to return null
        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType) //To Do : Target selection of allises 
        {

            if (Parent != null)
                return Parent.GetTarget(gameEngine, targetType);
            else
                return null;
        }


        public override GameObject GetAgentAncestor()
        {
            if (Parent != null)
                return Parent.GetAgentAncestor();
            else
                return null;
        }

        public override void ApplyForce(Vector2 force, float speedLimit) //fix it
        {
            float dotProduct = Math.Max(Vector2.Dot(force, Velocity), 0);
            float alpha = Math.Min(Velocity.LengthSquared() / (speedLimit * speedLimit), 1);
            Vector2 newForce;
            if (Velocity.LengthSquared() > 0.001f)
                newForce = force - alpha * (dotProduct * Velocity) / Velocity.LengthSquared();
            else
                newForce = force; //fix
            Velocity += newForce / profile.Mass;
        }

        public override void AddToKills()
        {
            GameObject ancestor = GetAgentAncestor();
            if (ancestor != null)
            {
                ancestor.AddToKills();
            }
        }

        public string Id
        {
            get { return profile.ID; }
            //set { }
        }

        public override float Mass {
            get {
                return profile.Mass;
            }

            set {
                throw new Exception("Projectile mass is read-only (defined in profile)");
            }
        }

        public override void SetMeterValue(MeterType type, float value)
        {
            if (type == MeterType.Hitpoints)
            {
                hitPoints = value;
            }
            else
            {
                //if(meterValues == null)
                //{
                //    meterValues = new Dictionary<MeterType, float>(1);
                //}
                //meterValues[type] = value;
            }

        }

        public override float GetMeterValue(MeterType type)
        {

            if (type == MeterType.Hitpoints)
            {
                //throw new Exception(); //TODO: just for testing
                return hitPoints;
            }
            //if (meterValues != null)
            //{
            //    float value;
            //    if (meterValues.TryGetValue(type, out value))
            //    {
            //        return value;
            //    }                   
            //}
            return 0;
        }

        public override void SetColor(Color? color) //remove
        {
            if (color != null)
            {
                this.color = color.Value;
            }
        }

        public override Color GetColor()
        {
            return this.color;
        }


        public override GameObjectType GetObjectType()
        {
            return profile.ObjectType;
        }

        public override GameObjectType GetCollideWithMask()
        {
            return profile.CollideWithMask;
        }

        public override float GetDisplayScale()
        {
            return Size * profile.ScaleMult;
        }

        public override CraftingStationType GetCraftingStationType()
        {
            return profile.CraftingStationType;
        }
    }

}

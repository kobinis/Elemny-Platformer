using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;
using XnaUtils;

namespace SolarConflict
{
    //public enum InitVelocityType { Normal, Enemy, Ally, LastHittingObject, ClosestDanger, Goal }
    public enum RotationSpeedType { Zero, Const, Random }
    /// <summary>
    /// EmitterCallerSystem - is used to call emitters from an agent,
    /// To be used for guns, effects and a lot of other things
    /// </summary>
    [Serializable]
    public class EmitterCallerSystem : AgentSystem //TODO: check, add warmup and warmup emitter
    {
        /// <summary>
        /// Check if can activate system
        /// </summary>
        public ActivationCheck ActivationCheck;
        /// <summary>
        /// Number of frames that the system will be active after activation
        /// </summary>
        public int ActiveTime;
        /// <summary>
        /// Cooldown between activations
        /// </summary>
        public int CooldownTime;
        /// <summary>
        /// Cooldown between emitter calls when system activated, default = 0;
        /// </summary>
        public int MidCooldownTime;        
        /// <summary>
        /// Main Emitter used for main projectile
        /// </summary>
        public IEmitter Emitter;                
        /// <summary>
        /// Secondary Emitter, usually used for effects
        /// </summary>
        public IEmitter SecondaryEmitter;
        /// <summary>
        /// Third Emitter, usually used for sound
        /// </summary>
        public IEmitter ThirdEmitter;                 
        /// <summary>
        /// Activation Emitter is called once per activation
        /// </summary>
        public IEmitter ActivationEmitter;
        /// <summary>
        /// EndOfWarmupEmitter is called once when warmpup ends
        /// </summary>
        public IEmitter EndOfWarmupEmitter;
        /// <summary>
        /// Warmup emitter
        /// </summary>
        public IEmitter WarmupEmitter;
        /// <summary>
        /// Time that it takes to fire from activation
        /// </summary>
        public int WarmupTime;
        /// <summary>
        /// If set overwrites the main emitter color
        /// </summary>           
        public Color? Color;
        /// <summary>
        /// If set overwrites the main emitter MaxLifetime
        /// </summary>           
        public int MaxLifetime = 0;
        /// <summary>
        /// If set overwrites the main emitter size
        /// </summary>           
        public float? Size = null;
        /// <summary>
        /// If set overwrites the main secondary emitter color
        /// </summary>
        public Color? SecondaryColor;
        /// <summary>
        /// If set overwrites the main secondary emitter MaxLifetime
        /// </summary>
        public int SecondaryMaxLifetime = 0;
        /// <summary>
        /// If set overwrites the main secondary emitter size
        /// </summary>
        public float? SecondarySize = null;

        public RotationSpeedType RotationSpeed;
        public float RotationSpeedRange = 0;     

        public CollisionSpec SelfImpactSpec;
        public float SelfSpeedLimit = 100;

        public float secondaryVelocityMult = 0;
        public float refVelocityMult;

        public bool activateOnlyOnEmit;
        public bool DontActivateEmitterWhenCloaked;

        public ItemCategory AmmoTypeNeeded;
        private Item ammoItem;
        public ParamEmitter ammoParamEmitter;
        
        private int warmupCounter;
        public Vector2 velocity;
        private int cooldown;
        private int midCooldown;
        private int active;

        public float EmitterSpeed
        {
            set { velocity = Vector2.UnitX * value; }
        }

        public string EmitterID
        {
            set { Emitter = ContentBank.Inst.GetEmitter(value); }
        }

        public string SecondaryEmitterID
        {
            set { SecondaryEmitter = ContentBank.Inst.GetEmitter(value); }
        }

        public string ThirdEmitterID
        {
            set { ThirdEmitter = ContentBank.Inst.GetEmitter(value); }
        }

        public string ActivationEmitterID
        {
            set { ActivationEmitter = ContentBank.Inst.GetEmitter(value); }
        }

        public string EndOfWarmupEmitterID
        {
            set { EndOfWarmupEmitter = ContentBank.Inst.GetEmitter(value); }
        }


    public EmitterCallerSystem()
            : this(0, 0, Vector2.Zero, null)
        {

        }

        public EmitterCallerSystem(string emitterId)
            : this(ControlSignals.None, 0, Vector2.Zero, ContentBank.Inst.GetEmitter(emitterId))
        {

        }


        public EmitterCallerSystem(ControlSignals signal, string emitterId)
            : this(signal, 0, Vector2.Zero, ContentBank.Inst.GetEmitter(emitterId))
        {

        }

        public EmitterCallerSystem(ControlSignals signal, int cooldownTime, string emitterId)
            : this(signal, cooldownTime, Vector2.Zero, ContentBank.Inst.GetEmitter(emitterId))
        {

        }

        public EmitterCallerSystem(ControlSignals signal, int cooldownTime, Vector2 velocity, IEmitter emitter)
        {
            ActivationCheck = new ActivationCheck(signal);
            this.CooldownTime = cooldownTime;
            this.velocity = velocity;
            this.Emitter = emitter;
            this.ActivationEmitter = null;
            ActiveTime = 1;
            MidCooldownTime = 0;
            refVelocityMult = 1;
            AmmoTypeNeeded = ItemCategory.None;
            cooldown = 0;
            midCooldown = 0;
            active = 0;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            bool wasActivated = false;
            if ( cooldown <=0 && warmupCounter <= 0 && ( ActivationCheck == null || ActivationCheck.Check(agent, tryActivate)) && (AmmoTypeNeeded == ItemCategory.None || ((ammoItem = CheckForAmmo(agent, AmmoTypeNeeded)) != null)))
            {
                float refRotationSpeed = 0; //TO change
                int maxLifetime = 0;
                float? size = null;

                GameObject emittedObject = null;
                if (ActivationEmitter != null)
                {
                    emittedObject = ActivationEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity, initRotation, refRotationSpeed, maxLifetime, size, Color);                    
                }

                if (!activateOnlyOnEmit || emittedObject != null)
                {
                    wasActivated = true;
                    active = ActiveTime;
                    if (ActivationCheck != null)
                        ActivationCheck.DrainCost(agent);
                    if (ammoItem != null)
                        ammoItem.Stack--; //?add If consome ammo              
                    cooldown = CooldownTime;
                    warmupCounter = WarmupTime;
                }               
            }

            GameObject gameObject = null;
            if (warmupCounter <= 1)
            {
                if(warmupCounter == 1)
                {
                    EndOfWarmupEmitter?.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity, initRotation, 0, MaxLifetime, Size, Color);
                }
                if (active > 0 && midCooldown <= 0)
                {
                    float refRotationSpeed = RotationSpeedRange;

                    switch (this.RotationSpeed)
                    {
                        case RotationSpeedType.Zero:
                            refRotationSpeed = 0;
                            break;
                        case RotationSpeedType.Const:
                            refRotationSpeed = RotationSpeedRange;
                            break;
                        case RotationSpeedType.Random:
                            refRotationSpeed = (float)gameEngine.Rand.NextDouble() * RotationSpeedRange;
                            break;
                        default:
                            break;
                    }


                    Vector2 rotatedVelocity = velocity;// agent.RotateVector(velocity);
                    if (initRotation != 0) //TODO: maybe remove if
                    {
                        Vector2 initRotVector = new Vector2((float)Math.Cos(initRotation), (float)Math.Sin(initRotation));
                        rotatedVelocity = FMath.RotateVector(rotatedVelocity, initRotVector);
                    }

                    if (Emitter != null && (!DontActivateEmitterWhenCloaked || !agent.IsCloaked))
                    {
                        gameObject = Emitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity + rotatedVelocity, initRotation, refRotationSpeed, MaxLifetime, Size, Color);
                    }

                    if (ammoItem != null && (!DontActivateEmitterWhenCloaked || !agent.IsCloaked))
                    {
                        if (ammoParamEmitter == null)
                            gameObject = ammoItem.Profile.AmmoEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity + rotatedVelocity, initRotation, refRotationSpeed, MaxLifetime, Size, Color);
                        else
                        {
                            ammoParamEmitter.Emitter = ammoItem.Profile.AmmoEmitter;
                            gameObject = ammoParamEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity + rotatedVelocity, initRotation, refRotationSpeed, MaxLifetime, Size, Color);
                        }
                    }

                    if (SecondaryEmitter != null)
                    {
                        SecondaryEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity + rotatedVelocity * secondaryVelocityMult,
                            initRotation, 0, SecondaryMaxLifetime, SecondarySize, SecondaryColor); //Maybe do something with rotation Speed
                    }

                    if (ThirdEmitter != null)
                    {
                        ThirdEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity + rotatedVelocity * secondaryVelocityMult,
                            initRotation, 0, 0, null, null);
                    }

                    if (SelfImpactSpec != null)
                    {
                        agent.ApplyCollisionMeters(SelfImpactSpec);

                        if (SelfImpactSpec.ForceType == ForceType.DirectionOfMovment)  //TODO: fix
                        {
                            if (Math.Abs(agent.Velocity.X) + Math.Abs( agent.Velocity.Y) > 0.01f)
                            {
                                Vector2 froce = -agent.Velocity.Normalized() * SelfImpactSpec.Force;
                                agent.ApplyForce(froce, SelfSpeedLimit);
                            }
                            else
                            {
                                if (agent.analogDiractions[0] != Vector2.Zero)
                                {
                                    Vector2 froce = -agent.analogDiractions[0].Normalized() * SelfImpactSpec.Force;
                                    agent.ApplyForce(froce, SelfSpeedLimit);
                                }
                            }
                        }
                        else
                        {
                            agent.ApplyForce(rotatedVelocity * SelfImpactSpec.Force, SelfSpeedLimit);
                        }
                    }
                    midCooldown = MidCooldownTime;
                }
                active--;
            }
            else
            {               
                if(WarmupEmitter != null && midCooldown <= 0)
                {
                    midCooldown = MidCooldownTime;
                    WarmupEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, refVelocityMult * agent.Velocity, initRotation, 0);
                }
            }

            warmupCounter--;
            cooldown = Math.Max(cooldown - 1, 0); //maybe remove the max
            midCooldown = Math.Max(midCooldown - 1, 0); //maybe remove the max
            return wasActivated;
        }

        public override AgentSystem GetWorkingCopy()
        {
            var result = MemberwiseClone() as EmitterCallerSystem;
            result.ammoItem = null;      
            return result;
        }

        public override float GetCooldownTime()
        {
            return CooldownTime;
        }

        public override float GetCooldown()
        {
            return cooldown;
        }

        private Item CheckForAmmo(Agent agent, ItemCategory ammoTypeNeeded)
        {
            if (agent.Inventory == null)
                return null;
            for (int i = 0; i < agent.Inventory.Items.Length; i++)
            {
                var item = agent.Inventory.Items[i];
                if (item != null && (item.Category & ammoTypeNeeded) > 0)
                {
                    return item;
                }
            }
            return null;
        }

    }
}

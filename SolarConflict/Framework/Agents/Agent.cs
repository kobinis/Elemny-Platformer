using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using SolarConflict.Framework;
using XnaUtils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using XnaUtils.Graphics;
using System.Linq;
using SolarConflict.GameContent;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;
using SolarConflict.AI.GameAI;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict
{
    //[Serializable]
    //public class StatMultipliers { //Maybe replace with meters

    //    public float EngineAcceleration;
    //    public float EngineMaxSpeed;


    //    public float ShieldStrength;
    //    public float ShieldRegeneration;

    //    public StatMultipliers() {
    //        EngineAcceleration = 1f;
    //        EngineMaxSpeed = 1f;

    //        ShieldStrength = 1f;
    //        ShieldRegeneration = 1f;
    //    }
    //}
    [Serializable]
    public class Agent : GameObject, IEmitter, IGameObjectFactory
    {
        public override PointLight Light { get; set; }            
        
        public IInteractionSystem InteractionSystem { get; set; }
        //private Integrity integrity;
        /// <summary>
        /// time since last taking damage 
        /// </summary>
        public int DamageTimer { get { return Lifetime - lifetimeWhenTakingDamage; } }

        private int lifetimeWhenTakingDamage;

        private float minDisFromDanger; // Holds the minimal distance from closest damaging projectile
        private GameObject closestDanger; //closest damaging gameObject
        public GameObject lastDamagingObjectToCollide;
        public int colideTimer; //Time since last damage        

        public SizeType SizeType;

        public GameObjectType collideWithMask;

       
        public GameObjectType gameObjectType = GameObjectType.Agent;
        //public string LoadoutID { get; set; }


        public override int Level { get; set; }

        // Rotation Params
        /// <summary>0 - stop rotating when no force is applyed. 1 - keeps rotating</summary>
        public float RotationInertia;
        public float RotationMass; //remove read Moment of Inertia on wiki

        // Movment Params
        public float VelocityInertia; //   0 - stop moving when no force is applyed. 1 - keeps moving
        float _mass; //remove mass, maybe change to proberty
        public override float Mass {
            get {
                return _mass;
            }

            set {
                _mass = value;
            }
        }

        public CollisionType listType;
        public DrawType _drawType;
        public CollisionSpec impactSpec; //remove

        /// <summary>Systems not associated with an item slot</summary>
        private List<AgentSystem> _slotlessSystems;
        private List<AgentSystem> _slotlessAfterSystems;
        //post Systems

        protected Dictionary<MeterType, Meter> meters;
        // protected List<PresistentEffect> _presistentEffects;

        /// <remarks>TODO: implement</remarks>
        //public StatMultipliers Multipliers = new StatMultipliers();
        
        private ControlSignals controlSignals;
        public Vector2[] analogDiractions; //This!!!!!!       

        public ControlHolder control;

        public FactionType FactionType;
        public bool RotateOnImpact;
        public Sprite Sprite;
        public AgentDraw draw;

        public Vector2 appliedForce;
        public Vector2 appliedForceBeforeReductions;
        protected float applayedRotetionalForce;
        protected Vector2 headingVec; // this is just the save repeated calculations        

        public TargetSelector targetSelector;//change ???                
        public Inventory Inventory; //TODO: changed to public to enable ships to have different size cargos, rethink          
        public ItemSlotsContainer ItemSlotsContainer = null; //!!!!!!!!!!!!!!!!!!!!!!
        public float HullCost { get; set; }

        /// <summary>Distance at which stuff is visible, but not necessarily identifiable</summary>
        //public float DetectionRadius;
        /// <summary>Distance at which stuff is identifiable</summary>
        //public float VisionRadius;        
        public bool isControllable;

        public CraftingStationType CraftingStationType;

        protected string id;

        public string _name;
        public override string Name
        {
            get {return _name;}
            set {_name = value;}
        }

        public override string Tag
        {
            get { return _name; }
        }

        #region Constructors

        //remove 
        public Agent():base() {

            lifetimeWhenTakingDamage = -60 * 60;
            collideWithMask = GameObjectType.All;
            appliedForce = Vector2.Zero;
            applayedRotetionalForce = 0;

            minDisFromDanger = float.MaxValue; // Holdes the minimal distanse from closeset damaging projectile
            closestDanger = null; //closest projectiles TODO: change name

            Lifetime = 0;

            // Rotation Params
            RotationInertia = 0; // 0 - stop rotating when no force is applyed. 1 - keeps rotating
            RotationMass = 1; //remove read Moment of Inertia on wiki

            // Movment Params
            VelocityInertia = 0.98f; //   0 - stop moving when no force is applyed. 1 - keeps moving
            _mass = 1; //remove mass, maybe change to proberty

            listType = CollisionType.CollideAll; //ToDo: check why if colide 2 ships don't die
            _drawType = DrawType.Alpha;
            impactSpec = CollisionSpec.SpecForce; //remove

            // public Sprite _sprite;

            _slotlessSystems = new List<AgentSystem>();            

            meters = new Dictionary<MeterType, Meter>();
            meters.Add(MeterType.Hitpoints, new Meter(100));
            // protected List<PresistentEffect> _presistentEffects;

            //Move to ControlSignals class
            controlSignals = 0;
            analogDiractions = new Vector2[2];
            RotateOnImpact = false; //needs to be changed to private
            headingVec = Vector2.Zero; // this is just the save repeated calculations                                                      

            gameObjectType = GameObjectType.Agent;
           // Inventory = new Inventory(9); //TODO: change to null
            control = new ControlHolder(); //null
            targetSelector = new TargetSelector(); // null

            isControllable = true;
        }

        #endregion

        public string ID {
            get { return id; }
            set { id = value; }
        }

        public GameObject ClosesetDanger {
            get { return closestDanger; }
            set { closestDanger = value; }
        }

        public void AddSystem(AgentSystem system) {
            if (system == null)
                return;    
            if(system is IInteractionSystem && InteractionSystem == null)
            {
                InteractionSystem = system as IInteractionSystem;
            }
            else
                _slotlessSystems.Add(system);
        }

        public T GetSystem<T>() where T : AgentSystem {
            var result = Systems.FirstOrDefault(s => s is T);
            if (result != null)
                return result as T;
            return null;
        }

        public void RemoveSystem(AgentSystem system) {
            _slotlessSystems.Remove(system);
        }

        public void AddAfterSystem(AgentSystem system) {
            if (_slotlessAfterSystems == null)
                _slotlessAfterSystems = new List<AgentSystem>();
            _slotlessAfterSystems.Add(system);
        }

        public void ApplyTorque(float torque) {
            applayedRotetionalForce += torque;
        }

        public override void ApplyForce(Vector2 force, float speedLimit) {
            appliedForceBeforeReductions += force;
            float dotProduct = Math.Max(Vector2.Dot(force, Velocity), 0);
            float alpha = Math.Min(Velocity.LengthSquared() / (speedLimit * speedLimit), 1);
            Vector2 newForce;
            if (Velocity.LengthSquared() > 0.001f)
                newForce = force - alpha * (dotProduct * Velocity) / Velocity.LengthSquared();
            else
                newForce = force;
            appliedForce += newForce;

            /**if (control.ControlType == AgentControlType.Player)
                Utility.Log($"V: {Velocity.Length()} MaxV: {speedLimit}");/**/
        }

        public void ApplyForce(float force, float forceAngle, float speedLimit) //maybe remove????
        {
            Vector2 vecForce = new Vector2((float)Math.Cos(forceAngle + Rotation) * force, (float)Math.Sin(forceAngle + Rotation) * force);
            float dotProduct = Math.Max(Vector2.Dot(vecForce, Velocity), 0);
            float alpha = Math.Min(Velocity.LengthSquared() / (speedLimit * speedLimit), 1);
            Vector2 newForce;
            if (Velocity.LengthSquared() > 0.001f)
                newForce = vecForce - alpha * (dotProduct * Velocity) / Velocity.LengthSquared();
            else
                newForce = vecForce;
            appliedForce += newForce;
        }

        /// <summary>
        /// Applays force and torque
        /// </summary>        
        public void ApplyForce(Vector2 force, Vector2 conntactPoint, float speedLimit) {
            float dotProduct = Math.Max(Vector2.Dot(force, Velocity), 0);
            float alpha = Math.Min(Velocity.LengthSquared() / (speedLimit * speedLimit), 1);
            Vector2 newForce;

            if (Velocity.LengthSquared() > 0.001f)
                newForce = force - alpha * (dotProduct * Velocity) / Velocity.LengthSquared();
            else
                newForce = force;

            appliedForce += newForce;

            Vector2 r = Position - conntactPoint; //by kobi - need to save this vector //compine applay force            
            applayedRotetionalForce -= (r.X * force.Y - r.Y * force.X);
        }




        /// <summary>
        /// Apply impect on this agent - 
        /// </summary>
        /// <param name="collidingObj"></param>
        public override void ApplyCollision(GameObject collidingObj, GameEngine gameEngine) // check if APPLAY IMPECT HAPPANS TWICE
        {
            this.colideTimer = 0; //this.ControlSig |= ControlSignals.OnColision;

            //possibly add faction
            if (collidingObj.IsActive && collidingObj.Parent != this && (collidingObj.CollisionInfo.Flags & CollisionSpecFlags.IsNotPushingAllies) == 0)  //add another chack if(Or Colide With parent) 
            {
                Vector2 reletivePos = (this.Position - collidingObj.Position);

                //Applys rotation
                if (reletivePos != Vector2.Zero) {
                    // needs to check if relForce id zero                          
                    //  applayedForce += relSpeed * reletiveForce * 1 * 0.05f; 
                    if (RotateOnImpact)
                    {
                        Vector2 r = collidingObj.Position - this.Position;
                        ApplyTorque((r.X * collidingObj.Velocity.Y - r.Y * collidingObj.Velocity.X) * collidingObj.CollisionInfo.Force * 0.01f);
                    }
                }
                
                //Applys force
                switch (collidingObj.CollisionInfo.ForceType) //KOBI: fix and add 
                {
                    case ForceType.FromCenter:
                        if (reletivePos != Vector2.Zero) {
                            reletivePos.Normalize();
                            appliedForce += reletivePos * collidingObj.CollisionInfo.Force;
                        }
                        else
                        {
                            appliedForce = FMath.ToCartesian(0.1f, gameEngine.Rand.NextFloat());
                        }
                        break;

                    case ForceType.Rotation:
                        appliedForce += FMath.ToCartesian(collidingObj.CollisionInfo.Force, collidingObj.Rotation) * Math.Min(_mass, 1000);
                        break;
                    case ForceType.Velocity:
                        appliedForce = collidingObj.CollisionInfo.Force * collidingObj.Velocity; //maybe normelize
                        break;
                    //case ForceType.Rotation:
                    //    //applayedForce += collidingObj.ImpactSpec.Force * new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                    //    applayedForce += collidingObj.ImpactSpec.Force * new Vector2((float)Math.Cos(collidingObj.Rotation), (float)Math.Sin(collidingObj.Rotation));
                    //    break;
                    case ForceType.Gravity:
                        Vector2 relPos = (this.Position - collidingObj.Position);
                        if (relPos != Vector2.Zero) {
                            float lengthSquerd = relPos.LengthSquared();
                            appliedForce += relPos * collidingObj.CollisionInfo.Force / (lengthSquerd + 1) * this._mass;
                        }
                        break;
                    case ForceType.Mult:
                        Velocity *= collidingObj.CollisionInfo.Force;
                        break;
                    case ForceType.DirectionOfMovment:
                        if (Velocity != Vector2.Zero)
                        {
                            appliedForce += Velocity.Normalized() * collidingObj.CollisionInfo.Force * Math.Min(_mass, 1000);                 
                        }
                        break;
                }
                /* Vector2 relPos = this.Position - spr.Position;
                  Vector2 relVel = (this.Velocity - spr.Velocity);
                  float relSpeed = Math.Max(-Vector2.Dot(relPos , relVel),0);
                  Vector2 reletiveForce = spr.Velocity - this.Velocity;*/

                //maybe seperate to allie and enemy //TODO: check
                if ( (collidingObj.GetFactionType() != FactionType) || collidingObj.CollisionInfo.Flags.HasFlag(CollisionSpecFlags.AffectsAllies)
                    || (!collidingObj.CollisionInfo.IsDamaging) || (collidingObj.GetFactionType() == 0 && (collidingObj.CollisionInfo.Flags & CollisionSpecFlags.DoNotEffectNeutral) == 0))
                {
                    if (collidingObj.CollisionInfo.IsDamaging) //??? 
                        lastDamagingObjectToCollide = collidingObj;

                    ApplyCollisionMeters(collidingObj.CollisionInfo, collidingObj.Velocity); //TODO: add buffs
                }                
            }
        }

        public void ApplyCollisionMeters(CollisionSpec appliedImpactSpec, Vector2 velocity = new Vector2()) //change
        {

            foreach (var impectValue in appliedImpactSpec.ImpactList) {
                switch (impectValue.impactType) {
                    case ImpactType.Additive:
                        AddMeterValue(impectValue.meterType, impectValue.amount);
                        break;
                    case ImpactType.Max:
                        MaxMeterValue(impectValue.meterType, impectValue.amount);
                        break;
                    case ImpactType.Min:
                        MinMeterValue(impectValue.meterType, impectValue.amount);
                        break;
                    case ImpactType.Mult: //a way to implament a buff, mult 1.2 to damage ???? //remove //add damage buff meter
                        MultMeterValue(impectValue.meterType, impectValue.amount);
                        break;
                    case ImpactType.Velocity:
                        float relVelocity = (velocity - this.Velocity).Length();
                        AddMeterValue(impectValue.meterType, impectValue.amount * relVelocity);
                        break;
                    case ImpactType.TargetHitpoints:
                        AddMeterValue(impectValue.meterType, impectValue.amount * this.CurrentHitpoints);
                        break;
                    /*case ImpactType.TargetHitpoints:  //ToDo: MAX HITPOINTS
                        AddMeterValue(impectValue.meterType, impectValue.amount * this.CurrentHitpoints);
                        break;
                     * */
                    //case ImpactType.

                    default:
                        break;
                }
            }
        }

        public bool IsDockable(Agent player)
        {
            return player.FactionType == FactionType && (Inventory != null || ItemSlotsContainer != null);
        }

        public override string GetInteractionTooltip(GameEngine gameEngine, Agent player)
        {
            if (InteractionSystem != null)
                return InteractionSystem.GetInteractionText(this, gameEngine, player);
            if (this == gameEngine.PlayerAgent)
                return string.Empty;
            if (IsDockable(player))
                return "Dock";
            return null;
        }

        public override bool Interact(GameEngine gameEngine, Agent player)
        {
            if (InteractionSystem != null)
                return InteractionSystem.Interact(this, gameEngine, player);
            if (IsDockable(player)) //Refactor, can be moved to default interaction system
            {
                gameEngine.Scene.SceneComponentSelector.SwitchActivity(Framework.Scenes.SceneComponentType.Inventory);
                return true;
            }
            return false;
        }

        public override bool IsSwitchable()
        {
            return IsControllable() && FactionType == FactionType.Player && control != null && control.ControlType != AgentControlType.Player;
        }

        public override void Update(GameEngine gameEngine) {
            
            if (!IsNotActive)  
            {                
                colideTimer++;
                
                //for speed improvments //TODO: remove?,don't like it
                headingVec.X = (float)Math.Cos(Rotation);
                headingVec.Y = (float)Math.Sin(Rotation);

                if (targetSelector != null)
                    targetSelector.Update(gameEngine, this);

                Velocity += appliedForce / _mass;
#if DEBUG
                if (float.IsNaN(appliedForce.X))
                    throw new Exception();
#endif
                appliedForce = Vector2.Zero;
                appliedForceBeforeReductions = Vector2.Zero;

                Position += Velocity;
                RotationSpeed += applayedRotetionalForce / RotationMass;
                applayedRotetionalForce = 0;
                Rotation += RotationSpeed;
                RotationSpeed *= RotationInertia;

                controlSignals = 0;                                       
                

                if (colideTimer == 1)
                    controlSignals |= ControlSignals.OnColision;

                if (this.GetMeterValue(MeterType.Hitpoints) <= 0) {
                    if (lastDamagingObjectToCollide != null)
                        lastDamagingObjectToCollide.AddToKills();
                    this.IsNotActive = true;
                    controlSignals |= ControlSignals.OnDestroyed;
                }
                float damage = this.GetMeterValue(MeterType.Damage);


                if (damage > 0)
                    lifetimeWhenTakingDamage = Lifetime;

                if (Lifetime - lifetimeWhenTakingDamage < 600) //TODO: possible remove and remove OnCombat flag
                {
                    controlSignals |= ControlSignals.OnCombat;
                }

                damageTaken = damage;
                //taking damage
                if (damage > 0) {                  
                    controlSignals |= ControlSignals.OnTakingDamage;

                    //Systems.Do(s => s.OnDamageSustained(damage));

                    float shiledValue = GetMeterValue(MeterType.Shield);
                    if (shiledValue > 0) {
                        AddMeterValue(MeterType.Shield, -damage);
                        damage = MathHelper.Max(damage - shiledValue, 0);
                        controlSignals |= ControlSignals.OnDamageToShield;
                    }

                    Meter armorMeter = this.GetMeter(MeterType.Armor);
                    if (armorMeter != null) {
                        damage = MathHelper.Max(damage - armorMeter.Value, 0); //TODO: add ControlSignals OnArmorHit ?
                    }

                    SetMeterValue(MeterType.Damage, damage);
                    this.GetMeter(MeterType.Hitpoints).AddValue(-damage);

                    //add to damage Hull and ?shiled damage
                    if (damage > 0) {
                        controlSignals |= ControlSignals.OnDamageToHull;
                    }
                }

                Meter energyMeter = this.GetMeter(MeterType.Energy); //ToDo move to a system?

                if (energyMeter != null && energyMeter.NormalizedValue < 0.3f) {
                    controlSignals |= ControlSignals.OnLowEnergy;
                }


                float normalizedHitpoints = GetMeter(MeterType.Hitpoints).NormalizedValue;
        
                if (normalizedHitpoints < 1f)
                {
                    controlSignals |= ControlSignals.OnHitpointsNotFull;
                    if (normalizedHitpoints < 0.4f)
                    {
                        controlSignals |= ControlSignals.OnLowHitpoints;
                    }
                }

                if (Inventory != null && !Inventory.IsInventoryFull())
                {
                    controlSignals |= ControlSignals.OnInventoryHasRoom;
                }

                //Maybe control update here
                ControlSignals inputSignals = ControlSignals.None;
                if (control != null)
                    inputSignals = control.Update(this, gameEngine, ref analogDiractions);

                if (GetMeterValue(MeterType.StunTime) > 0) //Stun
                {
                    inputSignals = 0;
                    analogDiractions[0] = Vector2.Zero;
                    analogDiractions[1] = Vector2.Zero;
                    AddMeterValue(MeterType.StunTime, -1);
                    controlSignals |= ControlSignals.OnStun;
                }

                //IsCloaked = GetMeterValue(MeterType.Cloak) > 0;

                controlSignals |= inputSignals;

               

                if ((controlSignals & ControlSignals.Brake) > 0)
                    Velocity *= 0.6f; //VelocityInertia;

                
                Velocity *= VelocityInertia;

                controlSignals |= ControlSignals.AlwaysOn;

                foreach (AgentSystem system in _slotlessSystems) {
                    system.Update(this, gameEngine, Position, Rotation);
                }
                if (ItemSlotsContainer != null)
                    ItemSlotsContainer.UpdateBasicSlots(gameEngine, this);
                //for speed improvments
                headingVec.X = (float)Math.Cos(Rotation);
                headingVec.Y = (float)Math.Sin(Rotation);

                if (ItemSlotsContainer != null)
                    ItemSlotsContainer.UpdateBodySlots(gameEngine, this);

                if (Inventory != null)
                    Inventory.Update(gameEngine, this);

                if (_slotlessAfterSystems != null) {
                    foreach (var system in _slotlessAfterSystems) {
                        system.Update(this, gameEngine, this.Position, this.Rotation);
                    }
                }

                this.SetMeterValue(MeterType.Damage, 0);
                Lifetime++;

                ////for speed improvments
                //headingVec.X = (float)Math.Cos(Rotation); //Before the draw ?
                //headingVec.Y = (float)Math.Sin(Rotation);

                minDisFromDanger = 1000000; //??? to add a param
                closestDanger = null;
                //if (control.controlAi != null)
                //    Name = control.controlAi.GetType().ToString();
            }
        }

        public override void Draw(Camera camera) {
            ////for speed improvments///Very bad
            headingVec.X = (float)Math.Cos(Rotation);
            headingVec.Y = (float)Math.Sin(Rotation);

            //foreach (AgentSystem system in _slotlessAfterSystems)
            //    system.Draw(camera, this, Position, Rotation, DrawType);

            if (!IsCloaked)
            {
                if (draw == null)
                {
                    // if(Sprite != null)
                    camera.CameraDraw(Sprite, Position, Rotation, 1f, Color.White);
                }
                else
                    draw.Draw(this, camera);

                foreach (AgentSystem system in _slotlessSystems)
                    system.Draw(camera, this, Position, Rotation, DrawType);

                if (ItemSlotsContainer != null)
                    ItemSlotsContainer.Draw(camera, this, DrawType); // note that we're disregarding our item slots' DrawType when deciding whether to use lighting
                                                                     // (think we were already disregarding it before)

            }
            else
                camera.CameraDraw(Sprite, Position, Rotation, 1.1f, new Color(0, 0, 0, 100));
        }

        public void DrawInGui(SpriteBatch sb, Rectangle rectangle)
        {
            int width = Sprite.Width; ;
            int height = Sprite.Height; 
            float scale = FMath.FindScaleFitToBox(Sprite.Width, Sprite.Height, rectangle.Width, rectangle.Height) * 0.9f;
            Vector2 pos = FMath.GetMidPoint(rectangle);
            float rotation = 0;
            if ((gameObjectType & GameObjectType.NonRotating) == 0)
                rotation = -MathHelper.PiOver2;

            sb.Draw(Sprite.Texture, pos, null, Color.White, rotation, Sprite.Size * 0.5f, scale, SpriteEffects.None, 0);


            if (ItemSlotsContainer != null)
                ItemSlotsContainer.DrawInGUI(sb, pos, scale, rotation);
        }
       

        public override void DrawOnMap(Camera camera) {
            throw new NotImplementedException();
        }

        public override void SetControlType(AgentControlType controlType) {
            if(control != null)
                control.ControlType = controlType;
        }

        public override AgentControlType GetControlType() {
            if (control == null)
                return AgentControlType.None;
            return control.ControlType;
        }

        public override Sprite GetSprite() {
            return Sprite;
        }

        public override CollisionType ListType {
            get {
                return listType;
            }
            set {
                listType = value;
            }
        }

        public Vector2 Heading {
            get { return this.headingVec; }
        }

        public override bool IsCloaked { get { return GetMeterValue(MeterType.Cloak) > 0; } }

        public override bool IsVisible(GameObject observer) {
            return !IsCloaked;// || ((controlSignals & ControlSignals.OnTakingDamage) > 0);
        }

        public ControlSignals ControlSignal {
            get { return this.controlSignals; }
            set { this.controlSignals = value; }
        }

        /* public List<PresistentEffect> PresistentEffects
         {
             get { return _presistentEffects; }
             set { _presistentEffects = value; }
         }*/

        public float CurrentHitpoints {
            get { return GetMeterValue(MeterType.Hitpoints); }
            set { SetMeterValue(MeterType.Hitpoints, value); }
        }

        public float MaxHitpoints {
            get { return GetMeter(MeterType.Hitpoints).MaxValue; }
            set { GetMeter(MeterType.Hitpoints).MaxValue = value; }
        }

        public void ResetSystems() {
            foreach (var s in Systems)
                s.Reset();
        }

        public IEnumerable<AgentSystem> Systems {
            get {
                var itemSlots = ItemSlotsContainer ?? new ItemSlotsContainer();

                foreach (var s in _slotlessSystems.Concat(_slotlessAfterSystems)
                    .Concat(itemSlots.Select(slot => slot?.Item?.System)
                    .Where(system => system != null)))
                    yield return s;
            }
        }

        /* public override float Mass
         {
             get { return this.mass; }
             set { this.mass = value; }
         }*/





        public override CollisionSpec CollisionInfo {
            get {
                return impactSpec;
            }
            set {
                impactSpec = value;
            }
        }

        //public override void Illuminate(IEnumerable<ILight> lights) {
        //    // Illuminate chassis
        //    base.Illuminate(lights);

        //    // Now illuminate systems            
        //    ItemSlotsContainer?.Illuminate(Sprite.IncomingLights);
        //}

        public void AddMeterValue(MeterType type, float value) {
            if (meters.ContainsKey(type)) {
                meters[type].AddValue(value);
            } else {
                meters.Add(type, new Meter(value, float.MaxValue));
            }
        }

        public void MaxMeterValue(MeterType type, float value) {
            if (meters.ContainsKey(type)) {
                meters[type].SetValue(Math.Max(meters[type].Value, value));
            } else {
                meters.Add(type, new Meter(Math.Max(value, 0), float.MaxValue));
            }
        }


        public void MinMeterValue(MeterType type, float value) {
            if (meters.ContainsKey(type)) {
                meters[type].SetValue(Math.Min(meters[type].Value, value));
            } else {
                //meters.Add(type, new Meter(Math.Max(value, 0), float.MaxValue));
            }
        }

        public void MultMeterValue(MeterType type, float value) {
            if (meters.ContainsKey(type)) {
                meters[type].SetValue(meters[type].Value * value);
            } else {
                //meters.Add(type, new Meter(Math.Max(value, 0), float.MaxValue));
            }
        }


        public void SetMeterMaxValue(MeterType type, float maxValue) //???
        {
            if (meters.ContainsKey(type)) {
                meters[type].MaxValue = maxValue;
            } else {
                meters.Add(type, new Meter(0, maxValue));
            }
        }

        public override void SetMeterValue(MeterType type, float value) {
            if (meters.ContainsKey(type)) {
                meters[type].SetValue(value);
            } else {
                meters.Add(type, new Meter(value, float.MaxValue));
            }
        }

        public override float GetMeterValue(MeterType type) {
            if (meters.ContainsKey(type)) {
                return meters[type].Value;
            }
            return 0;
        }

        public override Meter GetMeter(MeterType type) {
            Meter meter;
            meters.TryGetValue(type, out meter);
            return meter;
        }

        public void SetMeter(MeterType type, Meter meter) {
            meters[type] = meter;
            //if (meters.ContainsKey(type)) {
            //    meters[type] = meter;
            //} else {
            //    meters.Add(type, meter);
            //}
        }

        private float damageTaken;
        public override float GetDamageTaken() {
            return damageTaken;
        }

        //public Faction GetFaction() { //NOt good
        //    return MetaWorld.Inst.GetFaction(FactionType);
        //}

        public override FactionType GetFactionType() {
            return FactionType;
        }

        public override void SetFactionType(FactionType faction) {
            this.FactionType = faction;
        }



        public override GameObject GetTarget(GameEngine gameEngine, TargetType targetType) {
            //TODO: ? check if targetSelector ! null??
            return targetSelector.GetTarget(targetType);
        }

        public override void CheckClosest(GameObject otherGameObject, float disSquared) {
            //maybe slow, also faction check
            //&& otherGameObject.GetFaction() != this.faction
            if (otherGameObject.Parent != this && otherGameObject.CollisionInfo.IsDamaging && minDisFromDanger > disSquared - otherGameObject.Size * otherGameObject.Size) {
                closestDanger = otherGameObject;
                minDisFromDanger = disSquared - otherGameObject.Size * otherGameObject.Size;
            }
        }

        public Vector2 RotateVector(Vector2 inVector) {
            return new Vector2(inVector.X * this.Heading.X - inVector.Y * this.Heading.Y, inVector.X * this.Heading.Y + inVector.Y * this.Heading.X);
        }

        public override bool AddItemToInventory(Item item) 
        {
            if (item == null) 
                return true;
            if(Inventory != null)
                return Inventory.AddItem(item);
            return
                false;
            //if (Inventory != null) {

            //    if (item.Profile.Category == ItemCategory.Material)
            //    {
            //        return Inventory.AddItemFromEnd(item);
            //    }
            //    else
            //    {
            //        return Inventory.AddItem(item);
            //    }
            //}
            //return false;
        }

        

        //public override bool IsPotentialTarget() {
        //    return (gameObjectType & GameObjectType.PotentialTarget) > 0;
        //}

        public override GameObject GetAgentAncestor() {            
            return this;
        }
        
        public override void AddToKills() {
            AddMeterValue(MeterType.Kills, 1);
            AddMeterValue(MeterType.FactionKills, 1);
            //Reputation 
        }

        public override float GetHitpoints() {
            return CurrentHitpoints;
        }

        public GameObject GetIdleTarget() {
            return null;// //targetSelector.farAlly;
        }

        public override GameObjectType GetObjectType() {
            return gameObjectType;
        }

        public override Inventory GetInventory() {
            return Inventory; //change name to inventory
        }

        public override string GetId() {
            return ID;
        }     

        public override void SetAggroRange(float aggroRange, float aggroLostRange, TargetType targetType) {
            if (targetSelector != null) {
                targetSelector.SetAggroRange(aggroRange, aggroLostRange, targetType);
            }
        }

        public override void SetAggroRange(float aggroRange, TargetType targetType) {
            if (targetSelector != null) {
                targetSelector.SetAggroRange(aggroRange, aggroRange * 1.5f, targetType);
            }
        }

        public override void SetTarget(GameObject target, TargetType targetType) {
            if (targetSelector != null) //TODO: maybe all agent will have targetSelector?
            {
                targetSelector.SetTarget(target, targetType);
            }
        }

        public override DrawType DrawType {
            get {
                return _drawType;
            }

            set {
                _drawType = value;
            }
        }

        public Agent GetNewCopy() {
            Agent agentClone = GetWorkingCopy();

            agentClone.Revive();
            //agentClone.SetMeterValue(MeterType.RespawnTimer, 0);
            return agentClone;
        }

        /// <summary>Returns agent to life, if dead, and resets most stats to factory conditions</summary>
        public void Revive()
        {
            IsActive = true;
            ResetSystems();
            SetMeterValue(MeterType.Hitpoints, GetMeter(MeterType.Hitpoints).MaxValue);
            SetMeterValue(MeterType.Energy, GetMeter(MeterType.Energy)?.MaxValue ?? 0);
            SetMeterValue(MeterType.Shield, GetMeter(MeterType.Shield)?.MaxValue ?? 0);
        }


        public Agent GetWorkingCopy() {
            Agent agentClone = (Agent)MemberwiseClone();
           // agentClone.Sprite = Sprite?.Clone() as Sprite;
           // agentClone.Lights = Lights.Select(l => l.Clone() as ILight).ToList();

            if (this.Inventory != null)
                agentClone.Inventory = this.Inventory.GetWorkingCopy();

            agentClone.analogDiractions = new Vector2[2];
            for (int i = 0; i < analogDiractions.Length; i++) {
                agentClone.analogDiractions[i] = Vector2.UnitX;
            }


            //agentClone.draw = this.draw.
            if (this.ItemSlotsContainer != null)
                agentClone.ItemSlotsContainer = this.ItemSlotsContainer.GetWorkingCopy();
            agentClone.meters = new Dictionary<MeterType, Meter>();
            foreach (var item in this.meters) {
                agentClone.meters.Add(item.Key, item.Value.GetWorkingCopy());
            }
            agentClone._slotlessSystems = new List<AgentSystem>();
            foreach (var item in this._slotlessSystems) {
                agentClone._slotlessSystems.Add(item.GetWorkingCopy());
            }

            agentClone._slotlessAfterSystems = new List<AgentSystem>();
            if (_slotlessAfterSystems != null)
            {
                agentClone._slotlessAfterSystems = new List<AgentSystem>();
                foreach (var item in this._slotlessAfterSystems)
                {
                    agentClone._slotlessAfterSystems.Add(item.GetWorkingCopy());
                }
            }

            if (this.Inventory != null)
                agentClone.Inventory = this.Inventory.GetWorkingCopy();

            agentClone.targetSelector = this.targetSelector.GetWorkingCopy();

            agentClone.control = this.control?.GetWorkingCopy(); //fix

            return agentClone;
        }


        public GameObject MakeGameObject(GameEngine gameEngine,GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
           int maxLifetime = 0, float? size = null, Color? color = null, float param = 0) {
            Agent clone = this.GetWorkingCopy();
            clone.Parent = parent;
            clone.FactionType = faction;
            clone.Position = refPosition;
            clone.Velocity = refVelocity;
            clone.Rotation = refRotation;
            clone.RotationSpeed = refRotationSpeed;
            clone.Param = param;
            return clone;
        }

        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent = null, FactionType faction = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0) {
            return MakeGameObject(gameEngine, parent, faction, Vector2.Zero, Vector2.Zero, 0, 0, maxLifetime, size, color, param);
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
           int maxLifetime = 0, float? size = null, Color? color = null, float param = 0) {
            Agent clone = this.GetWorkingCopy();
            clone.Parent = parent;
            clone.FactionType = faction;
            clone.Position = refPosition;
            clone.Velocity = refVelocity;
            clone.Rotation = refRotation;
            clone.RotationSpeed = refRotationSpeed;
            clone.Param = param;
            gameEngine.AddList.Add(clone);
            return clone;
        }

        public override GameObject GetLastObjectToCollide() {
            return lastDamagingObjectToCollide;
        }

        public void SetIsControllable(bool value = true) {
            isControllable = value;
        }

        public override bool IsControllable() {
            return isControllable;
        }

        public override IEnumerable<GameObject> GetSelfLights(GameEngine gameEngine)
        {
            foreach (var slot in ItemSlotsContainer)
            {
                LightObject light = slot.Item?.System?.GetSelfLights(gameEngine);
                if (light != null)
                    yield return light;
            }
        }


        #region persistence
        /// <summary>Save the Agent in isolation (removes any references to stuff extrinsic to the agent, e.g. other GameObjects in the scene)</summary>
        /// <remarks>Should not be used when serializing an entire Scene, in which we'd want to retain references to extrinsic stuff.</remarks>        
        public void Save(Stream target) {
            //var closestDanger_ = closestDanger;
            closestDanger = null;
            //var lastObjectToCollide_ = lastDamagingObjectToCollide;
            lastDamagingObjectToCollide = null;
            //var targetSelector_ = targetSelector;
           // targetSelector = null;

            new BinaryFormatter().Serialize(target, this);
            target.Close();

            //closestDanger = closestDanger_;       
        }

        //public static Agent Load(Stream source) {
        //    var result = new BinaryFormatter().Deserialize(source) as Agent;

        //    result.targetSelector = new TargetSelector();

        //    return result;
        //}
        #endregion

        /// <summary>
        /// Gets the current hitpoints and shield total hitpoints
        /// </summary>
        /// <returns></returns>
        public float GetTotalHitpoints()
        {
            float ans = CurrentHitpoints; ;
            var shield = GetMeter(MeterType.Shield);
            if (shield != null)
                ans += shield.Value;
            return ans;
        }

        /// <summary>
        /// returens the cost of the hull + items in slots
        /// </summary>
        /// <returns></returns>
        public float GetCost()
        {
            float cost = HullCost;
            if(ItemSlotsContainer != null)
            {
                foreach (var itemSlot in ItemSlotsContainer)
                {                    
                    if(itemSlot.Item != null)
                    {
                        cost += itemSlot.Item.GetStackBuyPrice();
                    }
                }
            }
            return cost;
        }
       
        public override CraftingStationType GetCraftingStationType() //Kobi: Check
        {
            CraftingStationType res = CraftingStationType;

            if (Inventory != null)
            {
                foreach (var item in Inventory.Items)
                {
                    if (item != null)
                    {
                        res |= item.GetCraftingStationType();
                    }
                }
            }
            if(ItemSlotsContainer != null)
            {
                foreach (var slot in ItemSlotsContainer)
                {
                    if(slot.Item != null)
                    {
                        res |= slot.Item.GetCraftingStationType();
                    }
                }
            }
            return res;
        }

        public void AddItemToSlot(Item item)
        {
            foreach (var slot in ItemSlotsContainer)
            {
                if(slot.Item == null && slot.CanEquip(item))
                {
                    slot.Item = item;
                    break;
                }
            }
        }

        public void AddItemToSlots(string id)
        {
            AddItemToSlot(ContentBank.Inst.GetItem(id,true));
        }
       
        public void AddItemToInventory(string id, int stack = 0)
        {
            if(stack == 0)
                AddItemToInventory(ContentBank.Inst.GetItem(id, true));
            else
            {
                var item = ContentBank.Inst.GetItem(id, true);
                item.Stack = stack;
                AddItemToInventory(item);
            }

        }

        public override bool IsHostileToPlayer(GameEngine gameEngine)
        {
            return gameEngine.GetFaction(FactionType).GetRelationToFaction(FactionType.Player) < 0;
            //return targetSelector?.GetTarget(TargetType.Enemy)?.GetFactionType() == FactionType.Player;
        }

        public override GameObjectType GetCollideWithMask()
        {
            return collideWithMask;
        }

        public float GetNormalizedHitpoints()
        {
            float currentHitpoints = CurrentHitpoints;
            float totalHitpoints = MaxHitpoints;
            Meter shield = GetMeter(MeterType.Shield);
            if( shield  != null )
            {
                currentHitpoints += shield.Value;
                totalHitpoints += shield.MaxValue;
            }
            return currentHitpoints / totalHitpoints;
        }

        /// <summary>
        /// Automaticly equips items from inventory
        /// </summary>
        /// <param name="inventoryList"></param>
        public void AutoEquip(List<Inventory> inventoryList, bool isEmpty = false)
        {
            foreach (var inv in inventoryList)
            {
                for (int i = 0; i < inv.Size; i++)
                {
                    Item item = inv.GetItem(i);
                    if (item != null && item.Stack >= 1)
                    {
                        int? index = ItemSlotsContainer.FindItemSlot(item, isEmpty);
                        if (index.HasValue)
                        {
                            var tmpItem = ItemSlotsContainer[index.Value].Item;
                            ItemSlotsContainer[index.Value].Item = item;
                            inv.SetItem(i, tmpItem);
                        }
                    }
                }
            }

            
        }

        public static void EquipAgent(Agent agent, int level, bool makeAI) //Add option to permute items
        {
            if (agent == null)
                return;
            var items = ContentBank.Inst.GetAllItems().Where(i => i.Level == level).OrderBy(i => i.SlotType);
            foreach (var slot in agent.ItemSlotsContainer)
            {
                foreach (var item in items)
                {
                    if(slot.CanEquip(item) && slot.Item == null)
                    {
                        slot.Item = item.GetWorkingCopy();
                        break;
                    }
                }
            }
            if(makeAI)
            {
                agent.control.controlAi = ParameterControl.MakeAIFromAgent(agent);                
            }
        }

        public Character GetCharacter(Scene scene)
        {
            int chIndex = (int)GetMeterValue(MeterType.Character);
            if ( chIndex == 0)
            {
                var characterSystem = GetSystem<CharacterSystem>();
                if (characterSystem != null)
                    return characterSystem.Character;
            }
            else
            {
                return ContentBank.Inst.CharacterBank.Get(chIndex);
            }
            return ContentBank.Inst.CharacterBank.GetCharacterFromHash(this.GetHashCode());
        }

        public override float GetNormlizedHitpointsAndShield()
        {
            float ans = CurrentHitpoints;
            float maxValue = MaxHitpoints;
            var shield = GetMeter(MeterType.Shield);
            if (shield != null)
            {
                maxValue += shield.MaxValue;
                ans += shield.Value;
            }
            return ans / maxValue;
        }

       //public AutoEqup
    }
}
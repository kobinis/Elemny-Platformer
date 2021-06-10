using Microsoft.Xna.Framework;
using System;
using SolarConflict.Framework;
using XnaUtils;
using XnaUtils.Graphics;
using System.Collections.Generic;
using System.Linq;
using SolarConflict.Framework.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict 
{

    public enum GameObjectFlags
    {
        None = 0,
        /// <summary>
        /// Only Add gameobject if it is on screen
        /// </summary>
        AddOnlyOnScreen = 1 << 1, 
        IsOffScreen = 1 << 2,
        CollideOnlyOnScreen = 1 << 3,
        UpdateOnlyOnScreen = 1 << 4,        
    }

    //public struct StatMult
    //{
    //    float Damage;
    //    float Speed;
    //    float EngineForce;
    //    float ShieldCapacity;
    //    //float ShiledGenegeration;
    //    float Hitpoints;
    //}

    /// <summary>
    /// Game Object Class - base class for all objects in the game, representing physical objects or visual effect
    /// </summary>
    [Serializable]
    public abstract class GameObject
    {
        #region Fields           
        public virtual PointLight Light
        {
            get { return null; }
            set { throw new Exception(); }
        }
        public virtual int Level {
            get {
                return 0;
            }
            set {
                throw new Exception("Level property setter is abstract");
            }
        }

        public abstract float Mass { get; set; }
        public GameObjectFlags Flags;
        public GameObject Parent;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public float RotationSpeed;
        public int Lifetime;
        public bool IsNotActive;
        public float Size;
        public float Param; //TODO: remove 
        #endregion        

        #region Properties        
        public abstract string Name
        {
            get;
            set;
        }

        public abstract string Tag
        {
            get;
        }

        /// <summary>
        /// Gets a descriptive string in a rich text formmat (may include color and image)
        /// </summary>
       // public abstract string Tag { get; }

        public abstract CollisionSpec CollisionInfo //change name to collisionInfo
        {
            get;
            set;
        }        

        public virtual CollisionType ListType
        {
            get { return CollisionType.CollideAll; }
            set { }
        }

        public virtual DrawType DrawType
        {
            get { return DrawType.Alpha; }
            set { }
        }
        
        public virtual bool IsCloaked
        {
            get { return false; }
        }

        public virtual bool IsVisible(GameObject observer)
        {
            return !IsCloaked;
        }

        public bool IsActive //REfactor:maybe remove
        {
            get { return !IsNotActive; }
            set { IsNotActive = !value; }
        }

        #endregion

        #region Public Methods         
        
        public abstract string GetId(); //remove

        public virtual string GetInteractionTooltip(GameEngine gameEngine, Agent player) { return null; }
        public virtual bool Interact(GameEngine gameEngine, Agent player) { return false; }

        public abstract Sprite GetSprite();
       
        public abstract void ApplyCollision(GameObject collidingObject, GameEngine gameEngine);

        public abstract void ApplyForce(Vector2 force, float speedLimit);

        public virtual FactionType GetFactionType() 
        {
            return FactionType.Neutral;
        }

        public virtual void SetFactionType(FactionType faction)
        {
        }


        public virtual bool AddItemToInventory(Item item) // returns false if unable to add the item
        {
            return false;
        }

        public virtual CraftingStationType GetCraftingStationType()
        {
            return CraftingStationType.None;
        }

        public abstract GameObjectType GetObjectType();

        public virtual GameObjectType GetCollideWithMask()
        {
            return GameObjectType.All;
        }

        public virtual float GetNormlizedHitpointsAndShield()
        {
            return 1;
        }
                
           

        #endregion

        #region Update/Draw
        public abstract void Update(GameEngine gameEngine);
        public virtual void Draw(Camera camera)
        {
            //camera.CameraCircle(Position, Size , DebugUtils.DebugColor);
        }

        public virtual void DrawOnMap(Camera camera) 
        {            
        }
        #endregion

        #region Private Methods
        #endregion

        public virtual bool IsHostileToPlayer(GameEngine gameEngine)
        {
            return gameEngine.GetFaction(GetFactionType()).GetRelationToFaction(FactionType.Player) < 0;
        }

        public virtual float NormalizedLifetime()
        {
            return 0;
        }

        /// <summary>Should return true if player Faction ships will aggro on the object, and the object appears hostile in the
        /// player's UI</summary>
        public virtual bool IsPlayerHostile(GameEngine gameEngine)
        {
            return gameEngine.GetFaction(FactionType.Player).GetRelationToFaction(GetFactionType()) <= 0;
        }

        public virtual float GetDamageTaken()
        {
            return 0;
        } 

        public virtual void CheckClosest(GameObject otherGameObject, float disSquared)
        {
        }
 
        /// <summary>
        /// Returns the first agent encountered in the parent chain (if called on agent returns self)
        /// </summary>        
        public abstract GameObject GetAgentAncestor(); //TODO: can be implemented here 

        /// <summary>
        /// Returns the oldest ancestor in the parent chain (the one without a parent)
        /// </summary>        
        public GameObject GetLivePrimeAncestor()
        {
            if (Parent == null || Parent.IsNotActive)
                return this; //Maybe return null
            else
                return Parent.GetLivePrimeAncestor();
        }

        public GameObject GetPrimeAncestor()
        {
            if (Parent == null)
                return this;
            else
                return Parent.GetPrimeAncestor();
        }

        public abstract float GetMeterValue(MeterType type);
        public abstract void SetMeterValue(MeterType type, float value);

        /// <summary>
        /// Gets GetAggro Priority - if zero it won't add it
        /// </summary>
        /// <returns></returns>
        public virtual float GetAggroPriority()
        {
            return 1;
        }     

        public virtual void SetControlType(AgentControlType controlType)
        {
        
        }

        public virtual AgentControlType GetControlType()
        {
            return AgentControlType.None;
        }

        public virtual void AddToKills()
        {
        }

        public virtual float GetHitpoints()
        {
            return 0;
        }

        public virtual Meter GetMeter(MeterType type)
        {
            return null;
        }

        public virtual void SetColor(Color? color) //To Do
        {            
        }

        public virtual Color GetColor()
        {
            return Color.White;
        }

        public virtual Inventory GetInventory()
        {
            return null;
        }

        public virtual bool EquipLoadout(AgentLoadout loadout)
        {            
            return false;
        }

        public abstract GameObject GetTarget(GameEngine gameEngine, TargetType targetType);

        public virtual void SetTarget(GameObject target, TargetType targetType)
        {
        }

     
        public virtual void SetAggroRange(float aggroRange, float aggroLostRange, TargetType targetType)
        {
        }

        public virtual void SetAggroRange(float aggroRange, TargetType targetType)
        {
        }

        /// <summary>Necessary but insufficient condition for player control of a GameObject</summary>
        public virtual bool IsControllable()
        {            
            return false;
        }

        /// <summary>True if the player can switch to this object right now (i.e. it's not already under player
        /// control etc)</summary>
        public virtual bool IsSwitchable()
        {
            return false;
        }

        public virtual GameObject GetLastObjectToCollide()
        {
            return null;
        }


        public static bool CheckCollision(GameObject obj1, GameObject obj2)
        {
            if ((obj1.GetCollideWithMask() & obj2.GetObjectType()) > 0 && (obj2.GetCollideWithMask() & obj1.GetObjectType()) > 0)
            {
                // CALCULATE SQR DISDANSE
                float disSquared = (obj1.Position - obj2.Position).LengthSquared();
                bool boundingCir = (disSquared <= (obj1.Size + obj2.Size) * (obj1.Size + obj2.Size));
                obj1.CheckClosest(obj2, disSquared);
                obj2.CheckClosest(obj1, disSquared); 
                return boundingCir;
            }
            return false;           
        }
        
        public virtual float GetDisplayScale()
        {
            return 1;
        }
                      
        public static float DistanceFromEdge(Vector2 position1, Vector2 position2, float size1 = 0, float size2 = 0)
        {
            return (position1 - position2).Length() - size1 - size2;
        }

        public static float DistanceFromEdge(GameObject obj1, GameObject obj2)
        {
            return (obj1.Position - obj2.Position).Length() - obj1.Size - obj2.Size;
        }

        public virtual EffectTechnique GetEffectTechnique()
        {
            return null; // Or Default
        }

        public virtual IEnumerable<GameObject> GetSelfLights(GameEngine gameEngine)
        {
            return null;
        }

        //public virtual Vector2 GetAnalogDirection(int index)
        //{
        //    return Vector2.Zero;
        //}

    }
}

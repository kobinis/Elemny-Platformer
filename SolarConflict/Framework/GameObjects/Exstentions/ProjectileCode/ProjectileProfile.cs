using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using XnaUtils.Graphics;

namespace SolarConflict
{
    [Serializable]
    public class ProjectileProfile : IGameObjectFactory, IEmitter
    {
        public enum FactionLogicType { Parent, Const, Param}
        #region Proprieties        
        public string ID { get; set; }
        public GameObjectFlags Flags;
        public GameObjectType CollideWithMask { get; set; }
        public PointLight Light { get; set; }
        public GameObjectType ObjectType { get; set; }
        public List<BaseUpdate> UpdateList { get; set; }
        public List<BaseImpactUpdate> CollusionUpdateList { get; set; }
        public List<BaseDraw> DrawList { get; set; }      
        public float VelocityInertia { get; set; } //   0 - stop moving when no force is applied. 1 - keeps moving
        public float RotationInertia { get; set; } // 0 - stop rotating when no force is applied. 1 - keeps rotating
        //public bool IsPotentialTarget { get; set; }
        public float AggroRange { get; set; }
        public CraftingStationType CraftingStationType { get; set; }
        public String Name = null;
        public int Level;
        public FactionLogicType FactionLogic;
        public FactionType FactionType;

        /// <summary>
        /// Define cooldown time for an emit action for update emitter (emitter that called on update). For example: shot trail.
        /// </summary>
        public int UpdateEmitterCooldownTime { get; set; }

        /// <summary>
        /// Define cooldown time for an emit action for impact emitter (emitter that called on impact)
        /// </summary>
        public int ImpactEmitterCooldownTime { get; set; }

        /// <summary>
        /// Define the collusion type of the projectile
        /// </summary>
        public CollisionType CollisionType { get; set; }

        /// <summary>
        /// Define the way we draw the projectile.
        /// </summary>
        public DrawType DrawType { get; set; }

        /// <summary>
        /// Is the projectile velocity affected when colliding with other game objects.
        /// </summary>
        public bool IsEffectedByForce { get; set; }

        /// <summary>
        /// Is the rotation velocity affected when colliding with other game objects.
        /// </summary>
        public bool IsTurnedByForce { get; set; }

        /// <summary>
        /// Is the projectile destroyed when colliding with other game objects.
        /// </summary>
        public bool IsDestroyedOnCollision { get; set; }

        /// <summary>
        /// Is the projectile destroyed when his parent (the game object that emitted him) is destroyed.
        /// </summary>
        public bool IsDestroyedWhenParentDestroyed { get; set; }

        public float Mass { get; set; } // TODO: maybe add mass type or mass class so mass can be related to size

        /// <summary>
        /// Moment of inertia - see WIKI
        /// </summary>
        public float RotationMass { get; set; }

        [XmlIgnore]
        public float ScaleMult { get; set; }

        [XmlElement(Type = typeof(InitColorConst)), XmlElement(Type = typeof(InitColorFaction))]
        public BaseInitColor InitColor { get; set; }

        //[XmlElement(Type = typeof(UpdateMovementForward)), XmlElement(Type = typeof(MoveWithParent))]
        public BaseUpdate MovementLogic { get; set; }

        /// <summary>
        /// Updates rotation and rotation Speed
        /// </summary>
        public BaseUpdate RotationLogic { get; set; }
        public BaseUpdate UpdateSize { get; set; }
        public CollisionSpec CollisionSpec { get; set; } 
        public BaseDraw Draw { get; set; }

        #endregion


        #region Proprieties with ID

        public Sprite Sprite;

        public string TextureID
        {
            get { return Sprite.ID; }

            set
            {
                Sprite = Sprite.Get(value);
                if (Sprite != null)
                    CollisionWidth = Sprite.Width;
            }
        }

        [XmlIgnore]
        public BaseInitFloat InitMaxLifetime { get; set; } // TODO: can be removed
        public string InitMaxLifetimeID
        {
            get { return BaseInitFloat.GetInitFloatParam(InitMaxLifetime); }
            set { InitMaxLifetime = BaseInitFloat.Factory(value); }
        }


        [XmlIgnore]
        public BaseInitFloat InitSize { get; set; }

        /// <summary>
        /// The initial size of the projectile.
        /// </summary>
        public string InitSizeID // REFACTOR: consider refactoring the way we use this property, maybe use a class, or split to few fields
        {
            get { return BaseInitFloat.GetInitFloatParam(InitSize); }
            set { InitSize = BaseInitFloat.Factory(value); }
        }


        public BaseInitFloat InitHitPoints { get; set; } // TODO: can be removed
        public string InitHitPointsID
        {
            get { return BaseInitFloat.GetInitFloatParam(InitHitPoints); }
            set { InitHitPoints = BaseInitFloat.Factory(value); }
        }


        public BaseInitFloat InitParam { get; set; }
        public string InitParamID
        {
            get { return BaseInitFloat.GetInitFloatParam(InitParam); }
            set { InitParam = BaseInitFloat.Factory(value); }
        }


        [XmlIgnore]
        public IEmitter ImpactEmitter { get; set; }

        public string ImpactEmitterID
        {
            get { return GetEmitterPropertyID(ImpactEmitter); }
            set { ImpactEmitter = ContentBank.Inst.GetEmitter(value); }
        }


        [XmlIgnore]
        public IEmitter TimeOutEmitter { get; set; } // TODO: can be removed

        public string TimeOutEmitterID // REFACTOR: consider create one method to all the ID properties
        {
            get { return GetEmitterPropertyID(TimeOutEmitter); }
            set { TimeOutEmitter = ContentBank.Inst.GetEmitter(value); }
        }


        [XmlIgnore]
        public IEmitter HitPointZeroEmiiter;

        public string HitPointZeroEmiiterID
        {
            get { return GetEmitterPropertyID(HitPointZeroEmiiter); }
            set { HitPointZeroEmiiter = ContentBank.Inst.GetEmitter(value); }
        }


        [XmlIgnore]
        public IEmitter UpdateEmitter { get; set; } // TODO: null is not emitting 

        /// <summary>
        /// The emitter that will be activate every cool down update.
        /// </summary>
        public string UpdateEmitterID
        {
            get { return GetEmitterPropertyID(UpdateEmitter); }
            set { UpdateEmitter = ContentBank.Inst.GetEmitter(value); }
        }

        #endregion

        public static float WidthToScale(float width)
        {
            return 2f / width;
        }

        #region Proprieties with logic

        /// <summary>
        /// Width of the projectile for collision. //TODO: remove
        /// </summary>
        public float CollisionWidth
        {
            get
            {
                if (ScaleMult == 0)
                {
                    return 0;
                }
                else
                {
                    return 2f / ScaleMult;
                }
            }
            set
            {
                if (value == 0)
                {
                    ScaleMult = 0;
                }
                else
                {
                    ScaleMult = 2f / value;
                }
            }
        }


        public ColorUpdater _colorLogic; //ToDo: check this section *********************************************************************************
        [XmlElement(Type = typeof(UpdateColorFade)), XmlElement(Type = typeof(UpdateColorFadeInOut)), XmlElement(Type = typeof(UpdateColorSwitch))]
        public ColorUpdater ColorLogic
        {
            get
            {
                if (ColorUpdater.GetFactoryParams(_colorLogic) != null)
                {
                    return null;
                }
                else
                {
                    return _colorLogic;
                }
            }

            set { _colorLogic = value; }
        }

        public string ColorLogicID
        {
            get { return ColorUpdater.GetFactoryParams(_colorLogic); }
            set { _colorLogic = ColorUpdater.Factory(value); }
        } //********************************************************************************************************************************************

        #endregion



        #region Constructors

        public ProjectileProfile()
        {
            AggroRange = 10000;

            VelocityInertia = 1;
            RotationInertia = 1;

            IsTurnedByForce = false;
            RotationMass = 10;

            DrawList = new List<BaseDraw>();
            UpdateList = new List<BaseUpdate>();

            InitColor = null;
            InitMaxLifetime = null;
            InitSize = null;

            ScaleMult = 1;
            Draw = null;
            _colorLogic = null;
            MovementLogic = null;
            RotationLogic = null;
            UpdateSize = null;
            ImpactEmitter = null;
            TimeOutEmitter = null;
            UpdateEmitter = null;
            UpdateEmitterCooldownTime = 1;
            ImpactEmitterCooldownTime = 1;

            CollisionType = CollisionType.Collide1;
            DrawType = DrawType.AlphaFront;

            IsEffectedByForce = false;
            //turnedByForce;
            IsDestroyedOnCollision = true; //When Apply impact is called
            IsDestroyedWhenParentDestroyed = false; //When Parent is not CRIVE
            Mass = 1;


            CollisionSpec = new CollisionSpec(); //ImpactInfo.SpecEmpty; //???? //Can be both ImpectSpect and ImpectGen
            //public ImpectSpecGen 
            ID = null;

            //impactSpec.Force = 1;

            UpdateList = new List<BaseUpdate>();
            DrawList = new List<BaseDraw>();
            CollusionUpdateList = new List<BaseImpactUpdate>();
            CollisionWidth = 0;

            ObjectType = GameObjectType.Projectile;
            CollideWithMask = GameObjectType.All;
        }

        #endregion


        public virtual GameObject MakeGameObject(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
           int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Projectile projectile = new Projectile();
            projectile.Flags = Flags;
            projectile.profile = this;
            projectile.Lifetime = 0;
            projectile.Parent = parent;
            projectile.Position = refPosition;
            projectile.Velocity = refVelocity;
            projectile.Rotation = refRotation;
            projectile.RotationSpeed = refRotationSpeed;

            SetMaxLifetime(maxLifetime, projectile, gameEngine);
            SetInitColor(color, projectile, gameEngine);
            SetSize(size, projectile, gameEngine);

            if (InitHitPoints != null)
            {
                projectile.hitPoints = InitHitPoints.Init(projectile, gameEngine);
            }
            else
            {
                projectile.hitPoints = float.MaxValue / 2;
            }

            if (InitParam != null)
            {
                projectile.Param = InitParam.Init(projectile, gameEngine);
            }
            else
            {
                projectile.Param = 0;
            }
            return projectile;
        }

        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent = null, FactionType faction = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            return MakeGameObject(gameEngine, parent, faction, Vector2.Zero, Vector2.Zero, 0, 0, maxLifetime, size, color, param);
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity,
            float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            GameObject gameObject = MakeGameObject(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
            gameEngine.AddList.Add(gameObject);

            return gameObject;
        }

        private void SetSize(float? size, Projectile projectile, GameEngine gameEngine)
        {
            if (size != null)
            {
                projectile.Size = size.Value;
            }
            else
            {
                if (InitSize != null)
                {
                    projectile.Size = InitSize.Init(projectile, gameEngine);
                }
                else
                {
                    //throw (new Exception());
                    if (Sprite?.Texture != null)
                    {
                        projectile.Size = Sprite.Texture.Width / 2;
                    }
                    else
                    {
                        projectile.Size = 1;
                    }
                }
            }
        }

        private void SetInitColor(Color? color, Projectile projectile, GameEngine gameEngine)
        {
            if (InitColor != null)
            {
                projectile.ParticleColor = InitColor.Init(projectile, gameEngine);
                if (color != null)
                {
                    //TODO: maybe multiply the colors??
                    Color newColor = color.Value;
                    newColor.A = projectile.ParticleColor.A;
                    projectile.ParticleColor = newColor;
                }
            }
            else
            {
                projectile.ParticleColor = Color.White;
                if (color != null)
                {
                    projectile.ParticleColor = color.Value;
                }
            }
        }

        private void SetMaxLifetime(int maxLifetime, Projectile projectile, GameEngine gameEngine)
        {
            if (maxLifetime != 0)
            {
                projectile.MaxLifeTime = maxLifetime;
            }
            else
            {
                if (InitMaxLifetime != null)
                {
                    projectile.MaxLifeTime = InitMaxLifetime.Init(projectile, gameEngine);
                }
                else
                {
                    projectile.MaxLifeTime = float.MaxValue;
                }
            }
        }

        #region Helper Methods

        private string GetEmitterPropertyID(IEmitter emitter)
        {
            if (emitter != null)
            {
                return emitter.ID;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}

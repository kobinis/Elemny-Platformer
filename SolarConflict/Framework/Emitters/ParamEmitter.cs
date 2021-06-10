using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SolarConflict.Framework;
using XnaUtils;

//Finish
namespace SolarConflict
{    
    //TODO: Fix random size = 0
    //ToDo: Add summery to all enum values

    /// <summary>
    /// ParamEmitter - Emmits gameObjects according to parameters
    /// </summary>

    [Serializable]
    public class ParamEmitter : IEmitter //TODO: add color
    {
        //Position
        public enum EmitterPosRad
        {
            Const, //Sets posRad to posRadMin
            Range, //Sets posRad from posRadMin to posRadMin+posRadRange
            Random,//Sets posRad randmoly from posRadMin to posRadMin+posRadRange
            RandomCorrected, //Sets posRad randmoly from posRadMin to posRadMin+posRadRange Corrected to make uniform     
            ParentSize,
            ParentSizeTransformed,
            DistanceFromMotherShip,
            PerpendicularToRotation
        }

        public enum EmitterPosAngle
        {         
            Const,   //Sets posAngle to posAngleBase
            RangeCenterd, //Sets posAngle from posAngleBase - range to posAngleBase+range
            Random, //Sets posAngle randomly from posAngleBase - range to posAngleBase+range
            Range, //Sets posAngle from posAngleBase to posAngleBase+range 
            AngleToMotherShip
        }


        //Velocity
        public enum EmitterVelocityMag
        {         
            Const, //Sets velocityMag to velocityMagMin
            Range, //Sets velocityMagMin from velocityMagMin to velocityMagMin + velocityMagRange
            Random, //Sets velocityMagMin randomly from velocityMagMin to velocityMagMin + velocityMagRange            
        }

        public enum EmitterVelocityAngle
        {        
            Const, //Sets velocityAngle to velocityAngleBase
            RangeCenterd, //Sets velocityAngle from velocityAngleBase - velocityAngleRange to velocityAngleBase + velocityAngleRange
            Random, //Sets velocityAngle randomly from velocityAngleBase - velocityAngleRange to velocityAngleBase + velocityAngleRange
            Range, //Sets velocityAngle randomly from velocityAngleBase to velocityAngleBase + velocityAngleRange
            PosAngle, //Sets velocityAngle to posAngle  
            PosAngleTransformed, //Sets velocityAngle to posAngle * velocityAngleRange + velocityAngleBase            
            LastObjectToColide
        }

        //Rotation
        public enum EmitterRotation
        {
            Const, //Sets rotation to rotationBase
            RangeCenterd, //Sets rotation from rotationBase - rotationRange to rotationBase + rotationRange
            Random,  //Sets rotation from rotationBase - rotationRange to rotationBase + rotationRange
            Range,
            VelocityAngle,
            PosAngle,
            SpeedAngleTransformed,
            PosAngleTransformed
        }
        //Rotation speed
        public enum EmitterRotationSpeed
        {
            Const,
            RangeCenterd,
            Random,
            Range,
            SpeedAngle,
            PosAngle,
            SpeedAngleTransformed,
            PosAngleTransformed
        }

        public enum EmitterLifetime
        { //changeName
            Default, //Sets lifetime to zero (projectile will get its Default lifetime)
            Const,   //Sets lifetime to lifetimeMin
            Range,   //Sets lifetime from lifetimeMin to lifetimeMin+lifetimeRange
            ParentSizeTransormed,
            Random   //Sets lifetime from lifetimeMin to lifetimeMin+lifetimeRange
        }

        public enum InitSizeType
        {
            Default, //Sets size to zero (projectile will get its Default size)
            InputSize,
            Const,   //Sets size to lifetimeMin
            Range,   //Sets size from lifetimeMin to lifetimeMin+lifetimeRange
            ParentSize,
            ParentSizeTransformed,
            Random   //Sets size from lifetimeMin to lifetimeMin+lifetimeRange 
        }
               
        private string id;
        private IEmitter emitter; 
        private float refVelocityMult;
       // private float refRotationMult; //??

        private EmitterPosRad _positionRadType;
        private float _positionRadMin;
        private float _posRadRange;

        private EmitterPosAngle _positionAngleType;
        private float _positionAngleBase;
        private float _positionAngleRange;

        private EmitterVelocityMag _velocityMagType;
        private float _velocityMagMin;
        private float _velocityMagRange;

        private EmitterVelocityAngle _velocityAngleType;
        private float _velocityAngleBase;
        public float _velocityAngleRange;

        private EmitterRotation _rotationType;
        private float _rotationBase;
        private float _rotationRange;

        private EmitterRotationSpeed _rotationSpeedType;
        private float _rotationSpeedBase;
        private float _rotationSpeedRange; //maybe add bool the flips sign

        private EmitterLifetime _lifeTimeType;
        private int _lifeTimeMin;
        private int _lifeTimeRange;

        private InitSizeType _sizeType;
        private float _sizeBase;
        private float _sizeRange;

        private int _minNumberOfGameObjects;
        private int _rangeNumberOfGameObject;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
                
        [XmlIgnore]
        public IEmitter Emitter
        {
            get { return emitter; }
            set { emitter = value; }
        }
      
        public string EmitterID
        {
            get { return emitter.ID; }
            set { emitter = ContentBank.Inst.GetEmitter(value); }
        }
        
        public float RefVelocityMult
        {
            get { return refVelocityMult; }
            set { refVelocityMult = value; }
        }           

        public EmitterPosRad PosRadType
        {
            get { return _positionRadType; }
            set { _positionRadType = value; }
        }        

        public float PosRadRange
        {
            get { return _posRadRange; }
            set { _posRadRange = value; }
        }

        public float PosRadMin
        {
            get { return _positionRadMin; }
            set { _positionRadMin = value; }
        }
        
        public EmitterPosAngle PosAngleType
        {
            get { return _positionAngleType; }
            set { _positionAngleType = value; }
        }
        
        public float PosAngleRange
        {
            get { return _positionAngleRange * 180 / MathHelper.Pi; }
            set { _positionAngleRange = value * MathHelper.Pi / 180; }
        }

        public float PosAngleBase
        {
            get { return _positionAngleBase * 180 / MathHelper.Pi; }
            set { _positionAngleBase = value * MathHelper.Pi / 180; }
        }
     
        public EmitterVelocityMag VelocityMagType
        {
            get { return _velocityMagType; }
            set { _velocityMagType = value; }
        }
     
        public float VelocityMagRange
        {
            get { return _velocityMagRange; }
            set { _velocityMagRange = value; }
        }

        public float VelocityMagMin
        {
            get { return _velocityMagMin; }
            set { _velocityMagMin = value; }
        }
        
        public EmitterVelocityAngle VelocityAngleType
        {
            get { return _velocityAngleType; }
            set { _velocityAngleType = value; }
        }


        public float VelocityAngleRange
        {
            get { return _velocityAngleRange * 180 / MathHelper.Pi; }
            set { _velocityAngleRange = value * MathHelper.Pi / 180; }
        }

        public float VelocityAngleBase
        {
            get { return _velocityAngleBase * 180 / MathHelper.Pi; }
            set { _velocityAngleBase = value * MathHelper.Pi / 180; }
        }

        public EmitterRotation RotationType
        {
            get { return _rotationType; }
            set { _rotationType = value; }
        }

        public float RotationRange
        {
            get { return _rotationRange * 180 / MathHelper.Pi; }
            set { _rotationRange = value * MathHelper.Pi / 180; }
        }

        public float RotationBase
        {
            get { return _rotationBase * 180 / MathHelper.Pi; }
            set { _rotationBase = value * MathHelper.Pi / 180; }
        }

        public EmitterRotationSpeed RotationSpeedType
        {
            get { return _rotationSpeedType; }
            set { _rotationSpeedType = value; }
        }        

        public float RotationSpeedRange
        {
            get { return _rotationSpeedRange * 180 / MathHelper.Pi; }
            set { _rotationSpeedRange = value * MathHelper.Pi / 180; }
        }

        public float RotationSpeedBase
        {
            get { return _rotationSpeedBase * 180 / MathHelper.Pi; }
            set { _rotationSpeedBase = value * MathHelper.Pi / 180; }
        }

        public EmitterLifetime LifetimeType
        {
            get { return _lifeTimeType; }
            set { _lifeTimeType = value; }
        }

        public int LifetimeRange
        {
            get { return _lifeTimeRange; }
            set { _lifeTimeRange = value; }
        }

        public int LifetimeMin
        {
            get { return _lifeTimeMin; }
            set { _lifeTimeMin = value; }
        }
        
        public InitSizeType SizeType
        {
            get { return _sizeType; }
            set { _sizeType = value; }
        }
   
        public float SizeBase
        {
            get { return _sizeBase; }
            set { _sizeBase = value; }
        }

        public float SizeRange
        {
            get { return _sizeRange; }
            set { _sizeRange = value; }
        }

        public int RangeNumberOfGameObject
        {
            get { return _rangeNumberOfGameObject; }
            set { _rangeNumberOfGameObject = value; }
        }

        public int MinNumberOfGameObjects
        {
            get { return _minNumberOfGameObjects; }
            set { _minNumberOfGameObjects = value; }
        }

        public ParamEmitter(IEmitter emitter)
        {
            this.emitter = emitter;
            _minNumberOfGameObjects = 1;
            _velocityMagMin = 0f;
            _rotationType = EmitterRotation.Const;
            refVelocityMult = 1;            
            _rotationBase = 0;
            _rotationSpeedBase = 0;
            _rangeNumberOfGameObject = 0;
            _lifeTimeMin = 0;
            _lifeTimeRange = 0;            
        }

        public ParamEmitter()
            : this(null)
        {
        }
        

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {            
            
            Vector2 position = Vector2.Zero;
            Vector2 velocity = Vector2.Zero;
            
            int numberOfGameObject;
            if (_rangeNumberOfGameObject == 0)
            {
                numberOfGameObject = _minNumberOfGameObjects;
            }
            else
            {
                numberOfGameObject = _minNumberOfGameObjects + gameEngine.Rand.Next(_rangeNumberOfGameObject);
            }

            GameObject gameObject = null;

            for (int i = 0; i < numberOfGameObject; i++)
            {


                float normalizedIndex = i / (float)Math.Max(numberOfGameObject - 1, 1);

               

                float initPosAngle = 0;                
                switch (_positionAngleType)
                {                   
                    case EmitterPosAngle.Const:
                        initPosAngle = _positionAngleBase;
                        break;
                    case EmitterPosAngle.RangeCenterd:
                        initPosAngle = ((float)gameEngine.Rand.NextDouble() - 0.5f) * _positionAngleRange + _positionAngleBase;
                        break;
                    case EmitterPosAngle.Random:
                        initPosAngle = ((float)gameEngine.Rand.NextDouble() - 0.5f) * _positionAngleRange + _positionAngleBase;
                        break;
                    case EmitterPosAngle.Range:
                        initPosAngle = (i / (float)numberOfGameObject) * _positionAngleRange + _positionAngleBase;
                        break;
                    case EmitterPosAngle.AngleToMotherShip:
                        GameObject baseObject = gameEngine.GetFaction(faction).Mothership;
                        if (baseObject != null && parent != null)
                        {
                            Vector2 diff =  baseObject.Position - parent.Position;
                            initPosAngle = _positionAngleBase + (float)Math.Atan2(diff.Y, diff.X) - refRotation;
                        }
                        break;
                    default:
                        initPosAngle = 0;
                        break;
                }


                float initPosRad = 0;
                //Position
                switch (_positionRadType)
                {
                    case EmitterPosRad.Const:
                        initPosRad = _positionRadMin;
                        break;
                    case EmitterPosRad.Range:
                        initPosRad = _positionRadMin + normalizedIndex * _posRadRange;
                        break;
                    case EmitterPosRad.Random:
                        initPosRad = _positionRadMin + (float)gameEngine.Rand.NextDouble() * _posRadRange;
                        break;
                    case EmitterPosRad.RandomCorrected: //KOBI: Implement
                        initPosRad = FMath.TransformToRadius(gameEngine.Rand.NextFloat(), _positionRadMin + _posRadRange, _positionRadMin); //_positionRadMin + (float)gameEngine.Rand.NextDouble() * _posRadRange;
                        break;
                    case EmitterPosRad.ParentSize:
                        initPosRad = parent.Size;
                        break;
                    case EmitterPosRad.ParentSizeTransformed:
                        initPosRad = _positionRadMin + parent.Size * _posRadRange;
                        break;
                    case EmitterPosRad.DistanceFromMotherShip:
                        GameObject baseObject = gameEngine.GetFaction(faction).Mothership;
                        if (baseObject != null && parent != null)
                        {
                            float distance = (parent.Position - baseObject.Position).Length();
                            initPosRad = _positionRadMin + distance * _posRadRange;
                        }
                        break;
                    case EmitterPosRad.PerpendicularToRotation:
                        initPosRad = (_positionRadMin + normalizedIndex * _posRadRange) * (float)Math.Cos(initPosAngle + refRotation);
                        break;
                    default:
                        initPosRad = 0;
                        break;
                }


                //Velocity              
                float initVelocityMag = 0;                
                switch (_velocityMagType)
                {
                    case EmitterVelocityMag.Const:
                        initVelocityMag = _velocityMagMin;
                        break;
                    case EmitterVelocityMag.Range:
                        initVelocityMag = _velocityMagMin + normalizedIndex * _velocityMagRange;
                        break;
                    case EmitterVelocityMag.Random:
                        initVelocityMag = _velocityMagMin + (float)gameEngine.Rand.NextDouble() * _velocityMagRange;
                        break;
                    default:
                        initVelocityMag = 0;
                        break;
                }

                float initVelocityAngle = 0;
                switch (_velocityAngleType)
                {
                    case EmitterVelocityAngle.Const:
                        initVelocityAngle = _velocityAngleBase;
                        break;
                    case EmitterVelocityAngle.RangeCenterd:
                        initVelocityAngle = (normalizedIndex - 0.5f) * _velocityAngleRange + _velocityAngleBase;
                        break;
                    case EmitterVelocityAngle.Random:
                        initVelocityAngle = ((float)gameEngine.Rand.NextDouble() - 0.5f) * _velocityAngleRange + _velocityAngleBase;
                        break;
                    case EmitterVelocityAngle.Range:
                        initVelocityAngle = (i / (float)numberOfGameObject) * _velocityAngleRange + _velocityAngleBase;
                        break;
                    case EmitterVelocityAngle.PosAngle:
                        initVelocityAngle = initPosAngle;
                        break;
                    case  EmitterVelocityAngle.PosAngleTransformed:
                        initVelocityAngle = initPosAngle * _velocityAngleRange + _velocityAngleBase;
                        break;
                    case EmitterVelocityAngle.LastObjectToColide:
                        if(parent != null && parent.GetLastObjectToCollide() != null)
                        {
                            Vector2 diff = parent.GetLastObjectToCollide().Position - parent.Position;
                            initVelocityAngle = (float)Math.Atan2(diff.Y, diff.X) - refRotation;                            
                        }
                        initVelocityAngle += ((float)gameEngine.Rand.NextDouble() - 0.5f) * _velocityAngleRange + _velocityAngleBase;
                        break;
                    default:
                        initVelocityAngle = 0;
                        break;
                }


                //Rotation
                float initRotation = 0;
                switch (_rotationType)
                {
                    case EmitterRotation.Const:
                        initRotation = _rotationBase;
                        break;
                    case EmitterRotation.RangeCenterd:
                        initRotation = (normalizedIndex - 0.5f) * _rotationRange + _rotationBase;
                        break;
                    case EmitterRotation.Random:
                        initRotation = ((float)gameEngine.Rand.NextDouble() - 0.5f) * _rotationRange + _rotationBase;
                        break;
                    case EmitterRotation.Range:
                        initRotation = (i / (float)numberOfGameObject) * _rotationRange + _rotationBase;
                        break;
                    case EmitterRotation.VelocityAngle:
                        initRotation = initVelocityAngle;
                        break;
                    case EmitterRotation.PosAngle:
                        initRotation = initPosAngle;
                        break;
                    case EmitterRotation.SpeedAngleTransformed:
                        initRotation = initVelocityAngle * _rotationRange + _rotationBase;
                        break;
                    case EmitterRotation.PosAngleTransformed:
                        initRotation = initPosAngle * _rotationRange + _rotationBase;
                        break;
                    default:
                        initRotation = 0;
                        break;
                }

                //Rotation Speed
                float initRotationSpeed = 0;
                switch (_rotationSpeedType)
                {
                    case EmitterRotationSpeed.Const:
                        initRotationSpeed = _rotationSpeedBase;
                        break;
                    case EmitterRotationSpeed.RangeCenterd:
                        initRotationSpeed = (normalizedIndex - 0.5f) * _rotationSpeedRange + _rotationSpeedBase;
                        break;
                    case EmitterRotationSpeed.Random:
                        initRotationSpeed = ((float)gameEngine.Rand.NextDouble() - 0.5f) * _rotationSpeedRange + _rotationSpeedBase;
                        break;
                    case EmitterRotationSpeed.Range:
                        initRotationSpeed = (i / (float)numberOfGameObject) * _rotationSpeedRange + _rotationSpeedBase;
                        break;
                    case EmitterRotationSpeed.SpeedAngle:
                        initRotationSpeed = initVelocityAngle;
                        break;
                    case EmitterRotationSpeed.PosAngle:
                        initRotationSpeed = initPosAngle;
                        break;
                    case EmitterRotationSpeed.SpeedAngleTransformed:
                        initRotationSpeed = initVelocityAngle * _rotationSpeedRange + _rotationSpeedBase;
                        break;
                    case EmitterRotationSpeed.PosAngleTransformed:
                        initRotationSpeed = initPosAngle * _rotationSpeedRange + _rotationSpeedBase;
                        break;
                    default:
                        initRotationSpeed = 0;
                        break;
                }

                int initMaxLifetime;
                if (maxLifetime == 0)
                {
                    switch (_lifeTimeType)
                    {
                        case EmitterLifetime.Default:
                            initMaxLifetime = 0;
                            break;
                        case EmitterLifetime.Const:
                            initMaxLifetime = _lifeTimeMin;
                            break;
                        case EmitterLifetime.Range:
                            initMaxLifetime = _lifeTimeMin + (int)((i / (float)numberOfGameObject) * _lifeTimeRange + 0.5f);
                            break;
                        case EmitterLifetime.Random:
                            initMaxLifetime = _lifeTimeMin + gameEngine.Rand.Next(_lifeTimeRange);
                            break;
                        default:
                            initMaxLifetime = 0;
                            break;
                    }
                }
                else
                {
                    initMaxLifetime = maxLifetime;
                }

                float? initSize = null; //??

                if (size == null)
                {
                    switch (_sizeType)
                    {
                        case InitSizeType.Default:
                            initSize = null;
                            break;
                        case InitSizeType.Const:
                            initSize = _sizeBase;
                            break;
                        case InitSizeType.Range:
                            initSize = _sizeBase + normalizedIndex * _sizeRange;
                            break;
                        case InitSizeType.Random:
                            initSize = SizeBase + (float)gameEngine.Rand.NextDouble() * _sizeRange;
                            break;
                        case InitSizeType.ParentSize:
                            initSize = parent.Size;
                            break;
                        case InitSizeType.ParentSizeTransformed:
                            initSize = _sizeBase + parent.Size * _sizeRange;
                            break;
                        
                        default:
                            initSize = null;
                            break;
                    }
                }
                else
                {
                    initSize = size;
                }

                initPosAngle += refRotation;// *refRotationMult;
                initVelocityAngle += refRotation;// *refRotationMult;
                initRotation += refRotation;// *refRotationMult;

                position.X = refPosition.X + (float)Math.Cos(initPosAngle) * initPosRad;
                position.Y = refPosition.Y + (float)Math.Sin(initPosAngle) * initPosRad;

                velocity.X = refVelocity.X * refVelocityMult + (float)Math.Cos(initVelocityAngle) * initVelocityMag;
                velocity.Y = refVelocity.Y * refVelocityMult + (float)Math.Sin(initVelocityAngle) * initVelocityMag;
                
                gameObject = emitter.Emit(gameEngine, parent, faction, position, velocity , initRotation, initRotationSpeed, initMaxLifetime, initSize, color, param);                                   
            }
            return gameObject;
        }

        public ParamEmitter GetWorkingCopy()
        {
            return this;
        }

        public static ParamEmitter MakePositionSpreadParam(int num, int range = 0)
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.MinNumberOfGameObjects = num;
            emitter.RangeNumberOfGameObject = range;
            emitter.PosAngleType = EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;
            emitter.RotationType = EmitterRotation.PosAngle;
            return emitter;
        }

        public static ParamEmitter MakeSpreadParam(int num, int range = 0)
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.MinNumberOfGameObjects = num;
            emitter.RangeNumberOfGameObject = range;
            emitter.VelocityAngleType = EmitterVelocityAngle.Range;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagMin = 20;
            emitter.RotationType = EmitterRotation.VelocityAngle;
            return emitter;
        }

        public static ParamEmitter MakeRandomSpredParam(int num, int range = 0)
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.MinNumberOfGameObjects = num;
            emitter.RangeNumberOfGameObject = range;
            emitter.VelocityAngleType = EmitterVelocityAngle.Range;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagMin = 20;
            emitter.RotationType = EmitterRotation.VelocityAngle;
            return emitter;
        }

    }
}

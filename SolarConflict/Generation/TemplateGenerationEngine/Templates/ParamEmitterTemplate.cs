using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class ParamEmitterTemplate : ItemGenerationTemplate
    {
        private readonly string MINIMUM_NUMBER_OF_GAME_OBJECTS = "Minimum Number Of Game Objects";
        private readonly string RANGE_NUMBER_OF_GAME_OBJECTS = "Range Number Of Game Object";
        private readonly string VELOCITY_MAGNITUDE_MINIMUM = "Velocity Magnitude Minimum";
        private readonly string VELOCITY_MAGNITUDE_RANGE = "Velocity Magnitude Range";
        private readonly string VELOCITY_MAGNITUDE_TYPE = "Velocity Magnitude Type";
        private readonly string VELOCITY_ANGLE_TYPE = "Velocity Angle Type";
        private readonly string ROTATION_TYPE = "Rotation Type";


        // no included yet in the CSV
        //private IEmitter emitter;
        //private float refVelocityMult;
        //// private float refRotationMult; //??

        //private EmitterPosRad _positionRadType;
        //private float _positionRadMin;
        //private float _posRadRange;

        //private EmitterPosAngle _positionAngleType;
        //private float _positionAngleBase;
        //private float _positionAngleRange;

        //private float _velocityAngleBase;
        //private float _velocityAngleRange;

        //private float _rotationBase;
        //private float _rotationRange;

        //private EmitterRotationSpeed _rotationSpeedType;
        //private float _rotationSpeedBase;
        //private float _rotationSpeedRange; //maybe add bool the flips sign

        //private EmitterLifetime _lifeTimeType;
        //private int _lifeTimeMin;
        //private int _lifeTimeRange;

        //private InitSizeType _sizeType;
        //private float _sizeBase;
        //private float _sizeRange;


        public ParamEmitterTemplate()
        {
            _directoryName = "ParamEmitters";
            AddParametereName(ID);
            AddParametereName(MINIMUM_NUMBER_OF_GAME_OBJECTS);
            AddParametereName(RANGE_NUMBER_OF_GAME_OBJECTS);
            AddParametereName(VELOCITY_MAGNITUDE_MINIMUM);
            AddParametereName(VELOCITY_MAGNITUDE_RANGE);
            AddParametereName(VELOCITY_MAGNITUDE_TYPE);
            AddParametereName(VELOCITY_ANGLE_TYPE);
            AddParametereName(ROTATION_TYPE);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("KineticShot1")); // addd param Emitted ID
            emitter.ID = csvUtils.GetString(ID);
            emitter.MinNumberOfGameObjects = csvUtils.GetInt(MINIMUM_NUMBER_OF_GAME_OBJECTS);
            emitter.RangeNumberOfGameObject = csvUtils.GetInt(RANGE_NUMBER_OF_GAME_OBJECTS);
            emitter.VelocityAngleRange = MathHelper.ToDegrees(MathHelper.PiOver4); // yanik:  add float
            emitter.VelocityAngleType = csvUtils.GetEnum<ParamEmitter.EmitterVelocityAngle>(VELOCITY_ANGLE_TYPE); 
            emitter.VelocityMagType = csvUtils.GetEnum<ParamEmitter.EmitterVelocityMag>(VELOCITY_MAGNITUDE_TYPE); 
            emitter.VelocityMagMin = csvUtils.GetFloat(VELOCITY_MAGNITUDE_MINIMUM);
            emitter.VelocityMagRange = csvUtils.GetFloat(VELOCITY_MAGNITUDE_RANGE);
            emitter.RotationType = csvUtils.GetEnum<ParamEmitter.EmitterRotation>(ROTATION_TYPE); 

            ContentBank.Inst.AddContent(emitter); 
        }
    }
 }

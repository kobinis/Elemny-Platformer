using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{

    public class GroupEmitterTemplate : ItemGenerationTemplate
    {
        private readonly string TYPE = "Type*";
        private readonly string RANDOM_VELOCITY_RANGE = "Random Velocity Range*";
        private readonly string RANDOM_VELOCITY_BASE = "Random Velocity Base*";
        private readonly string ROTATION_SPEED = "Rotation Speed*";
        private readonly string VELOCITY_MULTIPLY = "Velocity Multiply";
        private readonly string SOUND_NAME = "Sound Name*";
        private readonly string SOUND_VOLUME = "Sound Volume*";


        public GroupEmitterTemplate()
        {
            _directoryName = "GroupEmitters";
            AddParametereName(ID);
            AddParametereName(TYPE);
            AddParametereName(RANDOM_VELOCITY_RANGE);
            AddParametereName(RANDOM_VELOCITY_BASE);
            AddParametereName(ROTATION_SPEED);
            AddParametereName(VELOCITY_MULTIPLY);
            AddParametereName(SOUND_NAME);
            AddParametereName(SOUND_VOLUME);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            GroupEmitter emitter = new GroupEmitter();

            emitter.RefVelocityMult = csvUtils.GetFloat(VELOCITY_MULTIPLY);
            emitter.RandomVelocityRange = csvUtils.GetFloat(RANDOM_VELOCITY_RANGE);
            emitter.RandomVelocityBase = csvUtils.GetFloat(RANDOM_VELOCITY_BASE);
            emitter.RotationSpeed = csvUtils.GetFloat(ROTATION_SPEED);
            emitter.RotationSpeed = csvUtils.GetFloat(ROTATION_SPEED);

            AddEmitters(parameters, emitter);

            string soundEffectName = csvUtils.GetString(SOUND_NAME);

            if (!string.IsNullOrEmpty(soundEffectName))            
                emitter.AddEmitter(new SoundEmitter(soundEffectName, csvUtils.GetFloat(SOUND_VOLUME, 1f)));
            

            ContentBank.Inst.AddContent(emitter); 
        }

        private void AddEmitters(string[] parameters, GroupEmitter emitter)
        {
            for (int i = 8; i < parameters.Length; i++)
            {
                if (!string.IsNullOrEmpty(parameters[i]))
                {
                    string[] emmiterData = parameters[i].Split(':');
                    string emmiterName = emmiterData[0];

                    if (emmiterData.Length > 1)
                    {
                        float probability = ParserUtils.ParseFloat(emmiterData[1]);
                        emitter.AddEmitter(emmiterName, probability);
                    }
                    else
                    {
                        emitter.AddEmitter(emmiterName);
                    }
                }
            }
        }
    }
}

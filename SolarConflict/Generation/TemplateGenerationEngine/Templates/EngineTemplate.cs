using Microsoft.Xna.Framework;
using System;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class EngineTemplate : ItemGenerationTemplate
    {
        private readonly string SUFFIX_DESCRIPTION = "\n\nMax Speed: {0}{2}\nForce: {1}";
        private readonly string TRAIL_SPEED = "Trail Speed*";
        private readonly string COOL_DOWN = "Cooldown*";

        private readonly int DEFUALT_TRAIL_SPEED = 3;
        private readonly int DEFUALT_COOL_DOWN = 1;

        public EngineTemplate()
        {
            _directoryName = "Engines";
            AddGeneralParameters();
            AddParametereName(ACTIVE_TIME);
            AddParametereName("Acceleration");
            AddParametereName("Max Speed");
            AddParametereName("Engine Trail Emitter ID");
            AddParametereName(COOL_DOWN);
            AddParametereName(TRAIL_SPEED);
            AddParametereName("Trail Color*"); 
            AddParametereName(METER_TYPE); 
            AddParametereName(METER_COST);
        }

        protected override Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID, 
                                         string equippedTextureID, float buyPrice, float sellRatio, Color? color)
        {
            int maxSpeed = csvUtils.GetInt("Max Speed");
            float force = csvUtils.GetFloat("Acceleration");

            string suffixDescription = string.Format(SUFFIX_DESCRIPTION, maxSpeed, force, "#Image{Speed}");

            ItemProfile profile = InitGeneralParameters(ID, name, description + suffixDescription, level, size, textureID, equippedTextureID, buyPrice, sellRatio, color, SlotType.Engine);
            profile.IsActivatable = true;
            profile.Category = ItemCategory.Engine;

            var trailEmitterId = csvUtils.GetString("Engine Trail Emitter ID");

            Item item = new Item(profile);

            var engine = new EmitterCallerSystem(ControlSignals.None, csvUtils.GetInt(COOL_DOWN, DEFUALT_COOL_DOWN), trailEmitterId);
            if (!string.IsNullOrWhiteSpace(trailEmitterId))
            {
                // TEMP: just use hardcoded interval size for interpolated emission
                // TODO: maybe have EmitterCallerSystem take a constructor arg on an InterpolationMode or whatever telling it to
                // close the gap     
                //engine.InterpolationInterval = 5f;
            }
            engine.DontActivateEmitterWhenCloaked = true;
            engine.ActiveTime = csvUtils.GetInt(ACTIVE_TIME, DEFUALT_ACTIVE_TIME);
            engine.velocity = Vector2.UnitX * csvUtils.GetFloat(TRAIL_SPEED, DEFUALT_TRAIL_SPEED);
            engine.SelfImpactSpec = new CollisionSpec();
            engine.SelfImpactSpec.Force = -force;
            engine.SelfSpeedLimit = maxSpeed;
            engine.Color = csvUtils.GetColor("Trail Color*", DEFAULT_COLOR);            

            if (csvUtils.HasValue(METER_COST) && csvUtils.GetFloat(METER_COST) > 0)
            {
                engine.ActivationCheck.AddCost(csvUtils.GetEnum<MeterType>(METER_TYPE, MeterType.Energy), csvUtils.GetFloat(METER_COST));
            }

            item.System = engine;
            

            return item;
        }


//        Engine:
//            Meter cost move to meter type
//Check if we can load the csv files when we edit them
    }
}





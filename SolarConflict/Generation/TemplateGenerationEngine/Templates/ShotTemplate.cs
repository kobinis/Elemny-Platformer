using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class ShotTemplate : GenerationTemplate
    {
        private readonly string TEXTURE = "Texture";
        private readonly string DRAW_TYPE = "Draw Type*";
        private readonly string COLOR_LOGIC = "Color Logic*";
        private readonly string SHOT_SIZE = "Size";  
        private readonly string MAX_LIFE_TIME = "Max Life Time";
        private readonly string MASS = "Mass";
        private readonly string IMPACT_EMMITER = "Impact Emitter";
        private readonly string IS_DESTROYED_ON_COLLUSION = "Is Destroyed On Collusion*";
        private readonly string IS_EFFECTED_BY_FORCE = "Is Effected By Force*";
        private readonly string DAMAGE = "Damage";
        private readonly string COLLUSION_FORCE = "Collusion Force";
        private readonly string COLLISION_WIDTH = "Collision Width*";
        private readonly string IMPACT_DAMAGE_TYPE = "Impact Damage Type*";
        public const string COLOR = "Color*";
        private readonly string SPECIAL_EFFECT = "Special Effect*";



        private readonly string DEFAULT_IMPACE_EMIITER_NAME = "EmitterImpactFx1";


        public ShotTemplate()
        {
            _directoryName = "Shots";
            AddParametereName(ID);
            AddParametereName(TEXTURE);
            AddParametereName(COLOR);
            AddParametereName(DRAW_TYPE);
            AddParametereName(COLOR_LOGIC);
            AddParametereName(SHOT_SIZE);
            AddParametereName(MAX_LIFE_TIME);
            AddParametereName(MASS);
            AddParametereName(IMPACT_EMMITER);
            AddParametereName(IS_DESTROYED_ON_COLLUSION);
            AddParametereName(IS_EFFECTED_BY_FORCE);
            AddParametereName(DAMAGE);
            AddParametereName(COLLUSION_FORCE);
            AddParametereName(COLLISION_WIDTH);
            AddParametereName(IMPACT_DAMAGE_TYPE);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            var profile = MakeProjectileProfile(parameters);

            ContentBank.Inst.AddContent(profile); 
        }

        protected virtual ProjectileProfile MakeProjectileProfile(string[] parameters)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = csvUtils.GetString(ID);
            profile.InitColor = new InitColorConst(csvUtils.GetColor(COLOR, Color.White));
            profile.DrawType = csvUtils.GetEnum<DrawType>(DRAW_TYPE, DrawType.Additive);

            var colorUpdaterType = csvUtils.GetEnum<ColorUpdater.Type>(COLOR_LOGIC, ColorUpdater.Type.FadeOut);
            profile.ColorLogic = ColorUpdater.GetColorUpdater(colorUpdaterType);

            profile.TextureID = csvUtils.GetString(TEXTURE);
            profile.CollisionWidth = profile.Sprite.Width - csvUtils.GetFloat(COLLISION_WIDTH, -10); 
            profile.InitSizeID = csvUtils.GetString(SHOT_SIZE);
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(csvUtils.GetFloat(MAX_LIFE_TIME));
            profile.Mass = csvUtils.GetFloat(MASS);
            profile.ImpactEmitterID = csvUtils.GetString(IMPACT_EMMITER, DEFAULT_IMPACE_EMIITER_NAME);
            profile.IsDestroyedOnCollision = csvUtils.GetBool(IS_DESTROYED_ON_COLLUSION, true);
            profile.IsEffectedByForce = csvUtils.GetBool(IS_EFFECTED_BY_FORCE, false);


            profile.CollisionSpec = new CollisionSpec(csvUtils.GetFloat(DAMAGE), csvUtils.GetFloat(COLLUSION_FORCE));

            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.Force = csvUtils.GetFloat(COLLUSION_FORCE);

            ImpactType impactType = csvUtils.GetEnum<ImpactType>(IMPACT_DAMAGE_TYPE, ImpactType.Additive);
            profile.CollisionSpec.AddEntry(MeterType.Damage, csvUtils.GetFloat(DAMAGE, 0), ImpactType.Additive);

            //if (csvUtils.HasValue(DAMAGE))
            //{
            //    ImpactType impactType = csvUtils.GetEnum<ImpactType>(IMPACT_DAMAGE_TYPE, ImpactType.Additive);               
            //    profile.CollisionSpec.AddEntry(MeterType.Damage, csvUtils.GetFloat(DAMAGE), impactType);
            //}

            //AddItems(parameters, profile.CollisionSpec);

            return profile;
        }

        private void AddItems(string[] parameters, CollisionSpec collusionInfo)
        {
            for (int i = 15; i < parameters.Length; i++)
            {
                float damage = csvUtils.GetFloat(DAMAGE);

                if (parameters[i] != string.Empty)
                {
                    string[] shotData = parameters[i].Split(':');

                    MeterType type = ParserUtils.ParseEnum<MeterType>(shotData[0], MeterType.None);

                    if (shotData.Length > 1)
                    {
                        damage = ParserUtils.ParseFloat(shotData[1], 0);
                    }

                    if (type == MeterType.Energy || type == MeterType.Shield || type == MeterType.Hitpoints)
                    {
                        collusionInfo.AddEntry(type, -1 * damage);
                    }
                    else if (type == MeterType.StunTime)
                    {            
                        collusionInfo.AddEntry(MeterType.StunTime, damage);                       
                    }
                }
            }
        }
    }
}

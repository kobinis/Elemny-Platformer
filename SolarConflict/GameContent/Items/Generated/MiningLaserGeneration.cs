using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems.EmitterCallers;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Items.Generated
{
    class MiningLaserGeneration
    {

        public static IEmitter Make()
        {
            for (int i = 1; i < 6; ++i)
                ContentBank.Inst.AddContent(MakeMiningLaserItem(i));
            return null;
        }

        public static Item MakeMiningLaserItem(int level)
        {
            var colors = new Color[] 
            {
                new Color(1f, 0f, 0f),
                new Color(0f, 1f, 0f),
                new Color(0f, 0f, 1f),
                new Color(1f, 1f, 0f),
                new Color(0f, 1f, 1f),
                new Color(1f, 0f, 1f),
            };

            string id = $"MiningLaser{level}";
            string suffix = string.Empty;
            if (level >= 1)
                suffix = " Mk " + level.ToString();                        
            WeaponData data = new WeaponData("Mining Laser" + suffix, level, "MiningLaser", "rapid-gun");            
            data.ItemData.BuyPrice = (ScalingUtils.ScaleCost(level) * 3 /10) *10;
            data.ItemData.Category = ItemCategory.Mining;
            
            if (level >= 1)
                data.ItemData.SecounderyIconID = $"lv{level}";
            

            var emitter = LaserEmitter.Make();
            float damage =ScalingUtils.ScaleDamagePerFrame(level) / 4f;
         //   emitter.Emitter = MakeMiningLaserBit(null, colors[level], damage, (float)level + 1);            


            data.ShotEmitter = emitter;
                        
            data.EnergyCost = (int)(ScalingUtils.ScaleEnergyCostPerFrame(level) * 60 * 2.5f);
            data.ActiveTime = 60;            
            data.Cooldown = 60;
            data.MidCooldownTime = 0;
            data.EffectSpeed = 0.1f;
            data.EffectEmitterID = "LaserGunFX";
            data.Range = 600;
            data.SoundEffectEmitterID = null;            
            Item item = WeaponQuickStart.Make(data);
            var beamSystem = new BeamSystem();
            beamSystem.beamEmitter = MakeBeam(colors[level], damage , (float)level + 1);
            beamSystem.EffectEmitterID = "VoidBeamFlash";
            item.System = beamSystem;
            item.ID = id;            
            return item;
        }


        public static ProjectileProfile MakeBeam(Color color, float damage, float miningDamage)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.MediumLight(Color.Purple);
            profile.DrawType = DrawType.Beams;
            //         profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "whitebeam";// "Beam01";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "5";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0;
            profile.MovementLogic = new MoveToRaycast(1000);
           // profile.ImpactEmitterID = "EmitterPickupFx";
            // profile.ImpactEmitter = new ParticleSystemEmitter()
            profile.ImpactEmitterID = "SmallVoidHitParticleFx";

            profile.CollisionSpec = new CollisionSpec(5, 0f);
          //  profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.CollisionSpec.AddEntry(MeterType.MiningLevel, miningDamage, ImpactType.Max);
            profile.CollisionSpec.AddEntry(MeterType.Damage, damage*4, ImpactType.Max);

            profile.Draw = new DrawBeam();
            profile.CollideWithMask = GameObjectType.All;
            // profile.MovementLogic = null;
       //     profile.IsDestroyedOnCollision = false;
        //    profile.IsEffectedByForce = false;
            profile.InitColor = new InitColorConst(color);
            return profile;
        }

        //static ProjectileProfile MakeMiningLaserBit(string id, Color color, float damage, float miningDamage) {            
        //    ProjectileProfile profile = new ProjectileProfile();
        //    profile.ID = id;
        //    profile.DrawType = DrawType.Additive;
        //    profile.InitColor = new InitColorConst(color);
        //    profile.ColorLogic = ColorUpdater.FadeOutSlow;
        //    profile.TextureID = "add10";
        //    profile.CollisionWidth = profile.Sprite.Width - 10;
        //    profile.InitSizeID = "15";
        //    profile.UpdateSize = null;
        //    profile.InitMaxLifetime = new InitFloatConst(1);
        //    profile.Mass = 0.1f;
        //    profile.ImpactEmitterID = typeof(LaserHitFx).Name;
        //    profile.CollisionSpec = new CollisionSpec(0.4f, 0f);
        //    profile.CollisionSpec.AddEntry(MeterType.MiningLevel, miningDamage, ImpactType.Max);
        //    profile.CollisionSpec.AddEntry(MeterType.Damage, damage, ImpactType.Max);
        //    profile.IsDestroyedOnCollision = true;
        //    profile.IsEffectedByForce = false;
        //   // profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
        //    return profile;
        //}
    }
}

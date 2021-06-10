using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.AAAInProgress
{
    class HealingBeam
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Healing Beam");
            weaponData.ItemData.IconID = "VoidTrident";
            weaponData.ItemData.EquippedTextureId = "VoidTrident";
            weaponData.ItemData.Level = 2;
            weaponData.ItemData.SecounderyIconID = null;
            
            weaponData.ShotSpeed = 0;
            //weaponData.ef
            weaponData.ShotLifetime = 1;
            weaponData.Cooldown = 1;
            weaponData.ActiveTime = 1;
            weaponData.ShotEmitter = MakeBeam();
            weaponData.EnergyCost = 1;
            weaponData.EffectEmitterID = null;
            //2 weaponData.IsTurreted = false;
            weaponData.ItemData.BuyPrice = 200;
            weaponData.KickbackForce = 0f;
            weaponData.SoundEffectEmitterID = null;
            weaponData.Range = 1000;

            weaponData.ActivationEmitterID = "VoidBeamFlash";
            //TODO: speed mult = 0;

            Item item = WeaponQuickStart.Make(weaponData, true);
            item.Profile.Category |= ItemCategory.Final;
            item.Profile.Category |= ItemCategory.NonAI;
            item.ItemFlags |= ItemFlags.WorkOnAlly | ItemFlags.Healing;
            
            return item;
        }

        public static ProjectileProfile MakeBeam()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.MediumLight(Color.Green);
            profile.DrawType = DrawType.Beams;
            //         profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "Beam02";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "5";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0;
            profile.MovementLogic = new MoveToRaycast(1000);
            profile.ImpactEmitterID = "EmitterImpactFxVoid1";
            // profile.ImpactEmitter = new ParticleSystemEmitter()
            profile.CollisionSpec = new CollisionSpec(0, 0);
            profile.CollisionSpec.AddEntry(MeterType.Hitpoints, 3);
            profile.CollisionSpec.AddEntry(MeterType.Shield, 4);
            profile.Draw = new DrawBeam();
            // profile.MovementLogic = null;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}

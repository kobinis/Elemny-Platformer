using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateRotation;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Items
{
    class EnergyBolaGun
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Entangling Energy Gun");
            weaponData.ItemData.IconID = "BolaIcon";
            weaponData.ItemData.EquippedTextureId = "Bola";
            weaponData.ItemData.Level = 4;
            weaponData.ItemData.SecounderyIconID = null;
            

            weaponData.Range = 5000;
            weaponData.ShotSpeed = 30;
            weaponData.ShotLifetime = (int)(weaponData.Range / weaponData.ShotSpeed);
            weaponData.Cooldown = 350;
            weaponData.ActiveTime = 1;
            
            weaponData.ShotEmitter = MakeShot();
            weaponData.EnergyCost = 60;
            //2 weaponData.IsTurreted = false;
            weaponData.ItemData.BuyPrice = 200;
            weaponData.KickbackForce = 0.1f;

            // weaponData.EffectEmitterID = "EmitterFxSmoke";
            //TODO: speed mult = 0;
            
            Item item = WeaponQuickStart.Make(weaponData);
            item.Profile.Category |= ItemCategory.NonAI;
            return item;
        }


        public static ProjectileProfile MakeShot()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.SmallLight(Color.Yellow);
            profile.DrawType = DrawType.Additive;
           // profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "BlastT2";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "80";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            //profile.RotationLogic = new updatero
            var draw = new ProjectileDrawMultipleSpritesheets(28, new Spritesheet("Bola30", 114, 61, 28));            
            draw.globalTimeMult = 0.3f;
            profile.Draw = draw;
            profile.UpdateList.Add(new UpdateRotationLifetime(0.08f));
            
            profile.ImpactEmitter = MakeBola();
            profile.TimeOutEmitterID = "EmitterPickupFx";
            profile.CollisionSpec = new CollisionSpec(0, 0);

            profile.CollisionSpec.IsDamaging = true;
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }

        public static ProjectileProfile MakeBola()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.SmallLight(Color.Yellow);
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "BlastT2";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "130";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "300";
            profile.Mass = 0.1f;
            profile.AggroRange = 40;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.AnyPotentialTarget, 0, 0, false);
            //profile.RotationLogic = new updatero
            profile.UpdateList.Add(new UpdateRotationLifetime(0.08f));
            var draw = new ProjectileDrawMultipleSpritesheets(28, new Spritesheet("Bola30", 114, 61, 28));
            
            //Add update size with target
            draw.globalTimeMult = 0.3f;
            profile.Draw = draw;
            //profile.ImpactEmitterID = "EmitterPickupFx";
            profile.CollisionSpec = new CollisionSpec(0, 0.8f); //TODO : change to slow debuff
            profile.CollisionSpec.ForceType = ForceType.Mult;
            profile.CollisionSpec.IsDamaging = true;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}

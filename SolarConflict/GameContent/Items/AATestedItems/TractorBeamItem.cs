using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class TractorBeamItem
    {
        public static Item Make()
        {           
            ItemProfile profile = ItemQuickStart.Profile("Tractor Beam", string.Empty, 2, "turret-engine", null);
            profile.SlotType = SlotType.Weapon | SlotType.Turret;
            profile.BuyPrice = ScalingUtils.ScaleCost(profile.Level);
            profile.MaxStack = 1;
            profile.IsShownOnHUD = true;

            Item item = new Item(profile);
            item.Profile.Category = ItemCategory.Mining;
           
            EmitterCallerSystem system = new EmitterCallerSystem();
            system.Emitter = MakeTractorProjectile();
            system.CooldownTime = 2;
            system.EmitterSpeed = 35;
            //system.SecondaryEmitterID = typeof(GunFlashFx).Name;
            system.secondaryVelocityMult = 0.1f;
            //system.ThirdEmitterID = "sound_shot1";
            TurretSystemHolder turretSystem = new TurretSystemHolder(system, Vector2.Zero, null);
            //turretSystem.
            //TurretSystemHolder holder = new TurretSystemHolder(system, Vector2.Zero, "item3");
            item.Profile.Category |= ItemCategory.NonAI;
            item.System = turretSystem;
            item.Profile.IsWorkingInInventory = true;
            return item;
        }

        public static IEmitter MakeTractorProjectile()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeInOut;
            profile.InitColor = new InitColorConst(Color.LightGreen);
            profile.TextureID = "add11";
           /// profile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "space-mine-advanced", "space-mine-advanced");
            //profile.
            profile.InitSizeID = "25";
            profile.UpdateSize = new UpdateSizeGrow(15);
            profile.InitMaxLifetimeID = "30";
            profile.Mass = 0.1f;
            profile.RotationLogic = new UpdateRotationHoming(0, 0, ProjectileTargetType.Parent); 
            //profile.
            //profile.ImpactEmitterID = typeof(GravityWellAoe).Name;
            //profile.TimeOutEmitterID = typeof(GravityWellAoe).Name;
            profile.CollisionSpec = new CollisionSpec(0, -0.1f);
            profile.CollisionSpec.ForceType = ForceType.Rotation;
            profile.CollisionSpec.Flags = CollisionSpecFlags.AffectsAllies;
            profile.CollisionSpec.AddEntry(MeterType.MiningSpeed, 0.1f);
            profile.VelocityInertia *= 1.02f;
            profile.CollisionType = CollisionType.CollideAll;
            profile.CollideWithMask = GameObjectType.CraftingStation | GameObjectType.Item | GameObjectType.Mineable;

            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}

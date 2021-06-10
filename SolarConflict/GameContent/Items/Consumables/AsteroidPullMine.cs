using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    /// <summary>
    /// When used it will 
    /// </summary>
    class AsteroidPullMine
    {
        public static Item Make()
        {
            //"Releases a mine that after a short delay will pull asteroids and destroy them\nGreat for mining asteroids batches"
            ItemData data = new ItemData("AsteroidPullMine", 2, "spacemine3");
            data.BuyPrice = ScalingUtils.ScaleCost(data.Level);

            data.BreaksClocking = false;
            var mineEmitter = MakePullMine();            
            data.Category = ItemCategory.Consumable | ItemCategory.AsteroidMiningGear | ItemCategory.Mines | ItemCategory.Hotbar;
            data.SlotType = SlotType.Consumable | SlotType.Ammo;
            data.MaxStack = 10;            
            var item = ItemQuickStart.Make(data);
            item.Profile.AmmoEmitter = mineEmitter;
            item.Profile.IsConsumed = true;
            item.Profile.IsWorkingInInventory = true;
            EmitterCallerSystem system = new EmitterCallerSystem(ControlSignals.None, 30, Vector2.Zero, mineEmitter);
            item.System = system;            
            return item;
        }

        public static ProjectileProfile MakePullMine()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;
            //profile.InitColor = new InitColorFaction();
            profile.DrawType = DrawType.Alpha;
            profile.Light = Lights.ContactLight(Color.Green);
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOut;
            profile.TextureID = "spacemine3";
            //profile.CollisionWidth = profile.Sprite.Width ;
            profile.InitSizeID = "40";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetime = new InitFloatConst(90);  // 1/60 of a second
            profile.Mass = 0.1f;

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter(AsteroidPullAOE());
            groupEmitter.AddEmitter(AsteroidGrind());
            groupEmitter.AddEmitter("FireExplosionFx");
            profile.ImpactEmitter = groupEmitter;
            profile.TimeOutEmitter = groupEmitter;
            profile.CollisionSpec = new CollisionSpec(5, 0.5f);
            profile.IsDestroyedOnCollision = true; //projectile is terminated on impact
            profile.IsEffectedByForce = false;
            profile.VelocityInertia = 0.992f;

            return profile;
        }

        public static ProjectileProfile AsteroidPullAOE()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.Collide1;
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = ColorUpdater.FadeInOut;
            profile.TextureID = "bigshockwave";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "1500";
           // profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.InitMaxLifetime = new InitFloatConst(60*20);
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(0, -2f);
            profile.CollisionSpec.Flags &= ~CollisionSpecFlags.IsRotating;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Asteroid;
            profile.DrawType = DrawType.Additive;
        
            return profile;
        }

        public static ProjectileProfile AsteroidGrind()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.Collide1;
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(150, 90, 20));
            profile.ColorLogic = ColorUpdater.FadeInOut;
            profile.TextureID = "add11";// "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSize = new InitFloatConst(500);//new InitFloatParentSize(1.5f, 60f);
            profile.RotationLogic = new UpdateRotationParent();
            profile.InitMaxLifetime = new InitFloatConst(60 * 20);
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(0.5f, 0f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Asteroid;
            profile.CollisionSpec.Flags = CollisionSpecFlags.AffectsAllies;
            return profile;
        }
    }
}

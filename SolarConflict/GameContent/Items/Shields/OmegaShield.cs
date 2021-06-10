using System;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Framework.Emitters;

namespace SolarConflict.GameContent.Items.Shields
{


    class OmegaShield
    {


        public static Item Make()
        {
            ShieldData data = new ShieldData("Omega Shield");
            data.Capacity = 50000;
            data.GenerationRatePerSec = 10*60;
            data.AbilitiyActivation = ControlSignals.OnDamageToHull;
            data.AblitiyCooldown = 60 * 5;
            data.ItemData.BuyPrice = 10000;
            GroupEmitter ability = new GroupEmitter();

            ability.AddEmitter(MakeStun());
            ability.AddEmitter(MakeActiveShield());
            ability.AddEmitter(new MeterChangeEmitter());
            data.AbilityEmitter = ability;
            data.ItemData.IconID = "Shield0";

            Item item = ShieldQuickStart.Make(data);
            return item;
        }

        public static ProjectileProfile MakeStun()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "add3";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatConst(150); //Parent
            projectileProfile.UpdateSize = new UpdateSizeGrow(20, 1.11f, 3600);
            projectileProfile.InitMaxLifetime = new InitFloatConst(90);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(new Color(100, 100, 155));
            projectileProfile.CollisionSpec = new CollisionSpec(0, 15f, 60 * 7 + 10);
            projectileProfile.VelocityInertia = 0.8f; //??
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //??
            projectileProfile.Draw = new ProjectileDrawRotateWithTime(0.01f, -0.012f, "add3", "add3");
            return projectileProfile;
        }

        public static ProjectileProfile MakeActiveShield()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "Shield";
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "add11";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSize = new InitFloatParentSize(3f, 0);
            profile.InitMaxLifetime = new InitFloatConst(80);
            profile.Mass = 0.1f;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsDestroyedWhenParentDestroyed = true;
            profile.IsEffectedByForce = false;
            profile.UpdateSize = new UpdateSizeGrow(20, 1.1f, 900);
            profile.CollisionType = CollisionType.CollideAll;
            return profile;
        }
    }
}

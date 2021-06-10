using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.LootCrates
{
    class CrateB
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Name = "Loot Crate";
            profile.IsTurnedByForce = true;
            profile.RotationInertia = 0.95f;
            profile.DrawType = DrawType.Lit;
            profile.TextureID = "container"; //add ship weark crate
            profile.CollisionWidth = profile.Sprite.Width - 5;
            profile.InitHitPoints = new InitFloatConst(10);
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            profile.InitSizeID = "100";
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            //profile.ObjectType |= GameObjectType.Collectible;
            profile.VelocityInertia = 0.98f;
            profile.Mass = 3f;
            profile.CollisionSpec = new CollisionSpec(0, 3);
            IEmitter loot = ContentBank.Inst.GetEmitter("CrateBLoot");
            //loot.AddEmitter("MatA1",1, 4, 10);
            //loot.AddEmitter("MatB1", 1, 4, 10);
            //loot.AddEmitter("MatC0", 0.8f, 1, 2);
            //loot.AddEmitter("RepairKit1", 0.5f, 1, 3);  //one of
            //loot.AddEmitter("EnergyKit1", 0.5f, 1, 3);            
            //loot.AddEmitter("HomeBeaconKit", 0.5f, 1, 3);
            //loot.AddEmitter("AsteroidPullMine", 0.2f, 1, 2);
            //loot.AddEmitter("MatC1", 0.1f, 1, 2);

            GroupEmitter emitter = new GroupEmitter();
            emitter.AddEmitter(loot);
            emitter.AddEmitter("FullExplosionFx1");
            profile.HitPointZeroEmiiter = emitter;
            return profile;
        }
    }
}

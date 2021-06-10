using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;

namespace SolarConflict.GameContent.Items.Generated
{
    class MineGeneration
    {
        //TODO: add descriptions
        public static List<string> names = new List<string>() {"Mini", "AOE", "Gravity", "Goo blast", "Energy Drain", "EMP" };        
        public static List<string> seconderySprite = new List<string>() { "Impact Mine", "AOE Mine", "Gravity Mine", "Goo blast mine", "Energy Drain Mine", "EMP Mine" };
        public static List<string> descriptions = new List<string>() { "Low-yield explosive mine that deals damage on impact",
            "High-yield explosive that detonates on impact or after a fixed duration, dealing damage in an area",
            "Mine that detonates on impact or after a fixed duration, generating a deep gravity well",
            "Mine that detonates on impact or after a fixed duration, releasing a goo that slows any ships it touches",
            "Mine that detonates on impact or after a fixed duration, draining energy from all ships in an area",
            "Mine that detonates on impact or after a fixed duration, disabling all ships in an area for a short time" };
        public static List<string> emitters = new List<string>() {typeof(FireFx).Name, typeof(AoeDamage1).Name, typeof(GravityWellAoe).Name, typeof(GooBlast).Name, typeof(EnergyDrainAoe).Name, typeof(StunAoe).Name};
        public static List<float> priceMult = new List<float>() { 0.1f, 0.15f, 0.2f, 0.2f, 0.2f, 1 };
        

        public static IEmitter Make()
        {
            for (int i = 1; i < names.Count; i++)
            {                 
                ContentBank.Inst.AddContent(MakeItem(i));
            }
            return null;
        }

        public static Item MakeItem(int index)
        {
            //descriptions[index] + "\nactivate from the inventory."
            ItemData data = new ItemData(names[index] + " Mine", index, "spacemine3");
            data.IsRatiendOnDeath = true;
            data.BreaksClocking = false;      
            data.Category = ItemCategory.Consumable | ItemCategory.AsteroidMiningGear | ItemCategory.Mines;
            data.SlotType = SlotType.Consumable | SlotType.Ammo;
            data.MaxStack = 999;
            data.BuyPrice = (int)Math.Round(ScalingUtils.ScaleCost(index) * priceMult[index] / 10f) * 10;
            data.SellRatio = 0.5f;
            
            var item = ItemQuickStart.Make(data);
            var mineEmitter = MakeMine(index);
            BasicEmitterCallerSystem system = new BasicEmitterCallerSystem(ControlSignals.None, mineEmitter, 30);
            item.Profile.AmmoEmitter = mineEmitter;
            item.Profile.IsConsumed = true;
            item.Profile.IsWorkingInInventory = true;
            item.System = system;
            item.ID = "MineAmmo" + index.ToString();
            return item;
        }

        public static ProjectileProfile MakeMine(int index)
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
            groupEmitter.RefVelocityMult = 0; //Think about it
            groupEmitter.AddEmitter(emitters[index]);
            groupEmitter.AddEmitter("FireExplosionFx");
            profile.ImpactEmitter = groupEmitter;
            profile.TimeOutEmitter = groupEmitter;
            profile.CollisionSpec = new CollisionSpec(5, 0.5f);
            profile.IsDestroyedOnCollision = true; //projectile is terminated on impact
            profile.IsEffectedByForce = false;
            profile.VelocityInertia = 1;// 0.992f;
            return profile;
        }
    }
}

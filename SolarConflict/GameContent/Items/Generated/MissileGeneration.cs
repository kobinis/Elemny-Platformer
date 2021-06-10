using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Items.Generated
{
    class MissileGeneration
    {        

        public static void Make()
        {
            float damageBase = 80;
            for (int i = 0; i < MineGeneration.names.Count; i++)
            {                                
                ContentBank.Inst.AddContent(MakeItem(i, MineGeneration.names[i] + " Rocket",ContentBank.Inst.GetEmitter(MineGeneration.emitters[i]), i+1, "missileAmmo", i == 0 ? damageBase : 0, false));
                ContentBank.Inst.AddContent(MakeItem(i, MineGeneration.names[i] + " Missile", ContentBank.Inst.GetEmitter(MineGeneration.emitters[i]), i+1, "homingMissileAmmo", i == 0 ? damageBase : 0, true));
            }
        }

        public static Item MakeItem(int index, string name, IEmitter effectEmitter, int level, string iconID, float damage, bool isGuided)
        {
            if (isGuided)
                name = "Guided " + name;//a projectile that deals " + Palette.Highlight.ToTag(damage.ToString())
            ItemData data = new ItemData(name, 2, iconID);
            //, "Bang bang!, Ammo for Missile Launcher"
            data.IsRatiendOnDeath = true;
            data.BreaksClocking = false;
            var missileEmitter = MakeMissile(damage, effectEmitter, isGuided);
            
            data.Level = level;
            data.Category = ItemCategory.Missiles;
            data.SlotType = SlotType.Ammo;
            data.MaxStack = 100;
            float guidedPriceMult = 1;
            if (isGuided)
                guidedPriceMult = 1.2f;
            data.BuyPrice = (int)Math.Round(ScalingUtils.ScaleCost(level) * guidedPriceMult* MineGeneration.priceMult[index]/10)* 10 + 60;

            data.SellRatio = 0.5f;
            
            var item = ItemQuickStart.Make(data);
            item.Profile.AmmoEmitter = missileEmitter;
            item.Profile.IsConsumed = true;            
            item.Profile.IsActivatable = false;
            if (isGuided)
            {
                item.ID = "Missile";
                missileEmitter.ID = "MissileProj";
            }
            else
            {
                item.ID = "Rocket";
                missileEmitter.ID = "RocketProj";
            }
            item.ID += index.ToString();
            missileEmitter.ID += index.ToString();
            ContentBank.Inst.AddContent(missileEmitter);
            return item;
        }

        public static IEmitter MakeMissile(float damage, IEmitter emitter, bool isGuided)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            //projectileProfile.AggroRange = 2000;
            projectileProfile.DrawType = DrawType.Alpha;
            //projectileProfile.InitColor = new InitColorConst(colors[i]);
            // projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectileProfile.TextureID = "item2";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 10;
            projectileProfile.InitSizeID = "15";
            projectileProfile.UpdateSize = null;
            projectileProfile.MovementLogic = new MoveForward(0.6f, 7);
            projectileProfile.InitMaxLifetime = new InitFloatConst(180);
            projectileProfile.Mass = 0.1f;
            //projectileProfile.u
            projectileProfile.UpdateEmitterID = typeof(EmitterFxSmoke).Name;
            projectileProfile.UpdateEmitterCooldownTime = 2;
            if (isGuided)
                projectileProfile.RotationLogic = new UpdateRotationHoming();
            projectileProfile.ImpactEmitter = emitter;// "FireFx";
            projectileProfile.TimeOutEmitterID = "EmitterDebris1";
            projectileProfile.CollisionSpec = new CollisionSpec(damage, 0.5f);
            projectileProfile.CollisionSpec.IsDamaging = true;
            projectileProfile.IsDestroyedOnCollision = true;
            projectileProfile.IsEffectedByForce = false;
            projectileProfile.CollisionType = CollisionType.Collide1;
            return projectileProfile;
        }
    }
}

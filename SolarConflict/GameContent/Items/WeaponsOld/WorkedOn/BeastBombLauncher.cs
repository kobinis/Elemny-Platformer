using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.WeaponsOld.WorkedOn
{
    class BeastBombLauncher
    {
        public static Item Make() => WeaponQuickStart.Make(MakeData());

        public static WeaponData MakeData()
        {
            ParamEmitter wormEmitter = new ParamEmitter();
            wormEmitter.RefVelocityMult = 0;
            wormEmitter.EmitterID = "Worm1";
            wormEmitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            wormEmitter.PosRadMin = 30;
            wormEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            wormEmitter.PosAngleRange = 360;
            wormEmitter.RotationType = ParamEmitter.EmitterRotation.PosAngle;
            wormEmitter.RotationRange = 360;
            wormEmitter.MinNumberOfGameObjects = 3;

            GroupEmitter ge = new GroupEmitter();
            ge.AddEmitter(wormEmitter);
            ge.AddEmitter("BigBloodSplashFx"); //Change to green explotion

            ProjectileProfile profile = new ProjectileProfile();
            profile.TextureID = "WormSegment1";
            profile.CollideWithMask = GameObjectType.Agent;
            profile.CollisionWidth = profile.Sprite.Height - 5;
            profile.InitColor = new InitColorConst(Color.Green);
            profile.InitSizeID = "30";
            profile.IsDestroyedOnCollision = true;
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.ImpactEmitter = ge;
            profile.TimeOutEmitter = ge;
            profile.ID = "BeastBombLauncher_Shot";
            if (!ContentBank.Inst.ContainsEmitter(profile.ID))
                ContentBank.Inst.AddContent(profile);

            //"Shoots munitions that explode into horror."
            WeaponData weaponData = new WeaponData("Beast Bomb Launcher", 5, "WormSegment1", null);
            weaponData.ShotEmitterID = "BeastBombLauncher_Shot";
            weaponData.KickbackForce = 0.15f;
            weaponData.Cooldown = Utility.Frames(30);
            weaponData.ShotSpeed = 25;
            weaponData.ItemData.BuyPrice = 1600;
            weaponData.ItemData.SellRatio = 0.5f;
            weaponData.ItemData.Category = ItemCategory.NonAI;

            return weaponData;
        }
    }
}

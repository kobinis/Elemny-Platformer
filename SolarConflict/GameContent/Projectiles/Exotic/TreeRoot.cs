﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.NewContent.Projectiles
//{
//    class TreeRoot
//    {
//        public static ProjectileProfile Make() //make things depentend on param, make an option to set param frpm outside
//        {
//            //make an emitter theat works when life time is == the a certine time
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            //profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeInOut;
//            profile.TextureID = "lightGlow";
//            profile.CollisionWidth = profile.Texture.Width - 10;
//            profile.InitSizeID = "20";
//            profile.UpdateSize = null;
//            profile.InitMaxLifeTimeID = "4";
//            profile.InitParamID = "8";
//            profile.Mass = 0.1f;
//            profile.InitHitPointsID = "100";
//            EmitterGroup groupEmitter = new EmitterGroup();
//            groupEmitter.EmitType = EmitterGroup.EmitterType.RandomProb;
//            groupEmitter.AddEmitter(typeof(TreeShot).Name);
//            groupEmitter.ProbabilityList.Add(1f);
//            ParamEmitter emitter = new ParamEmitter();
//            emitter.Emitter = groupEmitter;
//            emitter.MinNumberOfGameObjects = 2;
//            emitter.RangeNumberOfGameObject = 0;
//            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
//            emitter.VelocityAngleRange = 90;
//            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
//            emitter.VelocityMagMin = 5;
//            //emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            
//            profile.TimeOutEmitter = emitter;
//            profile.CollusionSpec = new CollusionInfo(10, 0.5f);
//            profile.IsDestroyedOnCollusion = true; //maybe false
//            profile.IsEffectedByForce = false;
//            return profile;
//        }
//    }
//}

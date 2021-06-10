//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Projectiles
//{
//    class Net
//    {
//        public static ProjectileProfile Make()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOutSlow;
//            profile.TextureId = "net";
//            profile.DisplayScale = profile.Texture.Width - 10;
//            profile.InitSizeId = "10";
//            profile.UpdateSize = new UpdateSizeGrow(1, 1.05f, 400);
//            profile.InitMaxLifetimeId = "240";
//            profile.Mass = 0.1f;
//           // profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;

//            profile.EndOfLifeImpact = false;
//            profile.IsEffectedByForce = false;
//            //profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.8f, 15); 
//            //profile.UpdateRotation = new UpdateRotationForward();
//            profile.ImpactSpec = new ImpactInfo(10, 0f);
//            profile.VelocityInertia = 0.9f;
//            profile.ImpactSpec.ForceType = ForceType.Mult;
//            profile.Draw = new ProjectileDrawRotateWithTime(0.001f, -0.0012f, null);
//            profile.ImpactSpec.AddEntry(MeterType.StunTime, 60, ImpactType.Max);
//            profile.ImpactSpec.AddEntry(MeterType.Energy, 0); //takes 0 energy
//            profile.ImpactSpec.AddEntry(MeterType.Shield, -5); //takes 5 Shield
//            profile.ImpactSpec.AddEntry(MeterType.Hitpoints, 0); //takes 20 hitpoint, ignores Shield

//            return profile;
//        }
//    }
//}

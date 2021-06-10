using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class FxRock
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            //profile.Id = "FxRock";
            //display
            profile.DrawType = DrawType.Alpha;
            profile.TextureID = "spacerock10000";

            //var multiSheetDraw = new ProjectileDrawMultipleSpritesheets(16,new Spritesheet("Rock1Sheet0", 453, 429, 16),
            //    new Spritesheet("Rock1Sheet1", 453, 429, 16), new Spritesheet("Rock1Sheet2", 453, 429, 16),
            //    new Spritesheet("Rock1Sheet3", 453, 429, 16), new Spritesheet("Rock1Sheet4", 453, 429, 16),
            //    new Spritesheet("Rock1Sheet5", 453, 429, 10));
            //ProjectileDrawAni draw = new ProjectileDrawAni();
            //for (int i = 10000; i <= 10178; i += 2)
            //{
            //    draw.AddTextureId("spacerock" + i.ToString());
            //}
            profile.CollisionWidth = profile.Sprite.Width - 180;
            //multiSheetDraw.paramMult = 1f;
           // multiSheetDraw.lifeTimeMult = 1;
           // profile.Draw = multiSheetDraw;                        
         //   profile.UpdateList.Add(new UpdateParamSumVelocity());

            profile.ColorLogicID = "FadeOutSlow";

            profile.InitMaxLifetimeID = "50";
            profile.InitParamID = "InitFloatRandom,0,10000";           
            profile.InitSizeID = "InitFloatRandom,6,10";

            profile.UpdateSize = new UpdateSizeGrow(-0.1f);

           

            profile.CollisionType = CollisionType.Effects; //updateOnScreen
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}

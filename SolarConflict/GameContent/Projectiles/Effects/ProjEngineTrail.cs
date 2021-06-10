using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Projectiles.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class ProjEngineTrail//EngineTrailFx
    {
        public static ProjectileProfile Make()
        {
            var lifetime = 5;

            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
           // projectileProfile.InitColor = new InitColorFaction();
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;//ProjectileProfile.ColorUpdate.FadeInOut;
            projectileProfile.TextureID = "add8";//"add8");
            projectileProfile.ScaleMult = 2.5f / (float)(projectileProfile.Sprite.Width);
            projectileProfile.InitSize = new InitFloatConst(15);
            projectileProfile.UpdateSize = new UpdateSizeGrowMult(0.95f);
            // projectileProfile.InitParam = new InitFloatRandom(-2, 2);
            projectileProfile.InitMaxLifetime = new InitFloatConst(lifetime);


            //projectileProfile.Light =  new PointLight(new Vector3(1f, 0.5f, 0.2f), 500, 1f, 0f);

            //projectileProfile.Light = Lights.ContactLight(new Vector3(1f, 0.8f, 0.8f));
            //(projectileProfile.Light.IntensityCalculator as IntensityCalculators.InverseMononomial).BaseIntensity /= (lifetime / 3);
            // ^ Note that we don't _fully_ compensate for the number of trail particles that can coexist: total intensity will be pretty high,
            // but it'll attenuate really quickly with distance, so the trail still won't illuminate the parent ship too much            
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
    }
}

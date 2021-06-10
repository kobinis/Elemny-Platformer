using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw
{
    [Serializable]
    class DrawBeam : BaseDraw
    {
        public override void Draw(Camera camera, Projectile projectile)
        {
            Vector2 origin =  projectile.Parent.Position;
            Vector2 destenation = projectile.Position;   //projectile.Position;
            Vector2 diff = destenation - origin;
            float length = diff.Length() ;
            Camera.NormalMapEffect.Parameters["BeamMaxLength"].SetValue(length);
            Camera.NormalMapEffect.Parameters["BeamLength"].SetValue(length);
            Camera.NormalMapEffect.Parameters["BeamLifetime"].SetValue((float)projectile.GetAgentAncestor().Lifetime * 0.045f);
            float rotation = (float)Math.Atan2(diff.Y, diff.X);
            float beamWidth = projectile.profile.Sprite.Height *0.5f; //TODO add *size
            //Rectangle source = new Rectangle((int)projectile.Parent.Lifetime * -10, 0, projectile.profile.Sprite.Width, projectile.profile.Sprite.Height);

            camera.SpriteBatch.Draw(projectile.profile.Sprite, new Rectangle((int)origin.X, (int)origin.Y, (int)length, (int)beamWidth), null, projectile.color, rotation, new Vector2(0, projectile.profile.Sprite.Height / 2f), SpriteEffects.None, 0);            
        }
    }
}

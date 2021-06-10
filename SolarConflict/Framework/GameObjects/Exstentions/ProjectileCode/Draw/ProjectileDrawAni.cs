using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{
    /// <summary>
    /// Draws a projectile according to the size
    /// </summary>
    [Serializable]
    public class ProjectileDrawAni : BaseDraw
    {
        private List<Sprite> _spriteList;       
        public float lifeTimeMult, paramMult, globalTimeMult;//hitPointMult, normalizedLifeTime
        



        public ProjectileDrawAni()
        {
            _spriteList = new List<Sprite>();
        }

        public void AddTextureId(string textureId)
        {
            _spriteList.Add(Sprite.Get(textureId));
        }
        

        public override void Draw(Camera camera, Projectile projectile)
        {
           
            int index = (int)(projectile.Lifetime * lifeTimeMult + projectile.Param * paramMult + Game1.time * globalTimeMult);
           
            //projectile.param += projectile.Velocity.Length() * Math.Sign(projectile.Velocity.X);
            
         //   int index = (int)(Math.Abs(projectile.param) / frameDelay) % textureList.Count; 
            
           // int index = (int)(Math.Abs(projectile.Position.X + projectile.Position.Y*0.9f)/frameDelay  ) % textureList.Count; //???
                                            //+param * textureList.count
           // int index = (int)(Math.Abs(projectile.lifetime) / frameDelay) % textureList.Count; //???
            int n = _spriteList.Count;
            Sprite texture = _spriteList[(index % n +n) % n];

            

            camera.CameraDraw(texture, projectile.Position, projectile.Rotation, projectile.Size * projectile.profile.ScaleMult, projectile.color);
        }

    }    
}

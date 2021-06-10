using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{

    [Serializable]
    public class ProjectileDrawMultipleSpritesheets : BaseDraw
    {
        Spritesheet[] _sheets;
        int _numSprites;
        int _spriteNumPerSheet = 1;

        public float lifeTimeMult, paramMult, globalTimeMult;//hitPointMult, normalizedLifeTime

        public ProjectileDrawMultipleSpritesheets(int spriteNumPerSheet, params Spritesheet[] sheets)
        {
            _spriteNumPerSheet = spriteNumPerSheet;
            _sheets = sheets.ToArray();
            _numSprites = sheets.Sum(s => s.NumSprites);
        }

        public override void Draw(Camera camera, Projectile projectile)
        {
            var spriteIndex = ((int)(projectile.Lifetime * lifeTimeMult + projectile.Param * paramMult + Game1.time * globalTimeMult)) % _numSprites;
            if (_spriteNumPerSheet == 0)
                _spriteNumPerSheet = 16;
            var sheetIndex = spriteIndex / _spriteNumPerSheet;

            spriteIndex -= sheetIndex * _spriteNumPerSheet;
            camera.CameraDraw(_sheets[sheetIndex], spriteIndex, projectile.Position, projectile.Rotation, projectile.Size * projectile.profile.ScaleMult, projectile.color, SpriteEffects.None);
        }
    }
}

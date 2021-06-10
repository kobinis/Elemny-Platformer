using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;
using SolarConflict.GameContent;

namespace SolarConflict.NewContent.Projectiles
{
    class FirstGateRight
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = FirstGateLeft.Make();
            profile.Draw = new DrawFlipHorizontal(profile.Sprite);
            return profile;
        }
    }
}


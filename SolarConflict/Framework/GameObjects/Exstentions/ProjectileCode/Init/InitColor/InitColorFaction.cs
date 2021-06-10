using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework;

namespace SolarConflict
{
    [Serializable]
    public class InitColorFaction: BaseInitColor
    {
        public byte Alpha;

        public InitColorFaction()
        {
            Alpha = 255;
        }

        public InitColorFaction(int alpha)
        {
            Alpha =(byte)alpha;
        }

        public override Color Init(Projectile projectile, GameEngine gameEngine)
        {
            FactionType faction = projectile.GetFactionType();
            return FactionColorIndicator.factionColors[(int)faction % FactionColorIndicator.factionColors.Length]; //changeit
            //throw new Exception("fix");
            //return Color.White;
        }
    }
}

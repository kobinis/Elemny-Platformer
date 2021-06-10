using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict.Framework
{
    public interface IHudComponent
    {
        //bool IsActive { set; get; }
        //Vector2 Position { set; get; }
        void Update(Scene scene, Agent player);
        void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos);
        Rectangle GetSize();
    }
}

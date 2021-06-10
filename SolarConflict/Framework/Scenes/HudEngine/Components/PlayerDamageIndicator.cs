using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;
using XnaUtils.Framework.Graphics;
using SolarConflict.Generation;
using XnaUtils;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Inticades to the player that thay take sgnificent damage. (screen flash red)
    /// </summary>
    [Serializable]
    class PlayerDamageIndicator : IHudComponent
    {
        private Agent lastPlayer;
        //private float playerHitpoints;
        private float frameDamage = 0;
        private float damageSum;
        private float damageTreshold = 100;
        [NonSerialized]
        static Texture2D blank = TextureBank.Inst.GetTexture("blank");

        public void Update(Scene scene, Agent player)
        {
            frameDamage = 0;
            if (player != null)
            {
                if (scene.PlayerAgent == lastPlayer && scene.PlayerAgent != null)
                {
                    frameDamage = scene.PlayerAgent.GetDamageTaken();
                    damageTreshold = Math.Max(player.GetTotalHitpoints(), 1) * 0.05f;
                }
               
            }
            lastPlayer = scene.PlayerAgent;
            damageSum += frameDamage;
            damageSum *= 0.9f;           
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);     
            float alpha = MathHelper.Clamp((damageSum- damageTreshold) / 10f,0, 0.5f);
            Color color = new Color(1, 0.1f, 0.1f, alpha);
            spriteBatch.Draw(blank, ActivityManager.ScreenRectangle, color);
            spriteBatch.End();          
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }

    }
}

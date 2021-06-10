using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    [Serializable]
    public class MiniMap : IHudComponent //TODO:
    {
        int space = 0;
        private readonly int SCAN_RANGE = 14000;
        private readonly int MAP_SIZE = 110;      
        private Sprite _shipIndicatorTexture;        
        private Sprite _backgroundTexture;        
        List<GameObject> objectsInRange;
        private Sprite _maskTexture;

        public static Effect minimapEffect;

        public MiniMap()
        {          
            _shipIndicatorTexture = Sprite.Get("glow128");
            _backgroundTexture = Sprite.Get("minimapV2");
            _maskTexture = Sprite.Get("minimapMask");
            objectsInRange = new List<GameObject>();
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            //Pos is center of the element
            objectsInRange.Clear();
            
            if (player != null)
            {
                /*
                spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive, null, Masking.DrawToStencil(), null, Masking.AlphaEffect);
                scene.GameEngine.CollisionManager.GetAllObjectInRange(player.Position, SCAN_RANGE, objectsInRange);
        
                Rectangle backgroundRect = new Rectangle((int)pos.X - MAP_SIZE, (int)pos.Y - MAP_SIZE, (MAP_SIZE) * 2, (MAP_SIZE) * 2);
                spriteBatch.Draw(_backgroundTexture, backgroundRect, Color.Black);
                spriteBatch.End();*/

                Vector2 offset = new Vector2(15, 15);
                pos += offset;
                //Get objects
                scene.GameEngine.CollisionManager.GetAllObjectInRange(player.Position, SCAN_RANGE, objectsInRange);

                if(_maskTexture != null)
                    ShadersEffects.MiniMapEffect.Parameters["MaskMap"].SetValue(_maskTexture);

                ShadersEffects.MiniMapEffect.Parameters["Viewport"].SetValue(new Vector2(ActivityManager.GraphicsDevice.Viewport.Width, ActivityManager.GraphicsDevice.Viewport.Height));


                ShadersEffects.MiniMapEffect.Parameters["MaskStart"].SetValue(new Vector2(pos.X - MAP_SIZE, pos.Y - MAP_SIZE)); //Left top corner

                ShadersEffects.MiniMapEffect.Parameters["MaskSize"].SetValue(new Vector2(MAP_SIZE * 2, MAP_SIZE * 2)); //True map size

                //ActivityManager.MiniMapEffect.CurrentTechnique = ActivityManager.MiniMapEffect.Techniquesú
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, ShadersEffects.MiniMapEffect);
                space = 5;
                Rectangle backgroundRect = new Rectangle((int)pos.X - MAP_SIZE, (int)pos.Y - MAP_SIZE, (MAP_SIZE) * 2, (MAP_SIZE) * 2);

                //background
                ShadersEffects.MiniMapEffect.CurrentTechnique = ShadersEffects.MiniMapEffect.Techniques["PassThrough"];
                spriteBatch.Draw(_backgroundTexture, backgroundRect, Color.White);

                //Masking
                ShadersEffects.MiniMapEffect.CurrentTechnique = ShadersEffects.MiniMapEffect.Techniques["MinimapMasking"];
                //Icons in minimap
                foreach (var gameObject in objectsInRange)
                {
                    //(gameObject.GetObjectType() & (GameObjectType.PotentialTarget | GameObjectType.VisibleInMiniMap)) > 0
                    if (gameObject.ListType == CollisionType.CollideAll)
                        DrawObjectInMinimap(spriteBatch, player, gameObject, scene.GameEngine, pos);
                }
                spriteBatch.End();
            }
        }

        private void DrawObjectInMinimap(SpriteBatch spriteBatch, Agent player, GameObject gameObject, GameEngine gameEngine, Vector2 pos) //TODO: add the sun draw with mask
        {
            Vector2 position = gameObject.Position - player.Position;            
            if (position.Length() < SCAN_RANGE + gameObject.Size)
            {
                int dotSize = Math.Max((int)(gameObject.Size * MAP_SIZE / SCAN_RANGE * 2f), 4);
                position = position * MAP_SIZE / SCAN_RANGE + pos;
                Rectangle positionRecangle = new Rectangle((int)position.X - dotSize, (int)position.Y - dotSize, dotSize * 2, dotSize * 2);
                Color objectColor = Color.Gray;
                objectColor = FactionColorIndicator.FactionToColor(gameObject.GetFactionType());                
                spriteBatch.Draw(_shipIndicatorTexture, positionRecangle, objectColor);                
            }
        }

        public void Update(Scene scene, Agent player)
        {
        }

        public Rectangle GetSize()
        {
            return new Rectangle((int) - MAP_SIZE - space, (int) - MAP_SIZE - space, (MAP_SIZE + space) * 1, (MAP_SIZE + space) * 1);
        }
    }
}

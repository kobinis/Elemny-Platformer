using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes
{
    [Serializable]
    class FactionColorIndicator : IHudComponent
    {
        [NonSerialized]
        static Sprite glow = Sprite.Get("smallGlow"); //ToDo: change it to get from static texture load from content
        static float textureScale = 2 / (float)glow.Width;
        //public static Color[] factionColors = { Color.Blue, Color.Green, Color.Red, Color.Purple, Color.Yellow, Color.Cyan, Color.MediumOrchid };
        //public static Color[] factionColors = { Color.Gray, Color.Green, Color.Blue , Color.Purple, Color.Yellow, Color.Red, Color.Cyan, Color.Silver }; //REFACTOR: remove
        public static Color[] factionColors = { new Color(50,50,255), Color.Green, Color.Blue, Color.Yellow, Color.Azure, Color.Red, Color.Purple * 0.6f, Color.Gray, Color.MediumOrchid, Color.Red, Color.Red }; 


        static FactionColorIndicator()
        {
            //for (int i = 0; i < factionColors.Length; i++)
            //{
                
            //    factionColors[i].R =  (byte)Math.Min(factionColors[i].R + 30,255);
            //    factionColors[i].G = (byte)Math.Min(factionColors[i].G + 30, 255);
            //    factionColors[i].B = (byte)Math.Min(factionColors[i].B + 30, 255);
            //    factionColors[i].A = 255;                
            //}
        }
        public static Color FactionToColor(FactionType faction)
        {
            return FactionToColor((int)faction);
        }

        public static Color FactionToColor(int factionId)
        {
            return factionColors[factionId % factionColors.Length];
        }

        public  FactionColorIndicator()
        {
            //change it
            for (int i = 0; i < factionColors.Length; i++)
            {
                Vector3 colorVec = factionColors[i].ToVector3();// +new Vector3(0.1f);
                factionColors[i] = new Color(colorVec);

            }
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            Draw(scene.Camera, scene, player);
        }

        public void Draw(Camera camera, Scene scene, GameObject player)
        {                        

            foreach (var gameObject in scene.GameEngine._collideAllCheckList) //Get items around screen
            {
                if (camera.IsOnScreen(gameObject.Position, gameObject.Size, 0))
                {
                    if (!gameObject.IsCloaked)
                    {
                        if (gameObject != player)
                        {
                            //TODO: change
                            Color color = factionColors[Math.Min((int)gameObject.GetFactionType(), factionColors.Length - 1)];
                            camera.CameraDraw(glow, gameObject.Position, 0, gameObject.Size * textureScale, color);
                        }
                        else
                        {
                            //  camera.CameraDraw(glow, gameObject.Position, 0, gameObject.Size * textureScale, Color.LightGreen);
                        }
                    }
                }
                else
                {
                    if ((player != null && gameObject.GetFactionType() == player.GetFactionType()) || !gameObject.IsCloaked)
                    {
                        Color color = factionColors[Math.Min((int)gameObject.GetFactionType(), factionColors.Length - 1)];
                        Vector2 position = camera.GetScreenPos(gameObject.Position);
                        position.X = MathHelper.Clamp(position.X, 0, ActivityManager.ScreenWidth); //change
                        position.Y = MathHelper.Clamp(position.Y, 0, ActivityManager.ScreenHeight); //change
                        int rad = 20;
                        camera.SpriteBatch.Draw(glow, new Rectangle((int)position.X - rad, (int)position.Y - rad, 2 * rad, 2 * rad), color);
                        //camera.CameraDraw(glow, gameObject.Position, 0, gameObject.Size * textureScale, color);
                    }
                }
            }
        }

        public void Update(Scene scene, Agent player)
        {
            
        }

        public Rectangle GetSize() { return new Rectangle(); }
    }
}

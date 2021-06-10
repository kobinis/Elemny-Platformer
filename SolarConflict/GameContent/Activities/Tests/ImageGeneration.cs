using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using SolarConflict.XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;
using SolarConflict.XnaUtils.Files;
using System.IO;

namespace SolarConflict.GameContent.Activities.Tests
{
    class ImageGeneration:Activity
    {
        Texture2D texture;
        protected override void Init(ActivityParameters parameters)
        {
            int size = 64;
            //TextureDesign desgin = new TextureDesign();
            //desgin.BaseBrightness = 1; //TODO: remove BaseBrightness from TextureDesign class
            //desgin.FrameDesign = FrameDesign.None;
            //desgin.FrameSize = 5;
            //desgin.FadeFrames = 3;
            //desgin.FrameDesign = FrameDesign.Normal;
            //desgin.FrameColor = Color.LightGray;
            //desgin.BodyColor = Color.LightGray;


            TextureDesign desgin = new TextureDesign();
            desgin.BaseBrightness = 1; //TODO: remove BaseBrightness from TextureDesign class
            desgin.FrameDesign = FrameDesign.None;
            desgin.FrameSize = 3;
            desgin.FadeFrames = 3;
            desgin.FrameColor = new Color(new Color(0, 224, 216).ToVector3() * 1.1f);
            desgin.BodyColor = new Color(new Color(0, 22, 26).ToVector3() * 1.1f); ;//new Color(0, 22, 26);
            TextureBank.Inst.AddTexture("guitexture1", TextureGenerator.GenerateTexture(64, 64, desgin));
            TextureBank.Inst.AddSprite(new Sprite9Sliced("guitexture1", 10, 10, 10, 10));

            //desgin.CornerSize = 0;
            texture = TextureGenerator.GenerateTexture(size, size, desgin);

            Stream stream = File.OpenWrite("guif9.png");
            texture.SaveAsPng(stream, texture.Width, texture.Height);
            stream.Close();


        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(texture, Vector2.One * 20, Color.White);
            sb.End();
        }

        public override void Update(InputState inputState)
        {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
        }

        private void MakeGuiDesign()
        {
            TextureDesign desgin = new TextureDesign();
            desgin.BaseBrightness = 1; //TODO: remove BaseBrightness from TextureDesign class
            desgin.FrameDesign = FrameDesign.None;
            desgin.FrameSize = 3;
            desgin.FadeFrames = 3;
            desgin.FrameColor = new Color(new Color(0, 224, 216).ToVector3() * 1.1f);
            desgin.BodyColor = new Color(new Color(0, 22, 26).ToVector3() * 1.1f); ;//new Color(0, 22, 26);
            TextureBank.Inst.AddTexture("guitexture1", TextureGenerator.GenerateTexture(64, 64, desgin));
            TextureBank.Inst.AddSprite(new Sprite9Sliced("guitexture1", 10, 10, 10, 10));
        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new ImageGeneration();
        }
    }
}

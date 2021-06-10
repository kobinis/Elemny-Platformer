using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict;
using System;
using XnaUtils;

public class FrameRateCounter : DrawableGameComponent
{
    SpriteBatch spriteBatch;
    SpriteFont spriteFont;

    int frameRate = 0;
    int frameCounter = 0;
    TimeSpan elapsedTime = TimeSpan.Zero;


    public FrameRateCounter(Game game)
        : base(game)
    {
        spriteFont = Game1.font;
        spriteBatch = Game1.sb;
    }



    public override void Update(GameTime gameTime)
    {        
        elapsedTime += gameTime.ElapsedGameTime;

        if (elapsedTime > TimeSpan.FromSeconds(1))
        {
            elapsedTime -= TimeSpan.FromSeconds(1);
            frameRate = frameCounter;
            frameCounter = 0;
        }
    }


    public override void Draw(GameTime gameTime)
    {
        frameCounter++;
        string fps = string.Format("fps: {0}", frameRate);
        //string fps = "Beta";
        Vector2 size = spriteFont.MeasureString(fps);
        spriteBatch.Begin();
        spriteBatch.DrawString(spriteFont, fps,  ActivityManager.ScreenSize - size - Vector2.One * 10, Color.White);
        spriteBatch.End();
    }
}
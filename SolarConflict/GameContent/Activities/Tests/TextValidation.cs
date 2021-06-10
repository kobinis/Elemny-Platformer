using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Activities {
    
    /// <summary>Draws all our text, fishes for errors</summary>
    class TextValidation : Activity
    {
        

        public override void Draw(SpriteBatch batch)
        {

            batch.Begin();
            var parser = new RichTextParser(Game1.font);
            
            var textAssets = TextBank.Inst.AllAssets;
            Game1.font.DefaultCharacter = null;
            // Assets
            foreach (var item in textAssets)
            {

                if(item.NextText != null && !item.NextText.Contains(":") && !TextBank.Inst.Contains(item.NextText))
                {
                    ActivityManager.Inst.AddToast("Error at: " + item.ID + "\nNo Next Text:" + item.NextText , 300);
                }

                try
                {
                    if (item.Text != null)
                    {
                        DrawText(batch, parser, item.Text);
                    }
                }
                catch (Exception e)
                {
                    Game1.font.DefaultCharacter = '*';
                    ActivityManager.Inst.AddToast("Error at: "+item.ID +"\n" + item.Text, 300); 
                }
                
            }

            //.Where(t => t.Text != null).Do(t => DrawText(batch, parser, t.Text));

            // Item texts
            ContentBank.Inst.GetAllItems().Do(i => DrawText(batch, parser, i.GetTooltipText(true, true, true, true)));
            Game1.font.DefaultCharacter = '*';
            batch.End();            
        }

        void DrawText(SpriteBatch batch, RichTextParser parser, string text) {
            // Draw with parser
            parser.Text = text;
            parser.Parse();
            parser.Draw(batch, Vector2.Zero);
            
            // And without
            batch.DrawString(Game1.font, text, Vector2.Zero, Color.White);
        }

        public override void Update(InputState inputState) {
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new TextValidation();
        }
    }
}

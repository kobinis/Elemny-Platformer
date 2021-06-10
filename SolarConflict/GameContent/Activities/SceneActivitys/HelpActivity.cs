using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes.Activitys;
using XnaUtils.Graphics;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.GameContent.Activities
{
    class HelpActivity : SceneActivity
    {
        GuiManager _gui;
        private Texture2D _cover;

        

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);

            _gui = new GuiManager();
            var layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            layout.AddChild(new RichTextControl(TextBank.Inst.GetString(parameters.Parameter), null, true));
            _gui.Root = layout;
            _cover = TextureBank.Inst.GetTexture("cover2");
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            GraphicsUtils.DrawBackground(_cover, sb);
            sb.End();
            _gui.Draw();
            base.Draw(sb);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new HelpActivity();
        }
        
    }
}

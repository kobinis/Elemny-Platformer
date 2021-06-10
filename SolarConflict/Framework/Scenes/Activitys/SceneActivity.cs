using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.PlayersManagement;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
using System.Collections.Generic;
using System.Linq;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.Scenes.Activitys
{
    public abstract class SceneActivity : Activity //TODO: change name
    {
        protected VerticalLayout _guiLayout;
        protected BackButton _back;        
        protected GuiManager _sideGui;
        protected SceneComponentSelector.Component _component;
        protected Sprite _cover;

        protected Scene _scene;
        protected Agent _calling_agent;

        public SceneActivity(bool isPopup = true)
        {
            IsPopup = isPopup;
            _sideGui = new GuiManager();
             _guiLayout = new VerticalLayout(Vector2.Zero);            
            _sideGui.Root = _guiLayout;
            _back = new BackButton(Vector2.Zero, Vector2.One * 45);
            //_back.Position = Vector2.One * 10 + _back.HalfSize;
          //  _back.ActivationKey = Keys.Escape;
            _back.ControlColor = Palette.GuiFrame;
            _guiLayout.AddChild(_back);
            _guiLayout.Position = _guiLayout.HalfSize;
        }

        public void AddHelp(string text)
        {
            ImageControl helpControl = new ImageControl(Sprite.Get("gui1"), Vector2.Zero, Vector2.One * 45);
            helpControl.CursorOn += _sideGui.ToolTipHandler;
            helpControl.TooltipText = text;
            _guiLayout.AddChild(helpControl);
            _guiLayout.Position = _guiLayout.HalfSize;
        }

        protected override void Init(ActivityParameters parameters)
        {
            if (parameters != null)
            {
                _scene = parameters.GetObjectParam("Scene", _scene) as Scene;
                _component = parameters.GetObjectParam("Component", null) as SceneComponentSelector.Component;
                _calling_agent = parameters.GetObjectParam("Calling_agent", null) as Agent;


            }
            if (_component != null)
            {
                _back.ActivationKey = _component.Key;                
            }            
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            if(_cover != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                GraphicsUtils.DrawBackground(_cover.Texture, spriteBatch);
                spriteBatch.End();
            }
            _scene?.background.Draw(_scene.GameEngine.Camera);
            _scene?.GameEngine.Draw(spriteBatch);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            _scene?.SceneComponentSelector.Draw(spriteBatch);
           // spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
           // _back.Draw(spriteBatch);
           //// _helpControl.Draw(spriteBatch);
           // // _titleControl?.Draw(spriteBatch); //Remove?
           // spriteBatch.End();
            _sideGui.Draw();        
        }

        public override void Update(InputState inputState)
        {
            _scene?.SceneComponentSelector.Update(inputState);
            if (inputState.IsKeyPressed(Keys.Escape))//|| (_scene != null && _scene.PlayersManager.players[0].IsCommandClicked(PlayerCommand.Use)))
            {
                ActivityManager.Inst.Back();
            }
            _sideGui.Update(inputState);            
        }

        public override ActivityParameters OnBack()
        {            
            return base.OnBack();
        }

    }
}

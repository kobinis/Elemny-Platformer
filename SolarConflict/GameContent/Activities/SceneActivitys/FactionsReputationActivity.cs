using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaUtils.Framework.Graphics;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using SolarConflict.GameContent;

using XnaUtils.Graphics;
using SolarConflict.Framework.Scenes.Activitys;

namespace SolarConflict.GameContent.Activities.SceneComponents
{
    public class FactionsReputationActivity : SceneActivity
    {
        Camera _camera;
        List<Faction> _factions;
        Sprite _glow;
        SetPixel pixelFunc;
        GuiManager _guiHolderDiagram;
        GuiManager _guiHolder;
        Sprite _backSprite;

        public FactionsReputationActivity()
        {            
            _camera = new Camera();
            _camera.Zoom = 1;
            var factions = MetaWorld.Inst.GetFactions();
            _factions = factions.ToList<Faction>();
            _glow = Sprite.Get("smallLight");
            _backSprite = Sprite.Get("smoke2");

            pixelFunc = new SetPixel(PutImage);
            //TODO: Change to gui
            _guiHolderDiagram = new GuiManager();
            _guiHolderDiagram.Root = new GuiControl();
            _guiHolder = new GuiManager();
            _guiHolder.Root = new GuiControl();

            for (int i = 0; i < _factions.Count; i++)
            {
                float angle = i / (float)_factions.Count * MathHelper.TwoPi - MathHelper.PiOver2;
                Vector2 position = FMath.ToCartesian(ActivityManager.ScreenRectangle.Height * 0.3f, angle);
                Vector2 targetPosition = _camera.GetScreenPos(FMath.ToCartesian(ActivityManager.ScreenRectangle.Height * 0.3f, angle));                
                ImageControl image = new ImageControl(_backSprite, targetPosition, Vector2.One * 200);
                image.TooltipText = _factions[i].GetInfoText();
                image.CursorOn += _guiHolderDiagram.ToolTipHandler;
                image.ControlColor = _factions[i].Color;
                ImageControl frontImage = new ImageControl(_factions[i].LogoSprite, Vector2.Zero, Vector2.One * 150);
                frontImage.TooltipText = _factions[i]. GetInfoText();
                frontImage.CursorOn += _guiHolderDiagram.ToolTipHandler;
                image.AddChild(frontImage);                

                TextControl textControl = new TextControl(_factions[i].Name, Game1.font);
                textControl.LocalPosition = new Vector2(0, -image.HalfSize.Y - 10);
                image.AddChild(textControl);                             
                _guiHolderDiagram.Root.AddChild(image);
            }

            //_guiHolder.Root.AddChild(new BackButton(UIElmentsTexts.Exit, new Vector2(ActivityManager.screenRectangle.Height - 200, (ActivityManager.screenRectangle.Width / 2) + 80), Game1.menuFont));

        }


        public override void OnEnter(ActivityParameters parameters)
        {
            _camera = new Camera();
            _camera.Zoom = 1;
            var factions = MetaWorld.Inst.GetFactions();
            _factions = factions.ToList<Faction>();
            _glow = Sprite.Get("smallLight");
            _backSprite = Sprite.Get("smoke2");

            pixelFunc = new SetPixel(PutImage);
            //TODO: Change to gui
            _guiHolderDiagram = new GuiManager();
            _guiHolderDiagram.Root = new GuiControl();
            for (int i = 0; i < _factions.Count; i++)
            {
                float angle = i / (float)_factions.Count * MathHelper.TwoPi - MathHelper.PiOver2;
                Vector2 position = FMath.ToCartesian(ActivityManager.ScreenRectangle.Height * 0.3f, angle);
                Vector2 targetPosition = _camera.GetScreenPos(FMath.ToCartesian(ActivityManager.ScreenRectangle.Height * 0.3f, angle));
                ImageControl image = new ImageControl(_backSprite, targetPosition, Vector2.One * 200);
                image.TooltipText = _factions[i].GetInfoText();
                image.CursorOn += _guiHolderDiagram.ToolTipHandler;
                image.ControlColor = _factions[i].Color;
                ImageControl frontImage = new ImageControl(_factions[i].LogoSprite, Vector2.Zero, Vector2.One * 150);
                frontImage.TooltipText = _factions[i].GetInfoText();
                frontImage.CursorOn += _guiHolderDiagram.ToolTipHandler;
                image.AddChild(frontImage);

                TextControl textControl = new TextControl(_factions[i].Name, Game1.font);
                textControl.LocalPosition = new Vector2(0, -image.HalfSize.Y - 10);
                image.AddChild(textControl);
                _guiHolderDiagram.Root.AddChild(image);
            }
        }

        public override void Update(InputState inputState)
        {
            _guiHolderDiagram.Update(inputState);
            _guiHolder.Update(inputState);
            if (inputState.IsKeyPressed(Keys.Escape))
                ActivityManager.Inst.Back();            
        }

        public override void Draw(SpriteBatch sb)
        {
            
            _camera.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);            
            for (int i = 0; i < _factions.Count; i++)
            {
                float angle = i / (float)_factions.Count * MathHelper.TwoPi - MathHelper.PiOver2;
                Vector2 position = FMath.ToCartesian(ActivityManager.ScreenRectangle.Height * 0.3f, angle);
                for (int j = i+1; j < _factions.Count; j++)
                {
                    float relations = MetaWorld.Inst.GetFactionReleations(_factions[i].FactionType, _factions[j].FactionType);
                    float targetAngle = j / (float)_factions.Count * MathHelper.TwoPi - MathHelper.PiOver2;
                    Vector2 targetPosition = FMath.ToCartesian(ActivityManager.ScreenRectangle.Height * 0.3f, targetAngle);

                    Color color = Color.Yellow;
                    if (relations < 0)
                        color = Color.Red;
                    if (relations > 0)
                        color = Color.Green;
                    color.A = (byte)MathHelper.Clamp( 255 * Math.Abs(relations),70,255);

                    GraphicsUtils.Line(_camera.SpriteBatch, position, targetPosition, color, 5, pixelFunc);
                }
            }
            _camera.SpriteBatch.End();

            _guiHolderDiagram.Draw(Color.White);
            _guiHolder.Draw(Color.White);

            //_camera.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //for (int i = 0; i < _factions.Count; i++)
            //{

            //    float angle = i  /(float)_factions.Count * MathHelper.TwoPi - MathHelper.PiOver2;
            //    Vector2 position = FMath.ToCartesian(ActivityManager.screenRectangle.Height*0.3f, angle);                
            //    _factions[i].DrawLogo(_camera, position, 0.25f);
            //}                 
            //_camera.spriteBatch.End();
        }

        private void PutImage(SpriteBatch sb, Vector2 position, Color color)
        {
            _camera.CameraDraw(_glow, position, 0, 0.5f, color);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new FactionsReputationActivity();
        }

    }
}

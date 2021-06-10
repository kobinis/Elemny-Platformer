using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.Scenes;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    //TODO: add, save, revert, default
    class KeyBindingActivity:SceneActivity
    {
        GuiManager gui;
        bool isExpectingKey;
        RichTextControl clickedControl;
        string target = null;

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
             _cover = Sprite.Get("cover2");
            gui = new GuiManager();
            gui.Root = MakeGui();
        }

        public override void Update(InputState inputState)
        {
            if(isExpectingKey)
            {
                var pressedKey = inputState.GetPressedKey();
                if(!pressedKey.IsEmpty())
                {                    
                    if(pressedKey.Key != Keys.Escape)
                    {
                        if (target == "signals")
                        {
                            ControlSignals signal = (ControlSignals)(clickedControl.Data);
                            KeysSettings.Data.KeyBindings[signal] = pressedKey;
                        }
                        if(target == "commands")
                        {
                            PlayerCommand command = (PlayerCommand)(clickedControl.Data);
                            KeysSettings.Data.CommandBindings[command] = pressedKey;
                        }
                        if(target == "menu")
                        {
                            SceneComponentType type = (SceneComponentType)(clickedControl.Data);
                            KeysSettings.Data.MenuBindings[type] = pressedKey;
                        }
                        clickedControl.Text = "#color{0,255,255}" + pressedKey.GetTag();
                    }
                    clickedControl.ControlColor = Color.White;
                    clickedControl.CursorOverColor = Color.Yellow;
                    clickedControl.Width = 500;
                    clickedControl.Height = 80;
                    isExpectingKey = false;
                    inputState = InputState.EmptyState;
                }
            }
            base.Update(inputState);
            gui.Update(inputState);            
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            gui.Draw();
            base.Draw(spriteBatch);
        }

        public static Activity ActivityProvider(string param)
        {
            return new KeyBindingActivity();
        }

        public GuiControl MakeGui()
        {
            VerticalLayout layout = new VerticalLayout(ActivityManager.ScreenCenter);

            GridControl title = new GridControl(2, 1, new Vector2(510, 60));
            RichTextControl nameControl = new RichTextControl("Action", isShowFrame: false);
            title.AddChild(nameControl);
            RichTextControl keyTitle = new RichTextControl("Key");
            title.AddChild(keyTitle);
            layout.AddChild(title);

            ScrollableGrid grid = new ScrollableGrid(1, 6, new Vector2(1010, 90));
            
            foreach (var pair in KeysSettings.Data.KeyBindings)
            {
                GridControl keyControl = new GridControl(2, 1, new Vector2(500, 80));
                keyControl.Sprite = null;
                RichTextControl signalName = new RichTextControl(pair.Key.GetUserName(), isShowFrame:true); ;
                keyControl.AddChild(signalName);
                RichTextControl keyName = new RichTextControl("#color{255,255,0}"+pair.Value.GetTag(), isShowFrame: true);
                //keyName.Sprite = Sprite.Get("guif8");
                keyName.Data = pair.Key;
                keyName.UserData = "signals";
                keyName.CursorOverColor = Color.Yellow;
                keyName.Action += KeyName_Action;
                keyControl.AddChild(keyName);
                grid.AddChild(keyControl);
            }

            foreach (var pair in KeysSettings.Data.CommandBindings)
            {
                GridControl keyControl = new GridControl(2, 1, new Vector2(500, 80));
                keyControl.Sprite = null;
                RichTextControl signalName = new RichTextControl(pair.Key.GetUserName(), isShowFrame: true); ;
                keyControl.AddChild(signalName);
                RichTextControl keyName = new RichTextControl("#color{255,255,0}" + pair.Value.GetTag(), isShowFrame: true);
               // keyName.Sprite = Sprite.Get("guif8");
                keyName.Data = pair.Key;
                keyName.UserData = "commands";
                keyName.CursorOverColor = Color.Yellow;
                keyName.Action += KeyName_Action;
                keyControl.AddChild(keyName);
                grid.AddChild(keyControl);
            }

            foreach (var pair in KeysSettings.Data.MenuBindings)
            {
                GridControl keyControl = new GridControl(2, 1, new Vector2(500, 80));
                keyControl.Sprite = null;
                RichTextControl signalName = new RichTextControl(pair.Key.GetUserName(), isShowFrame: true); ;
                keyControl.AddChild(signalName);
                RichTextControl keyName = new RichTextControl("#color{255,255,0}" + pair.Value.GetTag(), isShowFrame: true);
                // keyName.Sprite = Sprite.Get("guif8");
                keyName.Data = pair.Key;
                keyName.UserData = "menu";
                keyName.CursorOverColor = Color.Yellow;
                keyName.Action += KeyName_Action;
                keyControl.AddChild(keyName);
                grid.AddChild(keyControl);
            }

            layout.AddChild(grid);

            GridControl saveAndDefalut = new GridControl(2, 1, new Vector2(510, 60));            
            RichTextControl save = new RichTextControl("Save",isShowFrame:true);
            save.CursorOverColor = Color.Yellow;
            save.Action += Save_Action;
            saveAndDefalut.AddChild(save);
            RichTextControl reset = new RichTextControl("Restore To Default", isShowFrame:true);
            reset.CursorOverColor = Color.Yellow;
            reset.Action += Reset_Action;
            saveAndDefalut.AddChild(reset);
            layout.AddChild(saveAndDefalut);
            return layout;
        }

        private void Reset_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            KeysSettings.LoadOrDefaultBindings(false);
            gui.Root = MakeGui();
        }

        private void Save_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            KeysSettings.SaveKeys();
        }

        private void KeyName_Action(GuiControl source, global::XnaUtils.Input.CursorInfo cursorLocation)
        {
            isExpectingKey = true;
            if (clickedControl != null)
            {
                clickedControl.ControlColor = Color.White;
                clickedControl.CursorOverColor = Color.Yellow;
            }
            target = source.UserData;
            clickedControl = source as RichTextControl;
            clickedControl.ControlColor = new Color(0, 20, 255);
            clickedControl.CursorOverColor = new Color(0, 20, 255);
        }        
    }
}

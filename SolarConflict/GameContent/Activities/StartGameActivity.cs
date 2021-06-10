using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.Scenes.Activitys;
using SolarConflict.Session;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// This activity starts a game if no save exsists, if a save exists it lets you select the save or create a new game
    /// </summary>
    class StartGameActivity: SceneActivity
    {
        private GuiManager _gui;
        private bool _startNewGame = false;
        private bool _showNewGame;
        
        public StartGameActivity(bool showNewGame = true):base(true)
        {
            _showNewGame = showNewGame;
        }

        protected override void Init(ActivityParameters parameters)
        {
            _cover = Sprite.Get("coverA1");
            _gui = new GuiManager();
            var layout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            layout.IsUpdatingPosition = true;
            _gui.Root = layout;
            var cellSize = new Vector2(600, 100);
            ScrollableGrid savesListControl = new ScrollableGrid(1, 4, cellSize);            
            _gui.Root.AddChild(savesListControl);
            var metadataList = PersistenceManager.Inst.GetAllSavesMetadata();   
            if(_showNewGame)         
                savesListControl.AddChild(MakeNewGameControl());
            foreach (var metadata in metadataList)
            {
                var control = MakeGameMetaDataControl(metadata, _gui, cellSize);
                //control.CursorOn += _gui.ToolTipHandler;
                savesListControl.AddChild(control);                
            }
            _startNewGame = metadataList.Count == 0;
        }


        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            //if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            //    ActivityManager.Inst.Back();
            _gui.Update(inputState);

            if (_startNewGame)
            {
                PersistenceManager.Inst.NewSession(null);
                //ActivityManager.SwitchActivity("Prolog",null, false);
                GameSession.Inst.Continue();
            }            
        }

        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);
            _gui.Draw();
            base.Draw(sb);
        }                

        private GuiControl MakeNewGameControl()
        {
            RichTextControl newGame = new RichTextControl(Sprite.Get("smallcross").ToTag() + " New Game");
            newGame.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                PersistenceManager.Inst.NewSession(null);
                GameSession.Inst.Continue();
            };
            newGame.IsShowFrame = true;
            return newGame;
        }

        private GuiControl MakeGameMetaDataControl(SessionMetadata metadata, GuiManager gui, Vector2 size) {            
            //HorizontalLayout layout = new HorizontalLayout(Vector2.Zero);
            var layout = new FlexibleLayout();
            layout.CursorOverColor = new Color(0, 255, 0);
            layout.UserData = metadata.Path;
            layout.Action += delegate (GuiControl source, CursorInfo cursorLocation) {
                try
                {
                    PersistenceManager.Inst.LoadSession(source.UserData);
                    GameSession.Inst.Continue();
                }
                catch (Exception)
                {
                    ActivityManager.Inst.AddToast("Load unsuccessful",200);
                }
                
            };
            layout.ShowFrame = true;

            var image = new ImageControl(Sprite.Get(metadata.SpriteID), Vector2.Zero, Vector2.One * 80);
            image.IsConsumingInput = false;

            layout.AddChain(Alignment.LeftToRight, 0f, 10f, image);

            var text = new RichTextControl(metadata.SaveTime.ToString());
            text.IsConsumingInput = false;
            text.TooltipText = "Time Played: " + metadata.StarDate.GetTimeSpan().ToString(@"hh\:mm") + "\n";
            text.CursorOn += gui.ToolTipHandler;
            layout.AddChain(Alignment.LeftToRight, new Vector2(0.5f, 0.5f), text); // alignment doesn't matter; chain only has one element and shouldn't use padding

            // Delete save button
            GuiControl deleteControl = new ConfirmationControl();

            deleteControl.Sprite = Sprite.Get("trash1");
            deleteControl.HalfSize = Vector2.One * 25f;
            deleteControl.UserData = metadata.Path;
            deleteControl.UserData = metadata.Path;
            deleteControl.Action += delegate (GuiControl source, CursorInfo cursorLocation) {                
                PersistenceManager.Inst.DeleteSession(source.UserData);
                ActivityManager.Inst.AddToast("Save was deleted!", 60);
                source.Parent.Parent.RemoveChild(source.Parent);
            };

            layout.AddChain(Alignment.RightToLeft, new Vector2(1f, 0.5f), 0f, 10f, deleteControl); // alignment only matters for padding

            layout.HalfSize = size * 0.5f;
            layout.Refresh();

            return layout;
        }    

        public GuiControl MakeDeleteControl(GuiControl layout)
        {
            HorizontalLayout control = new HorizontalLayout(Vector2.Zero);
            control.Position = layout.Position;
            control.ShowFrame = true;
            RichTextControl yesControl = new RichTextControl("Yes");
            yesControl.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                PersistenceManager.Inst.DeleteSession(source.UserData);
                source.Parent.Parent.RemoveChild(source.Parent);
            };
            control.AddChild(yesControl);
            RichTextControl noControl = new RichTextControl("No");
            noControl.Data = layout;
            yesControl.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                source.Parent.Parent = source.Data as GuiControl;
            };
            control.AddChild(noControl);
            return control;
        }

        public static Activity ActivityProvider(string parameters)
        {
            if (parameters == "SkipProlog")
            {
                PersistenceManager.Inst.NewSession(null); //Change;
                GameSession.Inst.Continue();                
                ActivityManager.Inst.AddToast("Starting a new game", 20);
                return null;
            }

            if (parameters == "NewGame")
            {
                PersistenceManager.Inst.NewSession(null); //Change;
                //GameSession.Inst.Continue();
                ActivityParameters activityParameters = new ActivityParameters();
                activityParameters.ParamDictionary.Add("target_activity", "Prolog");
                activityParameters.ParamDictionary.Add("loadouts","RoundProlog1,PrologShip1");
                ActivityManager.Inst.SwitchActivity("StartingShipSelectActivity", activityParameters, false);
                ActivityManager.Inst.AddToast("Starting a new game", 20);
                return null;
            }


            if (parameters == "NewGameB")
            {
                PersistenceManager.Inst.NewSession(null); //Change;
                //GameSession.Inst.Continue();
                ActivityParameters activityParameters = new ActivityParameters();
                activityParameters.ParamDictionary.Add("target_activity", "PrologWave");
                activityParameters.ParamDictionary.Add("loadouts", "PrologShip1,PrologShip2");
                ActivityManager.Inst.SwitchActivity("StartingShipSelectActivity", activityParameters, false);
                ActivityManager.Inst.AddToast("Starting a new game", 20);
                return null;
            }

            if (parameters == "Continue")
            {
                try
                {
                    var metadataList = PersistenceManager.Inst.GetAllSavesMetadata();
                    if (metadataList.Count > 0)
                    {
                        PersistenceManager.Inst.LoadSession(metadataList[0].Path);
                        GameSession.Inst.Continue();
                        return null;
                    }
                    ActivityManager.Inst.AddToast(Color.Red.ToTag("No games to continue!"), 90);
                }
                catch (Exception)
                {
                    ActivityManager.Inst.AddToast("Unable to load game", 100);
                }
             
                return null;               
            }
            if(parameters == "Load")
            {
                var metadataList = PersistenceManager.Inst.GetAllSavesMetadata();
                if(metadataList.Count > 0)
                    return new StartGameActivity(false);
                else
                {
                    ActivityManager.Inst.AddToast(Color.Red.ToTag("No games to load!"), 90);
                    return null;
                }
            }
            return new StartGameActivity();
        }

    }
}

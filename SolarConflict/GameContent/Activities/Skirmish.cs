using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.Activitys;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.Files;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.XnaUtils.SimpleGui.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Skirmish - Select ships and battle
    /// </summary>
    public class Skirmish : SceneActivity
    {
        private const int SIZE = 150;
        private GuiManager gui;
        private ScrollableGrid selectionGrid;
        private GridControl leftTeamGrid;
        private GridControl rightTeamGrid;        
        private LoadoutControl cursorControl;

        public Skirmish():base(false)
        {            
        }

        protected override void Init(ActivityParameters parameters)
        {
            _cover = Sprite.Get("cover2");
            
            string shipList = parameters;
            if (!string.IsNullOrWhiteSpace(parameters) && parameters == "#user")
            {
                string path = Consts.GetLoadoutPath(Consts.USER_LOADOUTS);
                var files = FileUtils.GetFiles(path, "*.json", SearchOption.AllDirectories);

                StringBuilder sb = new StringBuilder();
                foreach (var item in files)
                {
                    sb.Append(Path.GetFileNameWithoutExtension(item)+",");
                }
                shipList = sb.ToString();
            }

            
            cursorControl = new LoadoutControl(null, Vector2.Zero, Vector2.One * 80);
            cursorControl.ControlColor = Color.Transparent;
            gui = new GuiManager();
                                                

            leftTeamGrid = new GridControl(3, 2, Vector2.One * 80);
            rightTeamGrid = new GridControl(3, 2, Vector2.One * 80);

            List<AgentLoadout> loadouts;
            if (string.IsNullOrWhiteSpace(shipList))
                loadouts = ContentBank.Inst.GetAllLoadout();
            else
            {
                loadouts = new List<AgentLoadout>();
                var shipIDs = shipList.Split(',');
                foreach (var shipID in shipIDs)
                {
                    try
                    {
                        AgentLoadout loadout = ContentBank.Inst.GetLoadout(shipID);
                        loadouts.Add(loadout);
                    }
                    catch (Exception)
                    {

                        //Write to log
                    }
                   
                }
            }
            int ynum = Math.Min((int)Math.Ceiling(loadouts.Count / 10f),4);
            selectionGrid = new ScrollableGrid(10, ynum, Vector2.One * SIZE);
            loadouts = loadouts.OrderByDescending(l => l.Agent.SizeType).ThenBy(l => l.Name).ToList();
                                   
            selectionGrid.RemoveAllChildren();
            leftTeamGrid.RemoveAllChildren();
            rightTeamGrid.RemoveAllChildren();

            for (int i = 0; i < loadouts.Count; i++)
            {
                var control = new LoadoutControl(loadouts[i], Vector2.One * 150, Vector2.One * 100);
                control.CursorOn += gui.ToolTipHandler;
                selectionGrid.AddChild(control);
                control.Action += ShipSelectHandler;
            }

            for (int i = 0; i < leftTeamGrid.Count; i++)
            {
                var control = new LoadoutControl(null, Vector2.One * 150, Vector2.One * 100);
                control.CursorOn += gui.ToolTipHandler;
                leftTeamGrid.AddChild(control);
                control.CursorOn += AddShipHandler;
            }

            for (int i = 0; i < rightTeamGrid.Count; i++)
            {
                var control = new LoadoutControl(null, Vector2.One * 150, Vector2.One * 100);
                control.CursorOn += gui.ToolTipHandler;
                rightTeamGrid.AddChild(control);
                control.CursorOn += AddShipHandler;
            }
            FMath.Shuffle(loadouts, FMath.Rand);
            int count = Math.Min(FMath.Rand.Next(1, loadouts.Count), 3);// leftTeamGrid.GetChildren().Length - 1);
            for (int i = 0; i < count; i++)
            {
                (leftTeamGrid.GetChildren()[i] as LoadoutControl).Loadout = loadouts[i];
                (rightTeamGrid.GetChildren()[i] as LoadoutControl).Loadout = loadouts[i];
            }

            //loadouts

            GuiControl holderControl = new GuiControl(Vector2.Zero, leftTeamGrid.HalfSize * 2);
            holderControl.Sprite = null;

            VerticalLayout leftLayout = new VerticalLayout(Vector2.Zero, 10);
            //RichTextControl leftTitle = new RichTextControl("Player Ships", isShowFrame:true);
            //leftLayout.AddChild(leftTitle);
            //leftLayout.AddChild(leftTeamGrid);
            //leftLayout.Position = new Vector2(-leftLayout.HalfSize.X - 5, 0);
            leftTeamGrid.LocalPosition = new Vector2(-leftTeamGrid.HalfSize.X - 5, 0);
            leftTeamGrid.TooltipText = "Player Ships";
            leftTeamGrid.CursorOn += gui.ToolTipHandler;

            rightTeamGrid.LocalPosition = new Vector2(rightTeamGrid.HalfSize.X + 5, 0);
            rightTeamGrid.TooltipText = "Enemy Ships";
            rightTeamGrid.CursorOn += gui.ToolTipHandler;
            holderControl.AddChild(leftTeamGrid);
            holderControl.AddChild(rightTeamGrid);

            gui.Root = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 10));
            gui.Root.AddChild(selectionGrid);
            gui.Root.AddChild(holderControl);

            TextControl startGame = new TextControl(UIElmentsTexts.StartGame, Game1.font);
            startGame.IsShowFrame = true;
            startGame.Action += GenerateLevel;
            gui.Root.AddChild(startGame);            
            string text = @"The left box contains your ships. The right box contains the enemy ships.
Place ships in the empty slots and prepare for battle.
You can control only one ship from your fleet at a time,
and you can change the ship you control by clicking #action{SwapUp}.
Uncontrolled ships will be controlled by the AI.
Good luck!";
            AddHelp(text);
            gui.Update(InputState.EmptyState);          
        }

        #region Update/Draw        

        public override void Update(InputState inputState) {
            base.Update(inputState);
            gui.Update(inputState);

            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();

            if(inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var control in leftTeamGrid.GetChildren())
                {
                    var loadoutControl = control as LoadoutControl;
                    if(loadoutControl.Loadout != null)
                        sb.Append(loadoutControl.Loadout.ID + ",");
                }
                string text = sb.ToString();
                File.WriteAllText("leftLoadouts.txt", text);
                ActivityManager.Inst.AddToast("Saved loadouts: " + text, 200);
            }

            cursorControl.Position = inputState.Cursor.Position;
            cursorControl.Update(InputState.EmptyState);
        }

        public override void Draw(SpriteBatch sb) {
            base.DrawBackground(sb);
            gui.Draw();
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            cursorControl.Draw(sb);
            sb.End();
            base.Draw(sb);
        }

        #endregion

        public static Activity ActivityProvider(string param) {
            return new Skirmish();
        }

        private void ShipSelectHandler(GuiControl source, CursorInfo cursorLocation) {
            LoadoutControl loadoutControl = (LoadoutControl)source;
            //if loadout != null play sound
            cursorControl.Loadout = loadoutControl.Loadout;
        }

        private void AddShipHandler(GuiControl source, CursorInfo cursorLocation) {
            if (cursorLocation.OnPressLeft || cursorLocation.OnReleaseLeft) {
                if (cursorControl.Loadout != null) {
                    LoadoutControl loadoutControl = (LoadoutControl)source;
                    loadoutControl.Loadout = cursorControl.Loadout;
                    cursorControl.Loadout = null;
                    //Play Sound
                }
            }

            if (cursorLocation.OnPressRight) {
                LoadoutControl loadoutControl = (LoadoutControl)source;
                loadoutControl.Loadout = null;
            }
        }

        private void GenerateLevel(GuiControl source, CursorInfo cursorLocation) {
            var level = new SkirmishBattle();
            level.HudManager.AddComponent(new AnnouncerEffects());
            // Add ships
            leftTeamGrid.GetChildren().Select(c => (c as LoadoutControl).Loadout).Where(l => l != null).Do(l => level.AddLoadout(l, FactionType.Player,true));
            rightTeamGrid.GetChildren().Select(c => (c as LoadoutControl).Loadout).Where(l => l != null).Do(l => level.AddLoadout(l, FactionType.Pirates1, false));
           
            // Add rocks
            level.AddObjectRandomlyInCircle("SmallAsteroid1", 80, 10000);
            level.AddObjectRandomlyInCircle("Asteroid1", 50, 10000);

            level.Update(InputState.EmptyState);
            ActivityManager.Inst.SwitchActivity(level);
        }
        
    }
}

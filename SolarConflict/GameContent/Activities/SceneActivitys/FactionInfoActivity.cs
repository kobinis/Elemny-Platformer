using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using SolarConflict.Session.World.MissionManagment;
using XnaUtils.SimpleGui.Controllers;
using SolarConflict.Framework.Scenes.Activitys;
using Microsoft.Xna.Framework;
using XnaUtils.Input;
using XnaUtils.Graphics;
using SolarConflict.XnaUtils.SimpleGui;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Activities.SceneActivitys {
    public class FactionInfoActivity : SceneActivity {
        Sprite _otherFactionHostile;
        Sprite _otherFactionNotHostile;
        Sprite _playerHostile;
        Sprite _playerNotHostile;
        protected GuiManager _gui;

        protected override void Init(ActivityParameters parameters) {
            base.Init(parameters);
            _gui = new GuiManager();
            _guiLayout = new VerticalLayout(ActivityManager.ScreenSize * 0.5f);
            _gui.AddControl(_guiLayout);
            _guiLayout.ShowFrame = true;
            _playerHostile = Sprite.Get("Failed");
            _playerNotHostile = Sprite.Get("button");
            _otherFactionHostile = Sprite.Get("fireball1");
            _otherFactionNotHostile = Sprite.Get("car1");
            InitGui();
        }

        private void InitGui() {
            var relevantFactions = new FactionType[] {FactionType.Player, FactionType.Alliance, FactionType.Empire, FactionType.Federation, FactionType.Hegemony, FactionType.MinerGuild,
                FactionType.Regime, FactionType.Republic, FactionType.Pirates1, FactionType.Pirates2, FactionType.Pirates3, FactionType.TradingGuild, FactionType.Vile, FactionType.Void }.ToSet();
            // ^ disregard stuff like neutrals

            relevantFactions.IntersectWith(_scene.GameEngine.GetSurvivingFactions());
            // ^ also disregard stuff that's not in the current node

            var title = new RichTextControl("FACTIONS:", Game1.menuFont);
            _guiLayout.AddChild(title);
            var grid = new ScrollableGrid(1, 8, new Vector2(ActivityManager.ScreenSize.X * 0.8f, 100));
            _guiLayout.AddChild(grid);
           
            relevantFactions.Do(f => {
                var control = MakeFactionControl(_scene.GameEngine.GetFaction(f));
                control.CursorOn += _gui.ToolTipHandler;
                control.Width = grid.Width;
                grid.AddChild(control);
            });            
        }

        public GuiControl MakeFactionControl(Faction faction)
        {
            var layout = new RelativeLayout();
            layout.ShowFrame = true;
            // Icon and name                    
            var icon = new ImageControl(faction.LogoSprite, Vector2.Zero, new Vector2(90f, 60f));
            var name = new RichTextControl(faction.Name);
            name.TextColor = faction.Color;
            layout.AddChild(icon, HorizontalAlignment.Left, VerticalAlignment.Center);
            layout.AddChild(name, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);
         
            icon.CursorOn += _gui.ToolTipHandler;
            icon.TooltipText = "Money: " + faction.GetMeter(MeterType.Money).Value + " Kills: " + faction.GetMeter(MeterType.FactionKills).Value;
        
           // layout.TooltipText = 

            var reputation = new RichTextControl(faction.GetPlayerRelationsString());
            if(faction.FactionType != FactionType.Player)
                layout.AddChild(reputation, HorizontalAlignment.Right, VerticalAlignment.Center);


            //// Button for setting relationship with faction
            //isHostile = playerFaction.GetRelationToFaction(faction.FactionType) < 0f;
            //var hostilityToggle = new ImageControl(_playerNotHostile, Vector2.Zero, Vector2.One * 25);
            //hostilityToggle.PressedSprite = _playerHostile;
            //hostilityToggle.IsPressed = isHostile;
            //hostilityToggle.CursorOn += _gui.ToolTipHandler;
            //hostilityToggle.IsToggleable = true;
            ////hostilityToggle.LogicFunction = (controlArg) => {
            ////    MetaWorld.Inst.GetFaction(FactionType.Player).SetRelationToFaction(_scene.GameEngine, faction.FactionType, controlArg.IsPressed ? -1f : 1f);
            ////    controlArg.TooltipText = hostilityToggle.TooltipText = controlArg.IsPressed ? "We are hostile to them (Click to toggle)" : "We are not hostile to them (Click to toggle)";
            ////    // Note that this won't work if the playerFaction.ReflectRelations
            ////};

            //result.AddChain(Alignment.TopToBottom, new Vector2(1f, 0.5f), new Vector2(0.975f, 0.5f), 0f, 10f, relationshipIndicator, hostilityToggle);


            return layout;
        }


        public override void Update(InputState inputState) {
            base.Update(inputState);
            _gui.Update(inputState);
            _guiLayout.Position = new Vector2(_guiLayout.HalfSize.X + 80, _guiLayout.HalfSize.Y + 10);            
        }

        public override void Draw(SpriteBatch sb) {
            _scene.background.Draw(_scene.Camera);
            base.Draw(sb);
            _gui.Draw();
        }

        public static Activity ActivityProvider(string parameters) {
            return new FactionInfoActivity();
        }
    }
}
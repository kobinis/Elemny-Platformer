using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework;
using SolarConflict.XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework.Input;
using XnaUtils.Input;
using XnaUtils.Graphics;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework;
using XnaUtils.Framework.Graphics;
using System.Runtime.Serialization;
using SolarConflict.Framework.Scenes.Activitys;

namespace SolarConflict.GameContent.Activities
{
    //[Serializable]
    public class ChallengeSelectionActivity : SceneActivity 
    {
        private static Vector2 controlSize = new Vector2(1000, 200);
        private static Color completeColor = Color.DarkGray;
        [NonSerialized]
        private GuiManager gui;
        [NonSerialized]
        private GuiControl selectedControl;
        private ChallengeInfo selectedChallenge;        
        private GameEngine gameEngine;
        private List<ChallengeInfo> challenges;

        public ChallengeSelectionActivity()
        {
                                       
        }

        protected override void Init(ActivityParameters parameters) //TODO: change
        {
            base.Init(parameters);
            gameEngine = parameters.GetObjectParam("game_engine") as GameEngine;
            int level = ParserUtils.ParseInt(parameters.GetParam("level"), 0);
            // int factionIndex = Parser.ParseInt(parameters.GetParam("faction_index"), 1);
            //TODO: get from component
            this.challenges = null; // MetaWorld.Inst.GetFaction(_calling_agent.FactionType).GetChallenges(level);


            gui = new GuiManager();           
            gui.Root = MakeGui();            
        }

        private GuiControl MakeGui()
        {
            GuiControl layout = new ControlsGroup();
            layout.Position = new Vector2(ActivityManager.ScreenSize.X / 2, 200);
            RichTextControl title = new RichTextControl("Choose your challenge", Game1.menuFont);
            title.AlignnUp();
            layout.AddChild(title);
            ScrollableGrid grid = new ScrollableGrid(1, 3, controlSize);
            layout.AddChild(grid);
            grid.AlignnUp(20, title);
            foreach (var item in challenges)
            {
                GuiControl control = MakeChallengeControl(item, gui);
                grid.AddChild(control);
            }

            return layout;        
        }

        private GuiControl MakeChallengeControl(ChallengeInfo challenge, GuiManager guiManager)
        {
            RelativeLayout layout = new RelativeLayout();
            //layout.Sprite = Sprite.Get("guif8");
            //layout.ControlColor = Palette.GuiFrame;
            layout.HalfSize = controlSize * 0.5f;
            layout.ShowFrame = true;
            if (challenge.IsComplete)
            {
                layout.ControlColor = completeColor;
            }

            ImageControl help = new ImageControl(Sprite.Get("helpicon2"),Vector2.Zero, Vector2.One * controlSize.Y * 0.8f);
            help.ControlColor = Color.AliceBlue;
            help.IsConsumingInput = false;
            help.TooltipText = challenge.Description;
            help.CursorOn += guiManager.ToolTipHandler;
            layout.AddChild(help, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);

            ImageControl image = new ImageControl(challenge.Icon, Vector2.Zero, Vector2.One * controlSize.Y * 0.8f);
            image.IsConsumingInput = false;
            image.TooltipText = challenge.IconText;
            image.CursorOn += guiManager.ToolTipHandler;
            layout.AddChild(image, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);
            RichTextControl textControl = new RichTextControl(challenge.Name);
            textControl.IsConsumingInput = false;
            layout.AddChild(textControl, HorizontalAlignment.Center, VerticalAlignment.Center, null);
            layout.Data = challenge;
            layout.Action += ActivateChallenge;
            layout.CursorOverColor = Color.Yellow;
            GuiControl lastAdded = null;            
            var reward = challenge.Reward;
            if (reward != null)
            {
                ImageControl rewardControl = new ImageControl(Sprite.Get("reward"), Vector2.Zero, Vector2.One * 50);
                //itemControl.CursorOn += gui.ToolTipHandler;
                rewardControl.IsConsumingInput = false;
                rewardControl.CursorOn += gui.ToolTipHandler;
                rewardControl.TooltipText = reward.GetTag();
                lastAdded = rewardControl;
                layout.AddChild(rewardControl, HorizontalAlignment.Right, VerticalAlignment.Center, lastAdded);
            }
            //if (challenge.MoneyReward > 0) //Change to 
            //{
            //    ImageControl imageControl = new ImageControl(Sprite.Get("spacebucks"), Vector2.Zero, Vector2.One * 70);
            //    imageControl.IsConsumingInput = false;
            //    TextControl sum = new TextControl(challenge.MoneyReward.ToString());
            //    sum.LocalPosition = Vector2.UnitY * 20;
            //    sum.IsConsumingInput = false;
            //    imageControl.AddChild(sum);
            //    layout.AddChild(imageControl, HorizontalAlignment.Right, VerticalAlignment.Center, lastAdded);
            //    lastAdded = imageControl;
            //}

            return layout;
        }

        private void ActivateChallenge(GuiControl source, CursorInfo cursorLocation)
        {
            selectedControl = source;
            selectedChallenge = source.Data as ChallengeInfo;
            ActivityManager.Inst.SwitchActivity(selectedChallenge.ActivityID, new ActivityParameters(selectedChallenge.ActivityParameters));
        }

        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            gui.Update(inputState);            
        }

        public override void Draw(SpriteBatch sb)
        {
            
            sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            GraphicsUtils.DrawBackground(Sprite.Get("cover2"), sb);
            sb.End();
            gui.Draw();
            base.Draw(sb);
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            gui = new GuiManager();
            gui.Root = MakeGui();
        }

        public override void OnResume(ActivityParameters parameters = null)
        {
            selectedChallenge.OnChallengeEnd(parameters, gameEngine);
            if (selectedChallenge.IsComplete)
                selectedControl.ControlColor = Color.DarkGreen; //Take from pallete                        
        }

        public static Activity ActivityProvider(string parameters) //Or get a file name
        {
          
            return new ChallengeSelectionActivity();
        }

    }
}

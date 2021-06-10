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
using SolarConflict.Framework.GUI;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    /// <summary>
    /// Shows the availabe missions an Agent (with an agent dispatcher system) has stored
    /// </summary>
    public class AgentMissionActivity : SceneActivity
    {
        private Sprite selectSprite;
        protected GuiManager _gui;
        protected VerticalLayout _layout;        

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
            _gui = new GuiManager();

            HorizontalLayout horizontalLayout = new HorizontalLayout(ActivityManager.ScreenCenter, 5, true);
            horizontalLayout.AddChild(GuiControlFactory.MakeCharacterControl(_calling_agent.GetCharacter(_scene), Vector2.One * 400, _scene.GameEngine.GetFaction(_calling_agent.GetFactionType())));
            
            _layout = new VerticalLayout(Vector2.Zero);
          
            _layout.ShowFrame = true;
            selectSprite = Sprite.Get("selected");         
            InitGui();
            horizontalLayout.AddChild(_layout);
            _gui.AddControl(horizontalLayout);
        }

        private void InitGui()
        {
            RichTextControl title = new RichTextControl("Quests:", Game1.menuFont);
            _layout.AddChild(title);
            ScrollableGrid grid = new ScrollableGrid(1, 8, new Vector2(600, 100));
            _layout.AddChild(grid);
            List<IMissionGenerator> generatos;
            MissionBankSystem bankSystem = _calling_agent?.GetSystem<MissionBankSystem>();
            if (bankSystem != null)
            {
                generatos = bankSystem.GetMissions(_scene, _calling_agent);
            }
            else
            {
                generatos = _scene.GetMissionGenerators(_calling_agent.FactionType);
            }
            
            foreach (var missionGen in generatos)
            {
                grid.AddChild(MakeMissionControl(missionGen, _scene));
            }
        }

        public GuiControl MakeMissionControl(IMissionGenerator generator, Scene scene) //Move to factory
        {
            RelativeLayout layout = new RelativeLayout();
            layout.TooltipText = generator.GetTooltipText();
            layout.CursorOn += _gui.ToolTipHandler;
            layout.ShowFrame = true;
            //ImageControl acceptMission = new ImageControl(selectSprite, Vector2.Zero, Vector2.One * 60);
            RichTextControl acceptMission = new RichTextControl("Accept", isShowFrame: true); //new ImageControl(selectSprite, Vector2.Zero, Vector2.One * 60);
                                                                                              // acceptMission
            acceptMission.CursorOverColor = Color.Yellow;
            acceptMission.CursorOn += _gui.ToolTipHandler;
            acceptMission.TooltipText = "Accept Mission";
            acceptMission.Data = generator;
            acceptMission.Action += delegate (GuiControl source, CursorInfo cursorLocation)
            {
                var missionGenerator = source.Data as IMissionGenerator;
                _scene.AddMission(missionGenerator.GenerateMission());
                _scene.RemoveMissionGenerator(missionGenerator); //TODO: remove 
                source.Parent.Parent.RemoveChild(source.Parent);
                ActivityManager.Inst.AddToast(Color.Green.ToTag("Mission accepted"), 60);
            };
            layout.AddChild(acceptMission, HorizontalAlignment.Left, VerticalAlignment.Center, layout.LastChildAdded);


            RichTextControl text = new RichTextControl(generator.Title);
            text.IsConsumingInput = false;
            text.IsShowFrame = false;
            text.PressedControlColor = new Color(0, 0, 255);
            layout.AddChild(text, HorizontalAlignment.Center, VerticalAlignment.Center, null);

            HorizontalLayout horizontalLayout = new HorizontalLayout(Vector2.Zero);

            if (generator.Icon != null)
            {
                ImageControl image = new ImageControl(generator.Icon, Vector2.Zero, Vector2.One * 60);
                image.IsConsumingInput = false;

                horizontalLayout.AddChild(image);
                //chain.Insert(0, image);
                //result.AddChain(Alignment.LeftToRight, image);
            }

            if (false && generator.IsDismissable)
            {
                ImageControl deleteControl = new ImageControl(Sprite.Get("trash1"), Vector2.Zero, Vector2.One * 50);
                deleteControl.Data = new Tuple<Scene, IMissionGenerator>(scene, generator);
                deleteControl.Action += delegate (GuiControl source, CursorInfo cursorLocation) {
                    var tuple = source.Data as Tuple<Scene, Mission>;
                    tuple.Item1.RemoveMissionGenerator(tuple.Item2);
                    source.Parent.Parent.Parent.Parent.RemoveChild(source.Parent.Parent); //FIx
                    ActivityManager.Inst.AddToast("Mission was dismissed", 60);
                };

                horizontalLayout.AddChild(deleteControl);
            }
            horizontalLayout.SetHorizontalPositions();
            horizontalLayout.RefreshSize();
            return layout;
        }



        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            _gui.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            _scene.background.Draw(_scene.Camera);
            base.Draw(sb);
            _gui.Draw();
        }



        public static Activity ActivityProvider(string parameters)
        {
            return new AgentMissionActivity();
        }
    }
}

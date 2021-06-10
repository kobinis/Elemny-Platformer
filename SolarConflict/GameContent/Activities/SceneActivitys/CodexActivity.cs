using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.Scenes.Activitys;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.XnaUtils.SimpleGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.GameContent.Activities.SceneActivitys
{
    [Serializable]
    public class CodexManager
    {
        public List<string> _codexIDList;
        public HashSet<string> _codexIDSet;

        public List<Mission> _missions; //Past missions

        public CodexManager()
        {
            _codexIDList = new List<string>();
            _codexIDSet = new HashSet<string>();
            _missions = new List<Mission>();
        }
        


        public void AddMission(Mission mission)
        {
            
            if (mission.ID != null && !mission.IsDismissable && !mission.IsHidden && !_codexIDSet.Contains(mission.ID))
            {
                _codexIDSet.Add(mission.ID);
                _missions.Add(mission);
            }
        }

        public List<Mission> GetPastMissionList()
        {
            return _missions;
        }

        public List<CodexEntry> GetCodexList()
        {
            List<CodexEntry> entryList = new List<CodexEntry>();
            foreach (var id in _codexIDList)
            {
                entryList.Add(ContentBank.Inst.GetEmitter(id) as CodexEntry);
            }
            return entryList;
        }

        public bool AddCodex(string id)
        {
            if(!_codexIDSet.Contains(id))
            {
                if(ContentBank.Inst.GetEmitter(id) is CodexEntry)
                {
                    _codexIDSet.Add(id);
                    _codexIDList.Add(id);
                }
                else
                {
                    ActivityManager.Inst.AddToast($"{id} is not a CodexEntry", 100, Color.Red);
                }
            }
            return false;
        }
    }

    [Serializable]
    public class CodexEntry:IEmitter
    {
        public string ID { get; set; }
        public String Name;
        public string Body;
        public string BodyImage;
        //public CodexCategory Category;
        public string SoundID;


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            MetaWorld.Inst.CodexManager.AddCodex(ID);
            return null;
        }
    }
       
    class CodexActivity : SceneActivity
    {
        private GuiManager gui;

        protected override void Init(ActivityParameters parameters)
        {
            //base.Init(parameters);
            gui = new GuiManager();
            gui.Root = MakeGUI();
        }


        public override void Update(InputState inputState)
        {
            base.Update(inputState);
            gui.Update(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            gui.Draw();
            base.Draw(spriteBatch);
        }

        public static Activity ActivityProvider(string parameters)
        {
            return new CodexActivity();
        }

        public static GuiControl MakeGUI()
        {
            
            VerticalLayout layout = new VerticalLayout(Vector2.Zero, 2, true);
            layout.Position = ActivityManager.ScreenCenter;
            int categoryNum = 5;
            float controlWitdh = (ActivityManager.ScreenSize.X-40) / categoryNum;
            string[] names = { "Plot", "Factions", "Mechanics", "Screens", "All" };
            GridControl grid = new GridControl(categoryNum, 1, new Vector2(controlWitdh-1, 40));
            foreach (var item in names)
            {
                grid.AddChild(new RichTextControl(item, isShowFrame:true));
            }

            RichTextControl codexBody = new RichTextControl("", isShowFrame:true);

            
            ScrollableGrid missions = new ScrollableGrid(1, 20, new Vector2(120, 30));
            codexBody.Width = grid.Width - missions.Width - 40;
            codexBody.Height = missions.Height;

            HorizontalLayout horizontal = new HorizontalLayout(Vector2.Zero);
            horizontal.AddChilds(missions, codexBody);

            layout.AddChilds(grid, horizontal);
            return layout;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SolarConflict.AI.GameAI;
using SolarConflict.Framework;
using SolarConflict.GameContent.ContentGeneration;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils.SimpleGui;
using System.IO;

namespace SolarConflict.GameContent.Activities
{
    [Serializable]
    public class LevelEditorActivity : Scene //change to normal scene with script
    {
        private GuiManager _gui;
        private IEmitter _selectedEmitter;
        private float _rotation;
        private FactionType _selectedFaction;
        //private int _selectedCategoryIndex;
        private GuiControlSelector _gridSelector;
        private Dictionary<FactionType, GameObject> _factionParent;
        private GuiControl _loadSaveGuiControl;
        
        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
            _gui = new GuiManager();            
            _gui.Root = MakeGui(_gui);
            _factionParent = new Dictionary<FactionType, GameObject>();
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory);
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.TacticalMap);
            CameraManager.MovmentType = CameraMovmentType.Manual;
            IsShipSwitchable = true;
            
        }

        public override void UpdateScript(InputState inputState)
        {           
            _gui.Update(inputState);
            if((inputState.Cursor.Position - inputState.Cursor.PreviousPosition).LengthSquared() > 1)
            {
                _rotation = FMath.GetRotation(inputState.Cursor.Position - inputState.Cursor.PreviousPosition);
            }

            if (inputState.Cursor.OnPressLeft && _selectedEmitter != null )
            {
                if (inputState.Cursor.IsActive)
                {
                   
                    FactionType factionType = _selectedFaction;
                    if(!_factionParent.ContainsKey(factionType))
                    {
                        _factionParent.Add(factionType, new DummyObject());
                    }
                    GameObject parent = _factionParent[factionType];
                    Vector2 position = Camera.GetWorldPos(inputState.Cursor.Position);
                    _selectedEmitter.Emit(GameEngine, parent, factionType, position, Vector2.Zero, _rotation);
                }
            }

            if(inputState.IsKeyPressed(Keys.Delete))
            {
                if(GameObjectUnderCursor != null && GameObjectUnderCursor.IsActive)
                {
                    GameEngine.RemoveGameObject(GameObjectUnderCursor);
                }
            }

            if(IsGamePaused)
            {
                CameraManager.Update(Camera, this.GameEngine, inputState);
                foreach (GameObject obj in GameEngine.AddList)
                {
                    GameEngine.AddGameObject(obj);
                }
                GameEngine.AddList.Clear();
            }
        }

        public override void DrawScript(SpriteBatch sb)
        {
            _gui.Draw();
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new LevelEditorActivity();
        }

        private GuiControl MakeGui(GuiManager gui)
        {   
            RelativeLayout layout = new RelativeLayout();
            layout.IsConsumingInput = false;
            layout.DefaultAligmentData.Anchor = new GuiControl(ActivityManager.ScreenCenter, ActivityManager.ScreenSize);
           // layout.IsAutoUpadeSize = true;
            GuiControlSelector gridSelector = new GuiControlSelector();
            string[] categories = { "Ships", "Items", "Projectiles" ,"Effects" , "Hulls", "All"  };
            Type[] types = { typeof(AgentLoadout), typeof(Item), typeof(ProjectileProfile) , typeof(ParamEmitter), typeof(Agent), null};
            foreach (var type in types)
            {
                gridSelector.AddChild(MakeEmitterGrid(ContentBank.Inst.GetAllEmittersOfType(type), gui));
            }
           // gridSelector.AddChild(MakeEmitterGrid(ContentBank.Inst.GetAllEmittersOfType
            layout.AddChild(gridSelector, HorizontalAlignment.Left, VerticalAlignment.Center);
            var factionControl = MakeFactionControl();
            layout.AddChild(factionControl, HorizontalAlignment.Right, VerticalAlignment.Down);
            layout.AddChild(MakeCategoryControl(categories), HorizontalAlignment.Center, VerticalAlignment.Up);
            layout.AddChild(MakeMiscControls(gui, this), HorizontalAlignment.Right, VerticalAlignment.Center);
            _gridSelector = gridSelector; //Move?
            return layout;
        }

        private GuiControl MakeMiscControls(GuiManager gui, Scene scene)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero);
            GuiControl togglePause = new GuiControl(Vector2.Zero, Vector2.One * 40);
            togglePause.TooltipText = "Pause\\Play";
            togglePause.CursorOn += gui.ToolTipHandler;
            togglePause.IsToggleable = true;
            togglePause.ControlColor = Color.Green;
            togglePause.PressedControlColor = Color.Red;
            togglePause.Sprite = Sprite.Get("play");
            togglePause.Data = scene;
            togglePause.Action += (source, cursorLocation) => { (source.Data as Scene).IsGamePaused = source.IsPressed; };
            layout.AddChild(togglePause);
            layout.AddChild(MakeLoadSaveControl(gui));
            layout.AddChild(MakeCameraControl(gui));
            return layout;
        }

        private GuiControl MakeLoadSaveControl(GuiManager gui)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero,showFrame:true);
            layout.IsResizeChildrenHorizontally = true;
            RichTextControl loadControl = new RichTextControl("Load", isShowFrame:true);
            loadControl.Action += (source, cursorLocation) => { Load(); };
            layout.AddChild(loadControl);

            RichTextControl saveControl = new RichTextControl("Save",isShowFrame:true);
            saveControl.Action += (source, cursorLocation) => { Save(); };
            layout.AddChild(saveControl);
            
            
            
            
            //layout.AddChild()
            return layout;
        }

        
        public void Save()
        {
          //  Activity backActivity = this.CallingActivity;
          //  this.CallingActivity = null;

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var file = new FileStream(@"\LevelEditor.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(file, this);
            file.Close();

           // this.CallingActivity = backActivity;
        }

        public void Load()
        {
            try
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var file = new FileStream(@"\LevelEditor.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                var scene = formatter.Deserialize(file) as LevelEditorActivity;
               // scene.CallingActivity = this.CallingActivity;
                ActivityManager.Inst.SwitchActivity(scene, false);
                file.Close();
            }
            catch (Exception e)
            {

                ActivityManager.Inst.AddToast(e.ToString(), 200);
            }
                     
        }

        //private GuiControl MakeSaveControl
        

        private GuiControl MakeCameraControl(GuiManager gui)
        {
            VerticalLayout layout = new VerticalLayout(Vector2.Zero, showFrame:true);
            RadioSelectionGroup radioGroup = new RadioSelectionGroup();
            int counter = 0;
            CameraMovmentType[] categories = { CameraMovmentType.Manual, CameraMovmentType.OnPlayer };
            foreach (var item in categories)
            {
                TextControl emitterControl = new TextControl(item.ToString());
                emitterControl.IsToggleable = true;
                emitterControl.IsShowFrame = true;
                emitterControl.HalfSize = new Vector2(100, 20);
                emitterControl.Data = categories[counter];
                emitterControl.Action += (GuiControl source, CursorInfo cursorLocation) => { CameraManager.MovmentType = (CameraMovmentType)source.Data; };
                radioGroup.AddControl(emitterControl);
                layout.AddChild(emitterControl);
                counter++;
            }
            return layout;
        }

        private GuiControl MakeEmitterGrid(List<IEmitter> emitters, GuiManager gui)
        {            
            ScrollableGrid grid = new ScrollableGrid(1, 20, new Vector2(300, 40));
            grid.IsConsumingInput = true;
            emitters.OrderBy(e => e.ID).ToList();
            RadioSelectionGroup radioGroup = new RadioSelectionGroup();
            foreach (var emitter in emitters)
            {
                TextControl emitterControl = new TextControl(emitter.ID);
                emitterControl.IsToggleable = true;
                emitterControl.IsShowFrame = true;
                emitterControl.HalfSize = new Vector2(100, 20);
                emitterControl.Data = emitter;
                emitterControl.Action += (GuiControl source, CursorInfo cursorLocation) => { _selectedEmitter = (IEmitter)source.Data; };
                emitterControl.CursorOn += gui.ToolTipHandler;
                emitterControl.TooltipText = GetEmitterTooltip(emitter);
                radioGroup.AddControl(emitterControl);
                grid.AddChild(emitterControl);
            }
            return grid;
        }

        public string GetEmitterTooltip(IEmitter emitter)
        {
            if(emitter is AgentLoadout)
            {
                return (emitter as AgentLoadout).FullDescription;
            }
            if (emitter is Item)
            {
                return (emitter as Item).Profile.DescriptionText;
            }
            return string.Empty;
        }
        
        private GuiControl MakeCategoryControl(string[] categories)
        {
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero, showFrame: true);
            RadioSelectionGroup radioGroup = new RadioSelectionGroup();
            int counter = 0;
            foreach (var item in categories)
            {
                TextControl emitterControl = new TextControl(item.ToString());
                emitterControl.IsToggleable = true;
                emitterControl.IsShowFrame = true;
                emitterControl.HalfSize = new Vector2(100, 20);
                emitterControl.Data = counter;
                emitterControl.Action += (GuiControl source, CursorInfo cursorLocation) => { _gridSelector.SelectedControl = (int)source.Data; };
                radioGroup.AddControl(emitterControl);
                layout.AddChild(emitterControl);
                counter++;
            }
            return layout;
        }

        private GuiControl MakeFactionControl()
        {
            HorizontalLayout layout = new HorizontalLayout(Vector2.Zero, showFrame:true);
            FactionType[] factions = { FactionType.Neutral, FactionType.Player, FactionType.Federation, FactionType.Empire, FactionType.TradingGuild, FactionType.Pirates1 };
            RadioSelectionGroup radioGroup = new RadioSelectionGroup();
            foreach (var item in factions)
            {
                TextControl emitterControl = new TextControl(item.ToString());
                emitterControl.IsToggleable = true;
                emitterControl.IsShowFrame = true;
                emitterControl.HalfSize = new Vector2(100, 20);
                emitterControl.Data = item;
                emitterControl.Action += (GuiControl source, CursorInfo cursorLocation) => { _selectedFaction = (FactionType)source.Data; };
                radioGroup.AddControl(emitterControl);
                layout.AddChild(emitterControl);
            }
            return layout;
        }
        
    }
}

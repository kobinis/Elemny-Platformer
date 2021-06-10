using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.Scenes.Activitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using XnaUtils;
using XnaUtils.Graphics;
using XnaUtils.Input;
using XnaUtils.SimpleGui;

namespace SolarConflict.Framework.Scenes
{
    //TODO: add a vertical layout
    //TODO: readscene in scene component from data 
    public enum SceneComponentType {EscapeMenu, MissionLog, Inventory, Hangar, TacticalMap, GalaxyMap, Crafting, Codex, Imbue }
    [Serializable]    
    public class SceneComponentSelector
    {
        [Serializable]
        public class Component
        {
            public SceneComponentType Type { get; private set; }
            public string Name { get; private set; }
            public string ActivityName { get; private set; }
            public Keys Key { get { return KeysSettings.Data.MenuBindings[Type].Key; } private set { } }
       //     public Keys SecondaryKey { get; private set; }
            public Sprite Icon;

            public Component(SceneComponentType type, string name, string activityName, string iconID)
            {
                this.Type = type;
                this.Name = name;
                this.Icon = Sprite.Get(iconID);
                this.ActivityName = activityName;
                this.Key = KeysSettings.GetSceneComponentKey(type).Key;
               // this.SecondaryKey = secondaryKey;
            }
        }

        public static Component[] components =
        {
            new Component(SceneComponentType.EscapeMenu, "Help & Settings", "EscapeMenuActivity", "settingsIconV2"),
            new Component(SceneComponentType.MissionLog, "Mission Log", "MissionActivity", "missionIconV2"),
            new Component(SceneComponentType.Inventory, "Inventory", "InventoryActivity", "InventoryIconV2"),
            new Component(SceneComponentType.Hangar, "Hangar", "HangarActivity", "CraftingIconV2"),
            new Component(SceneComponentType.TacticalMap, "Tactical Map", "TacticalMapActivity","TacticalMapIconV2"), //Change back to U
            new Component(SceneComponentType.GalaxyMap, "Galaxy Map", "GalaxyMapActivity", "GalaxyMapIconV2"),
            new Component(SceneComponentType.Crafting, "Crafting", "CraftingActivity", "CraftingIconV2"),
            new Component(SceneComponentType.Codex, "Codex", "CodexTmpActivity", "FactionIcon"),
            new Component(SceneComponentType.Imbue, "Imbue", "ImbueActivity", "FactionIcon")
        };

        static HashSet<SceneComponentType> _isPlayerNeeded = new HashSet<SceneComponentType> { SceneComponentType.Inventory };//, SceneComponentType. };

        const int CONTROL_SIZE = 54;

        public SceneComponentType? LastComponentSelected;

        private Dictionary<SceneComponentType, SceneActivityProviderControl> _activityProvidersDictionary;
        //private List<SceneActivityProviderControl> _providersList;
        private GuiManager _gui;
        private VerticalLayout _layout;
        private Scene _scene;
        private Sprite _glowSprite;
        private int _numComponents;

        public SceneComponentSelector(Scene scene) //TODO: add vertical layout
        {
            _scene = scene;
            _gui = new GuiManager();
            _layout = new VerticalLayout(Vector2.Zero);
            _layout.Position = new Vector2(ActivityManager.ScreenSize.X - _layout.HalfSize.X, _layout.HalfSize.Y);
            _gui.Root = _layout;
            //_layout.IsUpdatingPosition = true;
            _activityProvidersDictionary = new Dictionary<SceneComponentType, SceneActivityProviderControl>();
            _glowSprite = Sprite.Get("glow128");
            _numComponents = 0;
            LastComponentSelected = null;
        }

        public void AddComponent(SceneComponentType type, string activityProviderName = null, string activityParams = null)
        {
            RemoveComponent(type);
            SceneActivityProviderControl control = MakeDefaultControl(type, activityProviderName);
            _activityProvidersDictionary[type] = control;
            _layout.AddChild(control);
            _numComponents++;
        }

        public static Keys? GetKeyForComponent(SceneComponentType type) 
        {
            return KeysSettings.GetSceneComponentKey(type).Key;
        }

        public void RemoveComponent(SceneComponentType type)
        {
            if(_activityProvidersDictionary.ContainsKey(type))
            {
                SceneActivityProviderControl control = _activityProvidersDictionary[type];
                _activityProvidersDictionary.Remove(type);
                _layout.RemoveChild(control);
                _numComponents--;
            }
        }

        public SceneActivityProviderControl GetProviderControl(SceneComponentType type)
        {
            SceneActivityProviderControl provider;
            _activityProvidersDictionary.TryGetValue(type, out provider);
            return provider;
        }

        public SceneActivityProviderControl SetProvider(SceneComponentType type)
        {
            SceneActivityProviderControl provider;
            _activityProvidersDictionary.TryGetValue(type, out provider);
            return provider;
        }


        public void Update(InputState inputState)
        {
            _gui.Update(inputState);
            _layout.Position = new Vector2(ActivityManager.ScreenSize.X - _layout.HalfSize.X, _layout.HalfSize.Y);
            foreach (var item in KeysSettings.Data.MenuBindings)
            {
                if(_activityProvidersDictionary.ContainsKey(item.Key))
                {
                    _activityProvidersDictionary[item.Key].ActivationKey = item.Value.Key;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            _gui.Draw();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            foreach (var keyValue in _activityProvidersDictionary)
            {
                if (keyValue.Value.IsGlowing)
                {
                    Color color = Color.Yellow;
                    color.A = (byte)((Math.Sin(_scene.SceneFrameCounter / 20.0) * 0.5 + 0.5) * 200);
                    Rectangle rectangle = FMath.CenterAndSizeToRect(keyValue.Value.Position, Vector2.One * CONTROL_SIZE * 1.13f);
                    sb.Draw(_glowSprite, rectangle, color);
                }
            }
            sb.End();
        }

        public void SwitchActivity(SceneComponentType type)
        {
            SceneActivityProviderControl control = GetProviderControl(type);
            if (control != null)
            {
                if (ActivityManager.Inst.IsCurrentPopupActivity(control.ActivityProvider)) return;
                if (_scene.PlayerAgent == null && control.IsPlayerNeeded)
                    return;
            }
            else
            {
                control = MakeDefaultControl(type);
            }
            SwitchActivity(control.ActivityProvider, control.ActivityParams, control.ComponentData);
            if (control != null)
                control.IsGlowing = false;
            LastComponentSelected = type;
        }

        public Activity SwitchActivity(string activityProvider, ActivityParameters activityParams = null, Component component = null)
        {
            if (activityParams == null)
                activityParams = new ActivityParameters();
            activityParams.DataParams["Scene"] = _scene;
            //if (_scene.CallingAgent != null)
            //    activityParams.DataParams["Calling_agent"] = _scene.CallingAgent;
            if (component != null)
            {
                activityParams.ParamDictionary["Title"] = component.Name;
                activityParams.DataParams["Component"] = component;
            }
            return ActivityManager.Inst.SwitchActivity(activityProvider, activityParams);
        }

        private void OnGui(GuiControl source, CursorInfo cursorLocation)
        {            
            SceneActivityProviderControl control = source as SceneActivityProviderControl;
            control.IsGlowing = false;
            SwitchActivity(control.ComponentData.Type);     
        }

        //public Scene

        public SceneActivityProviderControl MakeDefaultControl(SceneComponentType type, string activityProviderName = null) //TODO move out
        {
            int index = _numComponents;
            Component component = components[(int)type];
           // Vector2 position = new Vector2(ActivityManager.ScreenSize.X - CONTROL_SIZE / 2 - 5, 10 + (index+0.5f) * (CONTROL_SIZE + 5));
            SceneActivityProviderControl control = new SceneActivityProviderControl(Vector2.Zero, Vector2.One * CONTROL_SIZE);
            control.ComponentData = component;
            control.Sprite = component.Icon;
            control.ControlColor = new Color(255, 255, 255, 255);
            //control.ActivationKey = component.Key;
            //control.SecondaryActivationKey = component.SecondaryKey;
            control.SetData("CompType", type);            
            control.LogicFunction = (s, i) => 
            { 
                SceneComponentType t = (SceneComponentType)s.GetData("CompType");
                if(i.IsGKeyPressed(KeysSettings.GetSceneComponentKey(t)))
                {
                    s.InvokeAction(i.Cursor);
                }

                s.TooltipText = t.GetUserName() + " (#color{255,255,0}" + KeysSettings.GetTag(KeysSettings.Data.MenuBindings[t]) + "#dcolor{})";
            };            
           // control.ActivationLogicOperator = GuiControl.ActivationLogicOperatorType.Or;            
            control.ActivityProvider = activityProviderName != null ? activityProviderName : component.ActivityName;
            control.IsPlayerNeeded = _isPlayerNeeded.Contains(type);
            control.CursorOn += _gui.ToolTipHandler;
            control.Action += OnGui;
            control.Index = index;
            return control;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            _layout.Position = new Vector2(ActivityManager.ScreenSize.X - _layout.HalfSize.X, _layout.HalfSize.Y);
        }
    }
}

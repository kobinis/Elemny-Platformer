using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaUtils.SimpleGui.Controllers;
using XnaUtils.Input;
using SolarConflict.GameContent;
using SolarConflict.XnaUtils.SimpleGui;

using XnaUtils.Graphics;
using System.IO;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System.Xml.Serialization;

namespace SolarConflict.GameContent.Activities
{
    //now: add slot size
    //now: add slot default action binding

    class ShipEditor : Activity
    {
        private SlotEditingControl _slotEditingControl;
        private GuiManager _guiHolder;
        private Agent _ship;
        private TextControl _saveButton;
        private SlotType _selectedCategories;

        public ShipEditor()
        {
        }

        public override void OnEnter(ActivityParameters parameters)
        {
            if (parameters != null && parameters.DataParams.ContainsKey("Agent"))
            {
                this._ship = (Agent)parameters.DataParams["Agent"];
            }
            else
            {
                this._ship = ((Agent)MetaWorld.Inst.PlayerShip).GetWorkingCopy(); //TODO: change
            }

            _guiHolder = new GuiManager();
            _guiHolder.Root = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 20));
            _slotEditingControl = new SlotEditingControl(_ship, Vector2.Zero, _guiHolder);
            _guiHolder.Root.AddChild(_slotEditingControl);            
            _saveButton = new TextControl(UIElmentsTexts.Save, Game1.font);
            _saveButton.IsShowFrame = true;
            _saveButton.CursorOverColor = Color.Gray;
            _saveButton.Action += Save;
            _guiHolder.Root.AddChild(_saveButton);

            var load = new RichTextControl("Load", isShowFrame:true);
            load.Action += Load;
            load.CursorOverColor = Color.Gray;
            _guiHolder.Root.AddChild(load);


            GridControl typesControl = CreateSlotTypesControl();
            GridControl rotaionsControl = CreateItemsRotaionsControl();

            _guiHolder.Root.AddChild(typesControl);
            _guiHolder.Root.AddChild(rotaionsControl);

            typesControl.Position = new Vector2(ActivityManager.ScreenRectangle.Width - typesControl.HalfSize.X - 10, typesControl.HalfSize.Y + 20);
            rotaionsControl.Position = new Vector2(rotaionsControl.HalfSize.X + 10, rotaionsControl.HalfSize.Y + 50);

            var activationSelect = CreateActivatioControl();
            _guiHolder.Root.AddChild(activationSelect);

            activationSelect.Position = new Vector2(rotaionsControl.HalfSize.X * 2 + 10 + activationSelect.HalfSize.X + 10, activationSelect.HalfSize.Y + 50);
           


        }

        public override void Draw(SpriteBatch sb)
        {
            ActivityManager.GraphicsDevice.Clear(Color.DarkBlue);
            if (_guiHolder.Root != null)
            {
                _guiHolder.Draw();
            }
        }

        public override void Update(InputState inputState)
        {
            _guiHolder?.Update(inputState);
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
        }

        public void Load(GuiControl source, CursorInfo location)
        {
            _ship.ID = _ship.Sprite.ID + "Hull";
            string path = Path.Combine(Consts.GAME_DATA_PATH, "Agents");
            string filename = _ship.ID + ".xml";
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShipData));
            StreamReader file = new StreamReader(Path.Combine(path, filename));
            ShipData shipdata = (ShipData)xmlSerializer.Deserialize(file);
            file.Close();

            var ship = ShipQuickStart.Make(shipdata, true);
            ship.ID = Path.GetFileNameWithoutExtension(filename);

            _ship = ship;
            _slotEditingControl.Init(_ship, _guiHolder);
            ContentBank.Inst.AddContent(_ship, true);
            // ContentBank.Instship);
            _ship = ship;
            _slotEditingControl.Init(_ship, _guiHolder);

        }

        private void Save(GuiControl source, CursorInfo location)
        {
            //var agents = ContentBank.Inst.GetAllAgents();
            //foreach (var agent in agents)
            //{
            //    string path = Path.Combine(Consts.GAME_DATA_PATH, "Agents");
            //    string filename = agent.ID + ".dat";
            //    FileStream file = new FileStream(Path.Combine(path, filename), FileMode.Create);
            //    agent.Save(file);
            //    file.Close();
            //}


            _slotEditingControl.Save();

            _ship.ID = _ship.Sprite.ID + "Hull";
            string path = Path.Combine(Consts.GAME_DATA_PATH, "Agents");
            string filename = _ship.ID + ".xml";
            //FileStream file = new FileStream(Path.Combine(path, filename), FileMode.Create);
            //_ship.Save(file);
            //file.Close();
           
      

            ShipData shipdata = new ShipData(_ship);
            shipdata.ItemSlotList = _slotEditingControl.GetSlotList();
            XmlSerializer xmlSerializer = new XmlSerializer(shipdata.GetType());
            StreamWriter file = new StreamWriter(Path.Combine(path, filename));
            xmlSerializer.Serialize(file, shipdata);
            file.Close();


            var ship = ShipQuickStart.Make(shipdata, true);
            ship.ID = Path.GetFileNameWithoutExtension(filename);
            
            _ship = ship;
            _slotEditingControl.Init(_ship, _guiHolder);
            ContentBank.Inst.AddContent(_ship, true);
            ActivityManager.Inst.AddToast(_ship.ID + " was saved!", 200);
            //if (_saveButton.CursorOverColor == Color.Red)
            //{
            //    _saveButton.CursorOverColor = Color.Blue;
            //}
            //else
            //{
            //    _saveButton.CursorOverColor = Color.Red;
            //}
        }

        public static Activity ActivityProvider(string param)
        {
            return new ShipEditor();
        }


        private List<SlotType> GetFilteredCategories()
        {
            var filteredCategories = new List<SlotType>() { SlotType.Engine, SlotType.MainEngine, SlotType.Weapon, SlotType.Turret, SlotType.Utility };

            return filteredCategories;
        }
       
        private GridControl CreateActivatioControl()
        {
            GridControl activationSelect = new GridControl(1, 9, new Vector2(200, 50));
            RadioSelectionGroup radio = new RadioSelectionGroup();
            for (int i = 0; i < 9; i++)
            {
                
                TextControl textControl = new TextControl(((ControlSignals)(1 << i)).ToString(), Game1.font);
                textControl.Sprite = Sprite.Get("guif8");
                textControl.TextColor = Color.Black;
                radio.AddControl(textControl);
                textControl.IsToggleable = true;
                textControl.IsShowFrame = true;
                textControl.Action += TextControl_Action;                
                textControl.PressedControlColor = Color.Green;
                activationSelect.AddChild(textControl);
            }            
            return activationSelect;
        }

        private void TextControl_Action(GuiControl source, CursorInfo cursorLocation)
        {
            _slotEditingControl.Activation = (ControlSignals)(1 << source.Index);
        }

        private GridControl CreateSlotTypesControl()
        {
            var filteredCategories = GetFilteredCategories();
            GridControl itemCategoriesSelect = new GridControl(1, filteredCategories.Count, new Vector2(200, 50));

            // Create Text Control for each category

            foreach (var item in filteredCategories)
            {
                TextControl textControl = new TextControl(item.ToString(), Game1.font);
                textControl.Sprite = Sprite.Get("guif8");
                textControl.TextColor = Color.Black;
                textControl.IsToggleable = true;
                textControl.IsShowFrame = true;
                textControl.Action += CategorySelectHandler;
                textControl.UserData = item.ToString();
                textControl.PressedControlColor = Color.Green;
                itemCategoriesSelect.AddChild(textControl);
            }

            return itemCategoriesSelect;
        }

        private GridControl CreateItemsRotaionsControl()
        {
            GridControl itemRotaionsSelect = new GridControl(1, 4, new Vector2(70, 70));
            // Create Image Control for each direction

            string[] textureNames = new string[4] { "arrowRight", "arrowDown", "arrowLeft", "arrowUp" };

            for (int i = 0; i < 4; i++)
            {
                ImageControl control = new ImageControl(Sprite.Get(textureNames[i]), Vector2.Zero, Vector2.Zero);
                control.IsToggleable = true;
                control.Action += RotaionSelectHandler;
                control.UserData = i.ToString();
                control.ControlColor = Color.Black;
                itemRotaionsSelect.AddChild(control);
            }

            return itemRotaionsSelect;
        }

        private void RotaionSelectHandler(GuiControl source, CursorInfo cursorLocation)
        {
            _slotEditingControl.Rotation = source.Index * 90;

            foreach (var item in source.Parent.GetChildren())
            {
                item.ControlColor = Color.Black;
            }
            source.ControlColor = Color.Yellow;

        }


        private void CategorySelectHandler(GuiControl source, CursorInfo cursorLocation)
        {
            if (source.IsPressed)
            {
                _selectedCategories |= (SlotType)Enum.Parse(typeof(SlotType), source.UserData);
            }

            if (!source.IsPressed)
            {
                _selectedCategories &= ~(SlotType)Enum.Parse(typeof(SlotType), source.UserData);
            }

            _slotEditingControl.SlotType = _selectedCategories;
        }
    }
}

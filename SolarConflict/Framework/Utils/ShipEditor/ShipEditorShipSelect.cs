//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using XnaUtils.SimpleGui;
//using SolarConflict.XnaUtils.SimpleGui;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils.Input;

//namespace SolarConflict
//{
//    class ShipEditorShipSelect : Activity
//    {
//        GuiHolder _gui;
//        GridControl _shipGrid;     


//        public ShipEditorShipSelect()        
//        {
//            _gui = new GuiHolder(ActivityManager.SpriteBatch);
//            _shipGrid = new GridControl(9, 4, Vector2.One * 100); //According to res
//            var ships = ContentBank.Inst.GetAllAgents(); //Add Filters

//            foreach (var ship in ships)
//            {
//                ShipControl control = new ShipControl(ship, Vector2.One * 150, Vector2.One * 100);
//                control.CursorOn += _gui.ToolTipHandler;
//                _shipGrid.AddChild(control);
//                control.Action += EditShipSlots;
//            }

//            _gui.root = new GuiLayout(new Vector2(ActivityManager.screenRectangle.Width / 2, 10));
//            _gui.root.AddChild(_shipGrid);

//            _gui.Update(InputState.EmptyState);
//        }
        
//        //TODO: understand and complete
//        private void EditShipSlots(GuiControl source, CursorInfo cursorLocation)
//        {
//            ShipControl shipControl = (ShipControl)source;
           
//            //ActivityManager.SwitchActivity();
//           // ActivityManager.SwitchActivity(new ShipEditActivity(shipControl.Ship));
//        }

//        #region Update/Draw
//        public override bool Update(InputState inputState)
//        {
//            _gui.Update(inputState);
//            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
//                ActivityManager.Back();
//            return false;
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//            _gui.Draw();
//        }
//        #endregion

//        public static Activity ActivityProvider(string param)
//        {
//            return new ShipEditorShipSelect();
//        }
//    }
//}

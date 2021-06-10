//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using SolarConflict.Framework;

//namespace SolarConflict.GameContent.Activities
//{
//    class SettingsActivity:Activity
//    {
//        MenuData data;

//        protected override void Init(ActivityParameters parameters)
//        {
//            data = new MenuData("Settings");
//            data.MenuEntryList.Add(new Framework.Menu.MenuEntry("Mouse and Keys"));
//            data.MenuEntryList.Add(new Framework.Menu.MenuEntry("Controller"));
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new SettingsActivity();
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//            //throw new NotImplementedException();
//        }

//        public override void Update(InputState inputState)
//        {
//        }
//    }
//}

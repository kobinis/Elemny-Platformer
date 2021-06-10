//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.Framework.Scenes.Activitys;
//using SolarConflict.GameContent;
//using System.Linq;
//using XnaUtils;
//using XnaUtils.Input;
//using XnaUtils.SimpleGui;
//using XnaUtils.SimpleGui.Controllers;

//namespace SolarConflict.GameContent.Activities
//{
//    public class KeysActivity : SceneActivity
//    {
//        private GuiManager _guiHolder;        

//        public KeysActivity()
//        {
//            //ActivityKeys = new Keys[] { Keys.F1 };
//            _guiHolder = new GuiManager();

//            GuiLayout layout = new GuiLayout(new Vector2(ActivityManager.ScreenRectangle.Width / 2, 100));

//            _guiHolder.Root = layout;

//            TextControl title = new TextControl("Keys Display", Game1.menuFont);
//            title.TextColor = Color.Red;
//            layout.AddChild(title);

//            TextControl keys = new TextControl(@"            Move Forward - W
//            Move Backwards - S
//            Steer/Rotate/Move Left - A
//            Steer/Rotate/Move Right - D
//            Activate Weapon 1 - left mouse button
//            Activate Weapon 2 - right mouse button
//            Activate Special ability 1 - Q
//            Activate Special ability 2 - E
//            Mouse scroll - Zoom in and out
//            Space - Skip text
//            Tab - Switch between your ships
//            Shift (holding) - Lock ship rotation (Use it if you want to rotate your 
//                              turrents without turning your ship).
//            1,2,3,4 - activate items in inventory slots 1,2,3,4
//            F1 - Keys Display
//            F2 / M - Mission Log
//            F3 / I - Inventory
//            F4 / C - Crafting 
//            F5 / T- Tactical Map
//            Esc - Exit");
//            keys.TextColor = Color.White;
//            layout.AddChild(keys);

//            title = new TextControl("Tips", Game1.menuFont);
//            title.TextColor = Color.Red;
//            layout.AddChild(title);

//            title = new TextControl(@"            If your shield is low on energy, fly back to your mothership for fast recharge.");
//            title.TextColor = Color.White;
//            layout.AddChild(title);

//            title = new TextControl("Remarks", Game1.menuFont);
//            title.TextColor = Color.Red;
//            layout.AddChild(title);

//            title = new TextControl(@"                        The demo doesn't support key configuration.
//The demo doesn't support saving, if you exit the game you will start from the beginning.");
//            title.TextColor = Color.White;
//            layout.AddChild(title);

//            layout.Position = new Vector2(layout.Position.X, -10);
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//          //  Background.Draw(_scene.Camera);
//            _guiHolder.Draw();
//            base.Draw(sb);
//        }

//        public static Activity ActivityProvider(string parameters)
//        {            
//            return new KeysActivity();
//        }
//    }
//}

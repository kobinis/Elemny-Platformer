//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using XnaUtils;
//using SolarConflict.Session;
//using System.IO;
//using SolarConflict.Framework.Utils;
//using SolarConflict.XnaUtils.SimpleGui;
//using Microsoft.Xna.Framework;
//using XnaUtils.SimpleGui.Controllers;
//using XnaUtils.SimpleGui;
//using XnaUtils.Input;
//using SolarConflict.Framework;

//namespace SolarConflict.GameContent.Activities
//{           
//    /// <summary>
//    /// Contine or start a new game
//    /// </summary>
//    public class ContinueGameActivity : Activity
//    {
//        //TODO: TOM - look at saving dir if there          
//        string _parameters;
//        bool _reset;
//        public ContinueGameActivity(string parameters)
//        {
//            _parameters = parameters;
//            _reset = parameters == "Reset";

                      
//        }

//        protected override void Init(ActivityParameters parameters)
//        {
//            _reset = parameters == "Reset";
//        }

//        public override void Draw(SpriteBatch sb) { }

//        public override bool Update(InputState inputState)
//        {
//            if (!_reset && LoadGameActivity.GetMostRecentSave() != null)
//            {
//                LoadGame(LoadGameActivity.GetMostRecentSave());
//            }
//            else
//            {
//                GameSession.Reset();
//                GameSession.Inst.Continue();
//            }
//            return true;
//        }

//        private void LoadGame(string path)
//        {
//            GameSession.Load(path);
//            GameSession.Inst.Continue();
//        }

//        public static Activity ActivityProvider(string parameters)
//        {                       
//            return new ContinueGameActivity(parameters);
//        }        

//        //private void LoadNameSelectHandler(GuiControl source, CursorInfo cursorLocation)
//        //{
//        //    if (source.UserData != null)
//        //    {
//        //        //LoadFile(source.UserData);
//        //    }

//        //    _guiHolder.Root.RemoveChild(_loadNamesSelect);
//        //    _loadNamesSelect = null;
//        //}
//    }
//}

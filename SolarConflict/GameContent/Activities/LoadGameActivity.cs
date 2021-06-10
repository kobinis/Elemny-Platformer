//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Utils;
//using SolarConflict.Session;
//using SolarConflict.XnaUtils.SimpleGui;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using XnaUtils;
//using XnaUtils.Graphics;
//using XnaUtils.Input;
//using XnaUtils.SimpleGui;
//using XnaUtils.SimpleGui.Controllers;

//namespace SolarConflict.GameContent.Activities {
    
//    class LoadGameActivity : Activity {
//        GuiManager _gui;

//        public LoadGameActivity(string parameters) {            
//            MakeGUI();
//        }                                             

//        public override void Draw(SpriteBatch sb) {
//            _gui.Draw();
//        }

//        public static string GetMostRecentSave() {
//            return GetSaveFiles().FirstOrDefault()?.Item1;
//        }

//        public static string GetPathToNewSave() {
//            var names = GetSaveFiles().Select(n => Path.GetFileNameWithoutExtension(n.Item1));

//            var maximalSuffix = (names.Count() == 0) ? -1 : names.Select(n => n.Substring(4))
//                .Select(s => int.Parse(s))
//                .Max();

//            return $"{Consts.SESSION_SAVE_PATH}/save{++maximalSuffix}.sesh";
//        }

//        /// <summary>[(path, metadata)]</summary>
//        /// <remarks>Sorted by date last written to, descending (so most recent saves first)</remarks>
//        static Tuple<string, SessionMetadata>[] GetSaveFiles() {
//            String dirName = Consts.SESSION_SAVE_PATH;
//            if (!Directory.Exists(dirName))
//            {
//                // TODO: (adamshai) create the directory in another place, not in the getter
//                Directory.CreateDirectory(dirName);
//            }
//            return Directory.GetFiles(dirName, "*.sesh")
//                .OrderByDescending(n => File.GetLastWriteTime(n).Ticks)
//                .Select(n => Tuple.Create(n, LoadMetaData(n + ".meta"))).ToArray();
//        }

//        static SessionMetadata LoadMetaData(string path) {
//            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
//            var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
//            var result = formatter.Deserialize(file) as SessionMetadata;
//            file.Close();

//            return result;
//        }

//        void MakeGUI() {            
//            _gui = new GuiManager();

//            var grid = new ScrollableGrid(1, 10, new Vector2(500, 50));

//            grid.Position = Vector2.One * 10 + grid.HalfSize;

//            var files = GetSaveFiles();
//            for (int i = 0; i < files.Length; i++) {
//                var horizontal = new HorizontalLayout(Vector2.Zero);                
//                var loadButton = new TextControl(Path.GetFileNameWithoutExtension(files[i].Item1) + ": " + files[i].Item2.dummyText
//                    , Game1.font);
//                loadButton.IsShowFrame = true;
//                loadButton.CursorOn += OnMouseoverButton;
//                loadButton.UserData = files[i].Item1;

//                var deleteButton = new ImageControl(Sprite.Get("replace"), Vector2.Zero, new Vector2(20, 20));
//                deleteButton.IsShowFrame = true;
//                deleteButton.CursorOn += OnMouseoverButton;
//                deleteButton.UserData = files[i].Item1;

//                horizontal.AddChild(loadButton);
//                horizontal.AddChild(deleteButton);
//                grid.AddChild(horizontal);
//            }

//            _gui.Root = grid;
//        }

//        void OnMouseoverButton(GuiControl source, CursorInfo cursorInfo) {
//            // Handle left clicks
//            if (cursorInfo.OnPressLeft) {
//                if (source is TextControl)
//                    // It's a load button
//                    GameSession.Load(source.UserData);
//                else if (source is ImageControl) {
//                    // It's a delete button
//                    File.Delete(source.UserData);
//                    MakeGUI();
//                }
//            }            
//        }

//        public override bool Update(InputState inputState) {
//            _gui.Update(inputState);

//            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
//                ActivityManager.Back();

//            return false;
//        }

//        public static Activity ActivityProvider(string parameters) {
//            return new LoadGameActivity(parameters);
//        }
//    }
//}

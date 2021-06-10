//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Media;
//using SolarConflict.AI;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Emitters;
//using SolarConflict.Framework.GameObjects.Emitters;
//using SolarConflict.Framework.GUI.ParserCommands;
//using SolarConflict.Framework.PlayersManagement;
//using SolarConflict.Framework.Scenes.Activitys;
//using SolarConflict.Framework.Scenes.Components;
//using SolarConflict.GameContent;
//using SolarConflict.GameContent.Activities;
//using SolarConflict.GameContent.Activities.Levels;
//using SolarConflict.GameContent.ContentGeneration;
//using SolarConflict.GameContent.ContentGeneration.Items;
//using SolarConflict.GameContent.ContentGeneration.Projectiles;
//using SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using SolarConflict.Generation.TemplateGenerationEngine;
//using SolarConflict.Generation.TemplateGenerationEngine.Templates;
//using SolarConflict.Session;
//using SolarConflict.XnaUtils;
//using SolarConflict.XnaUtils.Files;
//using SolarConflict.XnaUtils.SimpleGui;
//using SolarConflict.XnaUtils.SimpleGui.TextureGeneration;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Threading;
//using XnaUtils;
//using XnaUtils.Framework.Graphics;
//using XnaUtils.Graphics;
//using XnaUtils.SimpleGui;

//namespace SolarConflict //changed
//{
//    class Game1 :DrawableGameComponent
//    {

//        public static string SavePath = string.Concat(new object[]
//        {
//            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
//            Path.DirectorySeparatorChar,
//            "My Games",
//            Path.DirectorySeparatorChar,
//            "StarSingularity"
//        });



//        public Game1(Game game, string[] args)
//            : base(game)
//        {

//            //ActivityManager.SetWindowPosition();
//            //DebugUtils.Mode = ModeType.Release;
//        }













//        public override void Update(GameTime gameTime)
//        {                                  
//            base.Update(gameTime);


//        }

//        public override void Draw(GameTime gameTime)
//        {
//            base.Draw(gameTime);
//            ActivityManager.Inst.Draw(sb);            
//        }

//        protected override void Dispose(bool disposing)
//        {
//               base.Dispose(disposing);
//               TextureBank.Inst.Dispose();
//        }



//    }
//}

//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine;
//using SolarConflict.NewContent.Projectiles;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.GameContent.ContentGeneration
//{
//    public class AsteroidTemplate : GenerationTemplate
//    {
//        public String Name { get; private set; }
//        public float Level { get; private set; } // TODO: where we going to use this data, should we add it to a bank?

//        public AsteroidTemplate()
//        {
//            _directoryName = "Asteroids";
//            AddParametereName(ID);
//            AddParametereName(NAME);
//            AddParametereName(TEXTURE);
//            AddParametereName("Level");
//            AddParametereName(COLOR);
//            AddParametereName("Size");
//        }



//        protected override void ParseAndAddEmitter(string[] parameters)
//        {
//            ProjectileProfile asteroid = CreateAsteroidBySize();

//            Name = csvUtils.GetString(NAME);
//            asteroid.ID = Name;

//            if (!string.IsNullOrEmpty(csvUtils.GetString(TEXTURE)))
//            {
//                asteroid.Sprite = Sprite.Get(csvUtils.GetString((TEXTURE)));
//            }

//            asteroid.InitColor = new InitColorConst(csvUtils.GetColor(COLOR, DEFAULT_COLOR));
//            ContentBank.Inst.AddContent(asteroid);
//        }

//        private ProjectileProfile CreateAsteroidBySize()
//        {
//            string size = csvUtils.GetString("Size");
//            ProjectileProfile asteroid = null;

//            if (String.Compare(size, "Big", StringComparison.OrdinalIgnoreCase) == 0)
//            {
//                asteroid = BigAsteroid.Make();
//            }
//            else
//            {
//                asteroid = Asteroid.Make();
//            }

//            return asteroid;
//        }

//        private static GroupEmitter CreateLootEmitter(string[] parameters)
//        {
//            var lootEmitter = new GroupEmitter();

//            for (int i = 4; i < parameters.Length; i++)
//            {
//                if (parameters[i] != string.Empty)
//                {
//                    string[] material = parameters[i].Split(':');

//                    if (material.Length > 1)
//                    {
//                        float rarity = Parser.ParseFloat(material[1]);

//                        if (rarity > 0)
//                        {
//                            lootEmitter.AddEmitter(ContentBank.Inst.GetEmitter(material[0]), rarity);
//                        }
//                    }
//                }
//            }

//            return lootEmitter;
//        }
//    }
//}

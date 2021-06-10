using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration.Projectiles
{    

    public class AsteroidGeneration
    {
        private struct LootEntry
        {
            public string Emitter;
            public float Probability;
            public int AmountMin;
            public int AmountRange;

            public LootEntry(string emitter, float probability, int amountMin, int amountRange)
            {
                Emitter = emitter;
                Probability = probability;
                AmountMin = amountMin;
                AmountRange = amountRange;
            }
        }

        private class AsteroidGenerationProfile
        {
            
        }
       
        public static void GenerateAsteroidsProfiles()
        { 
            //Add Lava Asteroid 3 levels
            //Add Bio Asteroids
            string baseID = "Asteroid";
            
            float hitpoints = 200;
            float hitPointMult = 1.5f;
            float size = 80;
            float sizeMult = 1.05f;
            //Replace with a palette            
            List<Color?> colors = new List<Color?>() { null, Color.LawnGreen, Color.Red, Color.DeepSkyBlue, Color.Purple, Color.Yellow, Color.Gold};
            int levelsNum = 7;
                        
            for (int i = 1; i < levelsNum; i++)
            {
                IEmitter loot = ContentBank.Inst.GetEmitter("AsteroidLoot" + i.ToString());          
                AsteroidData data = new AsteroidData(baseID + i.ToString(), i);
                                               
                data.Name = ContentBank.Inst.GetItem("MatA" + i.ToString(),false).Tag + " Asteroid";
                
                data.Color = colors[i-1];
                data.Size = size;
                data.Hitpoints = hitpoints;
                data.Loot = loot;                
                data.GenerateSmallAsteroid = true;
                data.SmallLoot = data.Loot;
                AsteroidQuickStart.MakeAndAdd(data);

                hitpoints *= hitPointMult;
                size *= sizeMult;
            }

        }
        
        

    }
}

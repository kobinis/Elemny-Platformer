using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.Utils;
using SolarConflict.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.World.Generation.Profiles
{
    class AsteroidsFeature : GenerationFeature
    {
        public Vector2 Center { get; set; }
        public int NumberOfLevels = 3;
        public int StartingLevel = 0;
        public string baseName = "Asteroid";
        public int clusterMaxSize = 3000;
        public int clusterMinSize = 1000;
        public int MaxNumberOfBatches = 16;
        public int MinNumberOfBatches = 10;
        public List<IEmitter> DefenceEmitters;
        public float DefenceEmittersProb = 0.5f;
        public int DefenceNumberMin =2;
        public int DefenceNumberRange = 1;


        public AsteroidsFeature(int startingLevel, int numberOfLevels = 1) :base()
        {
            StartingLevel = startingLevel;
            NumberOfLevels = numberOfLevels;
            List<string> asteroids = new List<string>();
            DefenceEmitters = new List<IEmitter>();
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            float maxRadius = generator.SceneRadius;
            float minRadius = generator.SunRadius;
            for (int i = 0; i < NumberOfLevels; i++)
            {
                string name = baseName + (i + StartingLevel).ToString();
                float minRad = maxRadius - (i+1) * (maxRadius - minRadius - clusterMaxSize) / (float)(NumberOfLevels);
                float maxRad = maxRadius - i*(maxRadius - minRadius - clusterMaxSize) /(float)(NumberOfLevels);

                int numberOfBataches = (int)Math.Round((1f - i / (float)NumberOfLevels)*(MaxNumberOfBatches-MinNumberOfBatches) + MinNumberOfBatches);
                for (int j = 0; j < numberOfBataches; j++)
                {
                    int amount = Rand.Next(2, 7); //TODO: change
                    Vector2 centerPoint = Center + FMath.ToCartesian(FMath.TransformToRadius((float)Rand.NextDouble(), maxRad, minRad), (float)Rand.NextDouble() * MathHelper.TwoPi);
                    float rad = Rand.Next(clusterMinSize, clusterMaxSize);

                    var rockId = name;
                    if (!ContentBank.Inst.ContainsEmitter(name))
                        // Rock not found in content bank, get a random lower-level rock, instead
                        rockId = RandomLowLevelRockId();
                    
                    scene.AddObjectRandomlyInLocalCircle(rockId, amount, rad*0.9f, centerPoint, seed:Rand.Next());
                    if (ContentBank.Inst.ContainsEmitter("Small" + rockId))
                        scene.AddObjectRandomlyInLocalCircle("Small" + rockId, amount, rad, centerPoint, seed: Rand.Next());

                    if (DefenceEmitters.Count > 0 && Rand.NextFloat() < DefenceEmittersProb)
                    {
                        int num = DefenceNumberMin + Rand.IntBetween(0, DefenceNumberRange);
                        for (int n = 0; n < num; n++)
                        {
                            Vector2 pos = centerPoint + FMath.ToCartesian(rad, Rand.NextFloat() * MathHelper.TwoPi);
                            int level = generator.Rand.Next(1, i + StartingLevel);
                            var defenceAgent = DefenceEmitters[Math.Min(DefenceEmitters.Count - 1, i)].Emit(scene.GameEngine, null, FactionType.Pirates1, pos, Vector2.Zero, 0,param: level) as Agent;
                            if(defenceAgent != null)
                            {
                                defenceAgent.Name = "Automated defense " + level.ToString();
                                defenceAgent.AddSystem(new AgentSleepSystem());
                                LootSystem loot = new LootSystem();
                                loot.LootEmitter = ContentBank.Inst.GetEmitter("TurretALoot");
                                defenceAgent.AddSystem(loot);
                                defenceAgent.AddSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed, 0.1f));
                            }
                            // TODO: see the node level param above? Make it easier for all features to pass it to all Emit() calls
                        }
                    }
                }              
            }
            return null;
        }
       
        /// <summary>Return the content bank ID of a random asteroid at or _below_ our starting level</summary>
        string RandomLowLevelRockId() {
            return Utility.Range(0, StartingLevel + 1).Select(i => $"{baseName}{i}")
                .Where(id => ContentBank.Inst.ContainsEmitter(id))
                .Choice(Rand);
        }
    }
}

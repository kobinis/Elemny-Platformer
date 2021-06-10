using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using XnaUtils;
using SolarConflict.Framework;
using SolarConflict.NodeGeneration.Features;
using SolarConflict.Session.World.Generation.Features;

namespace SolarConflict.Session.World.Generation.Profiles
{
    /// <summary>
    /// Added the the faction bases in positions
    /// </summary>
    class FactionsFeature: GenerationFeature
    {
        int numberOfBases = 4;
     //   public List<BaseFeature> BaseList;
        public float BaselineRadius = 0;
        public float VariationRadius = 2000;
        public bool HasRoads;
        //Add highways?

        public FactionsFeature(FactionType faction):base()
        {
            localFaction = faction;                        
        }

        public override GameObject Generation(Scene scene, SceneGenerator generator)
        {
            var go = GenerationLogic(scene, generator);
            int counter = 0;
            var agents = new List<GameObject>();
            foreach (var child in children.OrderBy(c => c.Priority))
            {
                child.LocalPosition = GetBasePosition(counter, generator);
                agents.Add(child.Generation(scene, generator));
                counter++;
            }

            //Add Arena
            
            // Add roads
            if (HasRoads) {
                for (int i = 0; i < agents.Count; i++) {
                    var start = agents[i].Position;
                    var end = agents[(i + 1) % agents.Count].Position;

                    var emptyZoneSize = 800f; // Roads shouldn't start less than this distance from the feature

                    var diff = end - start;
                    if (diff.Length() > 2f * emptyZoneSize) {
                        var direction = (end - start).Normalized();
                        start += direction * emptyZoneSize;
                        end -= direction * emptyZoneSize;

                        scene.AddObjectInLine("SpeedBooster", start, end, spacingMultiplier: 5);
                    }
                }
            }
            
            return go;
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            Init(numberOfBases,scene.GameEngine.Rand);
            return null;
        }

        private Vector2 GetBasePosition(int index, SceneGenerator generator)
        {
            float angle = index / (float)children.Count * MathHelper.TwoPi;
            float rad = BaselineRadius == 0 ? generator.SceneRadius * 0.5f : BaselineRadius;
            Vector2 position = FMath.ToCartesian(rad, angle) + FMath.ToCartesian(Rand.NextFloat() * MathHelper.TwoPi, VariationRadius); //TODO: change to be uniformly in the circle (not on ring)
            return position;
        } 

        public void Init(int numberOfBases, Random rand)
        {
            int arenaIndex = rand.Next(numberOfBases);
            Faction faction = MetaWorld.Inst.GetFaction(Faction);         
            for (int i = 0; i < numberOfBases; i++)
            {
                // BaseFeature baseFature = new BaseFeature(faction.GenerationData.MothershipID, this.Faction);
                // baseFature.TurretNumber = 3;
                if (i != arenaIndex)
                {
                    MothershipFeature baseFature = new MothershipFeature(faction.GenerationData.MothershipID);
                    baseFature.ID = "Mothership" + Faction.ToString() + i.ToString();
                    AddChild(baseFature);
                }
                if(i == arenaIndex)
                {
                    if (rand.Next(2) == 1)
                    {
                        ShopFeature shop = new ShopFeature(Faction, Level, 5);
                        AddChild(shop);
                    }
                    else
                    {
                        ShipyardFeature shipyard = new ShipyardFeature();
                        AddChild(shipyard);
                        //if (rand.Next(2) == 0)
                        //{
                        //    ArenaFeature arena = new ArenaFeature();
                        //    //arena.LocalPosition = Vector2.One * 1500;
                        //    AddChild(arena);
                        //}
                        //else
                        //{
                           
                        //}
                    }
                }
                
            }
        }
        
    }
}

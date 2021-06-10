using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.InGameEvent.Content;
using SolarConflict.Framework.InGameEvent.GenericProcess;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using SolarConflict.Framework.World.Generation;
using SolarConflict.Framework.World.Generation.Profiles;
using SolarConflict.GameContent.Activities;
using SolarConflict.NodeGeneration;
using SolarConflict.NodeGeneration.NodeProcesess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using static SolarConflict.Framework.HudManager;

namespace SolarConflict.GameWorld
{    
    /// <summary>
    /// Holds the data and the logic needed to make a specific scene
    /// </summary>
    public class SceneGenerator
    {
        [Flags]
        public enum ZoneFlags
        {
            None = 0,
            IsClearZone = 1 << 0,
            IsRoadTarget = 1 << 1,
            IsDefentedByTurrets = 1 << 2,
            IsPotentialTarget = 1 << 3, 
            IsRaceTarget = 1 << 4         
        }

        public struct GenerationZone
        {
            public ZoneFlags Flags;
            public GameObject GameObject;
        }
        
        public NodeInfo NodeInfo;
        public int SceneRadius = 50000;
        public Vector3 AmbiantColor = Vector3.One * 0.5f;


        public Vector2 PlayerStartingPoint;
        public float startingPosMult = 0.8f;
        public Random Rand;
       // public Random PlanetRandom;

        //Data saved during generation
        public int SunRadius = 0;
        public GameObject Centerpice;
        public List<GenerationZone> Zones;
        

        public int Level { get { return NodeInfo.Level; } }

        private GenerationFeature root;
        PlanetGenerator planetGenerator;

        public LightObject lightObject;


        public SceneGenerator(NodeInfo nodeInfo)
        {
            NodeInfo = nodeInfo;        
            Rand = new Random(nodeInfo.RandomSeed);
            var rand = new Random(nodeInfo.RandomSeed);
            PlayerStartingPoint = nodeInfo.Position.Normalized() * SceneRadius * 1.1f;//  FMath.ToCartesian(SceneRadius * 1.1f, FMath.Rand.NextFloat()*MathHelper.TwoPi);
            root = new GenerationFeature(Rand);            
            root.SetLevel(NodeInfo.Level);
            root.SetFaction(FactionType.Neutral);
            planetGenerator = new PlanetGenerator();
           // PlanetRandom = new Random(nodeInfo.Name.GetHashCode());
        }   
        
        public void AddFeature(GenerationFeature feature)
        {
            root.AddChild(feature);
        }

        public void GenerateScene(Scene scene)
        {
            if(lightObject != null)
            {            
                scene.GameEngine.PermanentLights.Add(lightObject);
            }

            scene.GameEngine.Level = Level;

            scene.HudManager.AddComponent(new RespawnIndicator());
            scene.HudManager.AddComponent(new MothershipStatusIndicator());
            scene.HudManager.AddComponent(new PlayerGoalsIndicator(), PositionType.TLtoBL);
            // scene.HudManager.AddComponent(new FactionShipsHudCmp());

            scene.GameEngine.AmbientColor = AmbiantColor;
            System.Diagnostics.Debug.Assert(root.Level == NodeInfo.Level, "Inconsistent generation level (did you pass the SceneGenerator a node other than the one it was initialized with?)");
            
            scene.GameEngine.Factions[(int)FactionType.Player] = MetaWorld.Inst.GetFaction(FactionType.Player);
            Random oldRand = scene.GameEngine.Rand;
            scene.GameEngine.Rand = Rand;    
            scene.PlayerStartingPoint = GetStartingPoint();
            root.Generation(scene, this);

         


            scene.GameEngine.Rand = oldRand;

            scene.SetBackground((int)NodeInfo.Type);
//            scene.AddObjectRandomlyInCircle("Asteroid0", 1000, this.SceneRadius, this.SceneRadius * 0.8f); //NOW: fix

            scene.GameEngine.AddGameProcces(new GameOverInvokeProcess());
            //scene.GameEngine.AddGameProcces(new GlobalFactionSetterProcess()); //TODO:Fix
            scene.GameEngine.AddGameProcces(new PlayerDeathProcess());
            scene.Camera.Position = this.PlayerStartingPoint;

            //foreach (var item in NodeInfo.FactionsByStrength)
            //{
            //    scene.GameEngine.AddGameProcces(new GenerateDestroyMissionProcess(item));
            //    scene.GameEngine.AddGameProcces(new GenerateDestroyMissionProcess(item));
            //    //scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(item));
            //    //scene.GameEngine.AddGameProcces(new GenerateFetchMissionProcess(item));
            //} 
            PlanetGenerator.GeneratePlanets(this, scene);
        }

        private Vector2 GetStartingPoint()
        {
            if(PlayerStartingPoint == null)
            {
                return FMath.ToCartesian(SceneRadius * startingPosMult, Rand.NextFloat() * MathHelper.TwoPi);
            }
            return PlayerStartingPoint;
        }

        //private void InitScene(SceneGenerator generator, Scene scene)
        //{
        //    scene.SceneID = generator.NodeInfo
        //}

        //Geography:
        //Centerpice {sun, black hole, asteroid filed...}
        //Asteroids
        //Mines
        //Biomes and creatures {Spwaeers}
        //Netural: {Creates, containers, storms,...}
        //Events {pirate attack, traveling mearchent, Bosses..., cargo ships}
        //Constant events {Player lost, Mothership destryed}        
        //Add institutes: {Starport( around player ship), Shops, arenas, black market...}
        //Add Factions {Base,..., roads}
        //Add quests                      
        //Star Gates {moves you instantly to other nodes, without mothership?}
        //Artifacts
    }
}

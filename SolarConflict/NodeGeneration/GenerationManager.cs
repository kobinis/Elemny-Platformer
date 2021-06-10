using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Emitters;
using SolarConflict.Framework.InGameEvent.Content;
using SolarConflict.Framework.InGameEvent.GenericProcess;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using SolarConflict.Framework.Utils;
using SolarConflict.Framework.World.Generation;
using SolarConflict.Framework.World.Generation.Profiles;
using SolarConflict.Framework.World.MetaGame;
using SolarConflict.GameWorld;
using SolarConflict.NodeGeneration.Features;
using SolarConflict.NodeGeneration.Features.CenterFeatures;
using SolarConflict.NodeGeneration.Features.Missions;
using SolarConflict.NodeGeneration.NodeProcesess;
using SolarConflict.Session.World.Generation.Profiles;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Session.World.Generation
{
    /// <summary>
    /// 
    /// </summary>
    public class GenerationManager
    {           
        public GenerationManager()
        {            
        }

        public Scene Generate(NodeInfo info)
        {
            Scene scene = new Scene(null, true, info.Level);
            scene.IsShipSwitchable = true;
            scene.SaveOnExit = true;
            scene.IsConfirmQuitNeeded = true;
            scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.MissionLog);
            scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory);
            scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Hangar);
            scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.TacticalMap);
            scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.GalaxyMap);
            scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Codex);
            // scene.SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.FactionInfo);
            scene.HudManager.AddComponent(new RespawnIndicator());
            
            scene.TryInit(null);
            
            var generator = MakeSceneGenerator(info);
            generator.GenerateScene(scene);

            scene.AddObjectRandomlyInCircle("Asteroid1", 500, generator.SceneRadius *1.2f, generator.SceneRadius * 0.9f); //remove
            //scene.MissionManager.AddMission(MissionFactory.EnteringInterStllerSpace(generator.SceneRadius * 1.3f));
            scene.GameEngine.AddGameProcces(GameEventFactory.MakeCargoShipSpawner());
            scene.GameEngine.AddGameProcces(GameEventFactory.MakeCargoShipSpawner());
            scene.GameEngine.AddGameProcces(GameEventFactory.MakePirateRaidSpawner());

            if (info.Level >= 2 && info.Type == NodeType.Vile)
                scene.GameEngine.AddGameProcces(GameEventFactory.MakeBlobSpawner());

            if (info.Level >= 3 && (info.Type == NodeType.Vile || info.Type == NodeType.AsteroidField))
                scene.GameEngine.AddGameProcces(GameEventFactory.MakeBlobSpawner());

            //GameEngine.AddGameProcces(new GenerateFetchMissionProcess(FactionType.Federation));
            scene.UpdatePlayerShip = true;
            return scene;
        }

        public SceneGenerator MakeSceneGenerator(NodeInfo info) 
        {
            SceneGenerator generator = new SceneGenerator(info);
            AddNoteTypeFeatures(generator);
            //AddAsteroidFeature(info);
            // generator.AddFeature(_asteroiBasedFeature);
            //generator.AddFeature(_asteroidFeature);
            var shopf = new ShopFeature(FactionType.Neutral, info.Level, 10);
            shopf.Position = generator.PlayerStartingPoint + FMath.ToCartesian(10000, generator.Rand.NextFloat() * MathHelper.TwoPi);
            generator.AddFeature(shopf);
            var warpInhib = new PositionFeature();
            warpInhib.SetLevel(Math.Max(info.Level - 2, 0)); //TODO: remove
            warpInhib.AddChild(new WarpInhibitorFeature());
            generator.AddFeature(warpInhib);
            FactionsFeature ff = new FactionsFeature(info.ControllingFaction);
            ff.HasRoads = info.Type == NodeType.RedSun;  
            generator.AddFeature(ff);
            if (info.FactionsByStrength.Count > 1)
            {
                Faction faction = MetaWorld.Inst.GetFaction(info.FactionsByStrength[1]);
                MothershipFeature ms = new MothershipFeature(faction.GenerationData.MothershipID, faction.FactionType);
                ms.Position = generator.Rand.PointInCircle(30000);
                generator.AddFeature(ms);
                //TODO: maybe add another one
            }

            //// Resource mines            
            //var minePattern = new Circle(generator.SceneRadius).Transforms(minResourceMines + generator.Rand.IntBetween(0, resourceMinesRange), generator.Rand);
            //minePattern.Do(t => generator.AddFeature(new ResourceMineFeature() { Position = t.Position }));

            //AddAsteroidFieldFeatures(generator);

            LayersProfile creates = new LayersProfile(false);
            if(info.Type == NodeType.AsteroidField)
                creates.BaseName = "CrateB";
            else
                creates.BaseName = "CrateA";
            creates.startingLevel = 1;
            creates.numberOfLevels = 1;
            creates.MaxNumberOfBatches = 100;
            generator.AddFeature(creates);

            StarportFeature sp = new StarportFeature();
            generator.AddFeature(sp);
            sp.Position = generator.PlayerStartingPoint - new Vector2(6000, 1000);


            int mineLevel = Math.Min(generator.NodeInfo.Level, 6);
            LayersProfile mines = new LayersProfile(true);
            mines.BaseName = "ResourceMine";
            mines.startingLevel = mineLevel;
            mines.numberOfLevels = 1;
            mines.MaxNumberOfBatches = 3;
            mines.MinNumberOfBatches = 2;
            generator.AddFeature(mines);

            generator.AddFeature(new GameProcessFeature(GameEventFactory.MakeCargoShipSpawner()));
            generator.AddFeature(new GameProcessFeature(GameEventFactory.MakeCargoShipSpawner()));
            generator.AddFeature(new GameProcessFeature(GameEventFactory.MakePirateRaidSpawner()));
            return generator;
        }


        public void AddNoteTypeFeatures(SceneGenerator generator)
        {
            switch (generator.NodeInfo.Type)
            {
                case NodeType.RedSun:
                    AddFeaturesRedSun(generator);
                    break;
                case NodeType.BinaryStar:
                    AddFeaturesBinaryStar(generator);
                    break;
                case NodeType.WhiteDwarf:
                    AddFeaturesWhiteDwarf(generator);
                    break;
                case NodeType.AsteroidField:
                    AddFeaturesAsteroidField(generator);
                    break;
                case NodeType.Nebula:
                    AddFeaturesNebula(generator);
                    break;
                case NodeType.Vile:
                    AddFeaturesVile(generator);
                    break;
                case NodeType.BlueGiant:
                    AddFeaturesBlueGiant(generator);
                    break;                                    
                default:
                    throw new Exception("Node type " + generator.NodeInfo.Type.ToString() + " unimplamented");
            }
        }


        private void AddFeaturesRedSun(SceneGenerator generator)
        {
            generator.AmbiantColor = new Vector3(0.15f, 0.15f, 0.15f);
            generator.AddFeature(new CenterpieceFeature("SunWithBackground"));
            AddAsteroidFeature(generator.NodeInfo, generator);
        }
        
        private void AddFeaturesBinaryStar(SceneGenerator generator)
        {
            generator.AmbiantColor = new Vector3(0.2f, 0.15f, 0.15f);
            generator.AddFeature(new CenterpieceFeature( new BinaryStarFeature()));
            AddAsteroidFeature(generator.NodeInfo, generator);
        }

        private void AddFeaturesWhiteDwarf(SceneGenerator generator)
        {
            LightObject lightObj = new LightObject(new Vector3(1, 1, 0.8f), 1000000, 3);
            lightObj.Position = -Vector2.One * 50000;
            generator.lightObject = lightObj;
            generator.AddFeature(new CenterpieceFeature("Moon"));
            AddAsteroidFeature(generator.NodeInfo, generator);
        }

        private void AddFeaturesAsteroidField(SceneGenerator generator)
        {
            LightObject lightObj = new LightObject(new Vector3(0.2f, 0.2f, 0.2f), 1000000, 3);
            lightObj.Position = -Vector2.One * 50000;
            generator.lightObject = lightObj;

            generator.AmbiantColor = new Vector3(0.95f, 0.95f, 0.95f);
            generator.AddFeature(new CenterpieceFeature(new AsterodFieldFeature()));
            AddAsteroidFeature(generator.NodeInfo, generator);
            var lavaAstero = new AsteroidsFeature(1);
            lavaAstero.baseName = "LavaAsteroid";         
            lavaAstero.MaxNumberOfBatches = 15;
            lavaAstero.MinNumberOfBatches = 10;
            generator.AddFeature(lavaAstero);

            generator.AddFeature(new MinerBossFeature());
            AddAsteroidFeature(generator.NodeInfo, generator);

            generator.AddFeature(new GameProcessFeature(new FireflysProcess(5)));
            generator.AddFeature(new GameProcessFeature(new FireflysProcess(4)));
        }

        private void AddFeaturesNebula(SceneGenerator generator)
        {
            LightObject lightObj = new LightObject(new Vector3(0.8f, 0.8f, 1f), 1000000, 3);
            lightObj.Position = -Vector2.One * 50000;
            generator.lightObject = lightObj;

            generator.AmbiantColor = new Vector3(0.3f, 0.5f, 0.5f);
            generator.AddFeature(new CenterpieceFeature(new EmitterFeature(ContentBank.Get("NebulaEmitter1")) ));
            AddAsteroidFeature(generator.NodeInfo, generator); 
        }

        private void AddFeaturesVile(SceneGenerator generator)
        {
            generator.AmbiantColor = new Vector3(0.15f, 0, 0.15f);
            generator.AddFeature(new CenterpieceFeature("GreenSun"));
            AddAsteroidFeature(generator.NodeInfo, generator);
            var lavaAstero = new AsteroidsFeature(1);
            lavaAstero.baseName = "WormAsteroid";
            lavaAstero.MaxNumberOfBatches = 15;
            lavaAstero.MinNumberOfBatches = 10;
            generator.AddFeature(lavaAstero);
            generator.AddFeature(new GameProcessFeature(GameEventFactory.MakeBlobSpawner()));
            generator.AddFeature(new GameProcessFeature(GameEventFactory.MakeBlobSpawner()));

            var alter = FeatureFactory.MakeDemonAlterFeature();
            alter.Position = FMath.Rand.PointInRing(generator.SceneRadius * 0.6f, generator.SceneRadius * 0.75f); //Change postition
            generator.AddFeature(alter);
        }

        private void AddFeaturesBlueGiant(SceneGenerator generator)
        {
            generator.AddFeature(new CenterpieceFeature("Sun")); //fix
            AddAsteroidFeature(generator.NodeInfo, generator);
        }


        private void AddAsteroidFeature(NodeInfo info, SceneGenerator generator)
        {
            string baseName = "Asteroid";
            var _asteroiBasedFeature = new AsteroidsFeature(info.Level, 2);
            (_asteroiBasedFeature as AsteroidsFeature).baseName = baseName;
            var asteroidProfile2 = new AsteroidsFeature(info.Level, 2);

            AddSystemsEmitter deployableTurret1 = new AddSystemsEmitter();
            deployableTurret1.Emitter = ContentBank.Inst.GetEmitter("TurretA_Gen"); //TODO: can be group emitter with different options for defanders
                                                                                    //    BasicEmitterCaller lootSystem = new BasicEmitterCaller();
                                                                                    //   lootSystem.Emitter = ContentBank.Inst.GetEmitter("Loot1");
                                                                                    //   lootSystem.Activation = ControlSignals.OnDestroyed;
                                                                                    //    deployableTurret1.SystemsToAdd.Add(lootSystem);
            deployableTurret1.SystemsToAdd.Add(new SlotItemDropSystem(ControlSignals.OnDestroyed, 1, false));
            asteroidProfile2.DefenceEmitters.Add(deployableTurret1); //TODO:

            AddSystemsEmitter deployableTurret2 = new AddSystemsEmitter();
            deployableTurret2.Emitter = ContentBank.Inst.GetEmitter("TurretC_Gen"); //TODO: can be group emitter with different options for defanders            
                                                                                    //     
            deployableTurret2.SystemsToAdd.Add(new SlotItemDropSystem(ControlSignals.OnDestroyed, 1, false));

            asteroidProfile2.DefenceEmitters.Add(deployableTurret2);
            var _asteroidFeature = asteroidProfile2;

            generator.AddFeature(_asteroidFeature);
        }

    }
}

/*
   scene.GameEngine.AddGameProcces(GameEventFactory.MakeCargoShipSpawner());
            scene.GameEngine.AddGameProcces(GameEventFactory.MakeCargoShipSpawner());
            scene.GameEngine.AddGameProcces(GameEventFactory.MakePirateRaidSpawner());          
            scene.GameEngine.AddGameProcces(GameEventFactory.MakeBlobSpawner()); //TODO: add more events 
            if (NodeInfo.Type == Framework.World.MetaGame.NodeType.Vile)
            {
                var blobEvent = GameEventFactory.MakeBlobSpawner();
                blobEvent.ActivationCooldownTime = 60 * 60 ;
                scene.GameEngine.AddGameProcces(blobEvent); //TODO: add more events 
            }
            if(NodeInfo.Type == Framework.World.MetaGame.NodeType.AsteroidField)
            {
                scene.GameEngine.AddGameProcces(new FireflysProcess(5));
                scene.GameEngine.AddGameProcces(new FireflysProcess(4));
            }


    */

using Microsoft.Xna.Framework;
using SolarConflict.Framework.Emitters;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.World.Generation;
using SolarConflict.Framework.World.Generation.Profiles;
using SolarConflict.GameContent.NewItems;
using SolarConflict.GameWorld;
using SolarConflict.NodeGeneration.Features;
using SolarConflict.NodeGeneration.Features.Missions;
using SolarConflict.Session.World.Generation.Features;
using SolarConflict.Session.World.Generation.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Session.World.Generation.Content
{
    class FirstNodeGenerationInfo
    {
        public static SceneGenerator Make() {
            NodeInfo nodeInfo = new NodeInfo();
            nodeInfo.RandomSeed = 324234;
            nodeInfo.Type = Framework.World.MetaGame.NodeType.RedSun;                        
            nodeInfo.ControllingFaction = Framework.FactionType.Federation;
            nodeInfo.Level = 0;
            SceneGenerator generator = new SceneGenerator(nodeInfo);            
            generator.PlayerStartingPoint = Vector2.UnitY * generator.SceneRadius * 1.1f;
            generator.AddFeature(new CenterpieceFeature("Sun"));
           // generator.AddFeature(FeatureFactory.MakePlanetsFature());
            //generator.startingPosMult = 0.7f;

            AsteroidsFeature asteroidProfile1 = new AsteroidsFeature(5741, 0);
            asteroidProfile1.SetLevel(0);
            asteroidProfile1.DefenceNumberMin = 1;
            asteroidProfile1.DefenceNumberRange = 2;
            asteroidProfile1.DefenceEmittersProb = 1;
            GroupEmitter groupmitter = new GroupEmitter();
            groupmitter.EmitType = GroupEmitter.EmitterType.RandomOne;            
            groupmitter.AddEmitter("TurretC_Gen");
            groupmitter.AddEmitter("TurretA_Gen");
            groupmitter.AddEmitter("TurretA_Gen");

            asteroidProfile1.DefenceEmitters.Add(groupmitter);
            
            asteroidProfile1.MinNumberOfBatches = 20;
            
            asteroidProfile1.baseName = "Asteroid";
            asteroidProfile1.StartingLevel = 1;
            asteroidProfile1.NumberOfLevels = 2;
            asteroidProfile1.clusterMinSize = 1500;
            asteroidProfile1.clusterMaxSize = 2500;
            asteroidProfile1.DefenceEmittersProb = 0.45f;
            //asteroidProfile1.MaxNumberOfBatches =
            generator.AddFeature(asteroidProfile1);

            var guardedMine = new ResourceMineFeature();
            guardedMine.SetLevel(1);
            guardedMine.Position = generator.PlayerStartingPoint - new Vector2(6000f);
            generator.AddFeature(guardedMine);

            LayersProfile mines = new LayersProfile(true);
            mines.BaseName = "ResourceMine";
            mines.startingLevel = 1;
            mines.numberOfLevels = 1;
            mines.MaxNumberOfBatches = 5;
            mines.MinNumberOfBatches = 5;
            generator.AddFeature(mines);

            LayersProfile creates = new LayersProfile(false);
            creates.BaseName = "CrateA";
            creates.startingLevel = 1;
            creates.numberOfLevels = 1;
            creates.MaxNumberOfBatches = 15;
            generator.AddFeature(creates);

           // ShipyardFeature shipyard = new ShipyardFeature();
          //  shipyard.Position = generator.PlayerStartingPoint + Vector2.One * 6000;//  Vector2.UnitY * 20000;
            //PirateLord,PirateKing,Matador,Mirage,Vile,Devestator,CargoShip1C,HelperGun,Skill
         //   shipyard.ships.Add("ShipyardShip1");
         //   shipyard.ships.Add("ShipyardShip2");
         //   shipyard.ships.Add("Skill");
           // shipyard.ships.Add("CargoShip1C");
           // generator.AddFeature(shipyard);

            FactionsFeature ff = new FactionsFeature(Framework.FactionType.Empire);
            ff.HasRoads = true;
            generator.AddFeature(ff);

            MothershipFeature ms = new MothershipFeature("FederationBase", Framework.FactionType.Federation);
            ms.ID = "FederationBase";
            ms.FleetCommand = FleetCommandType.MothershipGoal;
            ms.Position = Vector2.One *generator.SceneRadius * 0.5f;        
            ms.AggroRange = generator.SceneRadius *2;            
            generator.AddFeature(ms);
            
            ms = new MothershipFeature("PirateBase", Framework.FactionType.Pirates1);
            ms.ID = "PirateBase";            
            ms.FleetCommand = FleetCommandType.MothershipGoal;
            ms.Position = new Vector2(-1.5f,0.8f) * generator.SceneRadius * 0.5f;
            ms.AggroRange = generator.SceneRadius * 2;
            generator.AddFeature(ms);

            StarportFeature sp = new StarportFeature(false);
            generator.AddFeature(sp);
            sp.Position = generator.PlayerStartingPoint - new Vector2(9000, 1000);

            WarpInhibitorFeature warpInhib = new WarpInhibitorFeature(false);
            warpInhib.SetLevel(0);
            generator.AddFeature(warpInhib);


            //DestroyTheBaseFeature destroyBase = new DestroyTheBaseFeature();
            //destroyBase.SetFaction(Framework.FactionType.Pirates1);
            //generator.AddFeature(destroyBase);
            //destroyBase.Position = new Vector2(-7000, generator.SceneRadius + 25000);

            return generator;
        }
    }
}

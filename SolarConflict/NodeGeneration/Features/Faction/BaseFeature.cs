using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Session.World.Generation.Features;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Session.World.Generation.Profiles
{
    //Control menuver by, 
    //What is active and what is not 
    //AI hard,  
    // Make , remove sun , more meanveblity 

    //TODO: add emitter that adds systems to agents
    //TODO: add sleep system 
    //TODO: ??add emitter that emitts acroding to level

    /// <summary>
    /// Emmits a base level:...
    /// </summary>
    class BaseFeature : GenerationFeature
    {
        public Vector2 BasePosition { get; set; }
        public IEmitter BaseEmitter;
        public IEmitter DefenceTurretEmitter;
        public float TurretRad { get; set; }
        public int TurretNumber;
        public int TurretNumberRange = 0;

        public List<IEmitter> Arenas { get; private set; }

        private ParamEmitter _turrentDefencePositionEmitter;

        public BaseFeature(string baseEmitter, FactionType factionType) : base()
        {
            localFaction = factionType;
            _turrentDefencePositionEmitter = new ParamEmitter();
            _turrentDefencePositionEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            _turrentDefencePositionEmitter.PosAngleRange = 360;
            TurretRad = 0;
            TurretNumber = 3;
            BaseEmitter = ContentBank.Inst.GetEmitter(baseEmitter);
            var shipyardFeature = new ShipyardFeature();
            shipyardFeature.Position = Vector2.One * 2000;
            AddChild(shipyardFeature);
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            float rad = FMath.TransformToRadius(Rand.NextFloat(), generator.SceneRadius, generator.SunRadius);
            BasePosition = FMath.ToCartesian(rad, Rand.NextFloat() * MathHelper.TwoPi);

            BasePosition = generator.PlayerStartingPoint + FMath.ToCartesian(5000, Rand.NextFloat() * MathHelper.TwoPi);
            Agent baseGameObject = BaseEmitter.Emit(scene.GameEngine, null, Faction, BasePosition, Vector2.Zero, 0) as Agent;
            baseGameObject.Name = GenerateBaseName(scene);
            FleetSystem fleetSystem = new FleetSystem();
            fleetSystem.AddAgentSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed, 0.9f, false));
            fleetSystem.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));
            fleetSystem.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));
            fleetSystem.FleetSlots.Add(new FleetSystem.FleetSlot(SizeType.Large));

            var generationData = MetaWorld.Inst.GetFaction(Faction).GenerationData;
            if (generationData != null)
            {
                var agentGenerators = generationData.GetAgentGenerators();
                for (int i = 0; i < Math.Min(agentGenerators.Count, fleetSystem.FleetSlots.Count); i++)
                {
                    Agent agent = (agentGenerators[i] as IEmitter).Emit(scene.GameEngine, baseGameObject, Faction, baseGameObject.Position, Vector2.Zero, 0, param: Level) as Agent;
                    fleetSystem.AddShipCopyToSlot(0, agent);
                }
            }



            baseGameObject.AddSystem(fleetSystem);
            if (DefenceTurretEmitter != null)
            {
                _turrentDefencePositionEmitter.Emitter = DefenceTurretEmitter;
                _turrentDefencePositionEmitter.MinNumberOfGameObjects = TurretNumber;
                _turrentDefencePositionEmitter.RangeNumberOfGameObject = TurretNumberRange;
                _turrentDefencePositionEmitter.PosRadMin = TurretRad == 0 ? baseGameObject.Size * ((TurretNumber + TurretNumberRange) * 0.5f + 2) : TurretRad;
                _turrentDefencePositionEmitter.Emit(scene.GameEngine, baseGameObject, Faction, baseGameObject.Position, Vector2.Zero, Rand.NextFloat() * MathHelper.TwoPi);
            }

            return baseGameObject;
        }

        private string GenerateBaseName(Scene scene)
        {
            Faction faction = scene.GameEngine.GetFaction(Faction);
            return faction.ToTag(faction.Name + " Base");
        }

    }
}


//var expectedNumBasesForControllingFaction = 4; // typical number of bases for the current faction
//for (int i = 0; i < info.FactionsByStrength.Count; ++i) {
//    var faction = info.FactionsByStrength[i];

//    var numBases = expectedNumBasesForControllingFaction - i;
//    // ^ Each faction is expected to have one base more than the next, in descending order of strength
//    if (generator.Rand.NextFloat() < 0.2f)
//        // Chance of faction having one base less than expected
//        --numBases;

//    if (numBases <= 0)
//        // Nothing to do here
//        continue;

//    var level = info.Level;
//    if (generator.Rand.NextFloat() > Math.Pow(0.85f, i))
//        // Each faction has a progressively greater chance of being underleveled by one tier
//        --level;

//    //var factionFeature = new FactionsFeature(faction);
//    //factionFeature.HasRoads = i == 0; // only controlling faction gets roads
//    //factionFeature.SetLevel(level);
//    //factionFeature.Init(numBases);
//    //generator.AddFeature(factionFeature);
//    //generator.AddFeature(new ShopFeature(faction, level, 10));
//}
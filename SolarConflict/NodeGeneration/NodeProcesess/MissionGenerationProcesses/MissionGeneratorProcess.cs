using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration.NodeProcesess
{
    /// <summary>
    /// Generates missions per game object: 
    /// Mission types are: 
    /// * Fatch (find items), 
    /// * Destroy a target (other faction base), 
    /// * Defend from waves/attack
    /// * Deliver an generated item (that decays) to other allied base/starport
    /// Rewards:
    ///   * Faction reputation - proprtional to level and diffaclty
    ///   * Items from faction
    /// 
    /// </summary>
    [Serializable]
    class MissionGeneratorProcess : GameProcess, IEmitter
    {
        public string ID { get; set; }

        public MissionGeneratorProcess(FactionType missionGiverFaction)
        {
            
        }

        public override void Update(GameEngine gameEngine)
        {
            throw new NotImplementedException();
        }

        public override GameProcess GetWorkingCopy()
        {
            throw new NotImplementedException();
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            throw new NotImplementedException();
        }
    }
}
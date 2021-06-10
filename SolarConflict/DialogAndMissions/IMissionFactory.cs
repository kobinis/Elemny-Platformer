using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    /// <summary>
    /// Interface for creating MissionGenerators/missions
    /// </summary>
    public interface IMissionFactory
    {
        /// <summary>
        /// Adds new missions generated missions
        /// </summary>
        /// <param name="newMissionsList">The list to add the missions to</param>
        /// <returns>The number of new missions added</returns>
        int CrateMissionGenerator(Scene scene, Agent agent, List<GameObject> targetList, List<IMissionGenerator> newMissionsList, int numOfMissionsToAdd = 0);
        int NumOfMissionsToGenerate { get; set; }
    }
}
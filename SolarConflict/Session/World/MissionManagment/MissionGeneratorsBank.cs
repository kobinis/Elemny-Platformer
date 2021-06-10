using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment
{
    [Serializable]
    public class MissionGeneratorsBank
    {
        private List<IMissionGenerator> missionGenerators;//TODO: change

        public MissionGeneratorsBank()
        {
            missionGenerators = new List<IMissionGenerator>();
        }

        public bool Add(IMissionGenerator  generator)
        {
            missionGenerators.Add(generator);
            return true;
        }        

        public void Remove(IMissionGenerator generator)
        {
            missionGenerators.Remove(generator);
        }

        public List<IMissionGenerator> GetGenerators(Scene scene, FactionType faction = FactionType.None, int level = -1)
        {
            List<IMissionGenerator> generatorsList = new List<IMissionGenerator>(missionGenerators.Count);

            // Return all valid generators of the appropriate faction and level
            return missionGenerators
                .Where(g => (faction == FactionType.None || faction == g.Faction) && (level == -1 || level == g.Level) && g.CheckIfValid(scene))
                .ToList();            
        }

    }
}

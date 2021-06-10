using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.InGameEvent;
using SolarConflict.Framework.InGameEvent.Activations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.Spawners
{
    class PlayerLostSpawner
    {
        public static InGameEventSpawner Make()
        {
            var spawner = new SpawnAgents();
            spawner.Faction = FactionType.TradingGuild;
            spawner.IsOneTime = false;
            spawner.Target = SpawnAgents.AgentTargetType.Player;
            var activation = new DistanceFromPoint();
            activation.Distance = 10000;
            activation.Closer = false;
            spawner.ActivationCheck = activation;
            spawner.ActivationCooldownTime = 60 * 10;                        
            spawner.ActivationProbability = 1f;
            spawner.GoalPositionRad = 200000;
            spawner.SpawnPositionRad = 10000;
            spawner.EmitterList.Add(ContentBank.Inst.GetEmitter("GuildSalesman"));
            return spawner;
        }
    }
}

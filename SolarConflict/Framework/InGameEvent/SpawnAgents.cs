using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.InGameEvent
{
    [Serializable]
    public class SpawnAgents : InGameEventSpawner, IEmitter
    {
        public enum AgentTargetType { PassPlayer, Player, PlayerMothership}

        public List<IEmitter> EmitterList;
        public List<AgentSystem> SystemsToAdd;
        public IEmitter Emitter;                        

        public float SpawnPositionRad; //Change
        public float GoalPositionRad; //change
        public AgentTargetType Target;                
        public FactionType Faction;
        public string Text;
        public int LevelOffset;

        public SpawnAgents()
        {
            Target = AgentTargetType.PassPlayer;
            Faction = FactionType.Neutral;
            EmitterList = new List<IEmitter>(); 
            SystemsToAdd = new List<AgentSystem>();
            //Temp:
            var agentDespawner = new AgentDespawner();
            agentDespawner.MaxLifetime = 60 * 60 * 2;
            agentDespawner.MustBeOffScreen = true;
            agentDespawner.Effect = ContentBank.Inst.GetEmitter(Consts.WARPOUT_EFFECT);
            SystemsToAdd.Add(agentDespawner);
            Emitter = ContentBank.Inst.GetEmitter(Consts.WARPIN_EFFECT);
        }

        public override GameProcess MakeEvent(GameEngine gameEngine)
        {
            Agent player = gameEngine.PlayerAgent;
            Vector2 centerPosition = FindPlaceOutOfScreen(player, gameEngine);            
            var target = FindTargetPosition(player, gameEngine);
            Emitter.Emit(gameEngine, null, Faction, centerPosition, Vector2.Zero, 0, param: Math.Max(gameEngine.Level + LevelOffset,0));
            // ^ note that we pass the current node's level via "param"
            foreach (var emitter in EmitterList)
            {
                Vector2 position = centerPosition + FMath.ToCartesian(200, gameEngine.Rand.NextFloat() * MathHelper.TwoPi);
                GameObject gameObject = emitter.Emit(gameEngine, null, Faction, position, Vector2.Zero, gameEngine.Rand.NextFloat() * MathHelper.TwoPi,
                    param: Math.Max(gameEngine.Level + LevelOffset, 0)) as Agent;
                if (gameObject != null) {
                    gameObject.SetTarget(target, TargetType.Goal);
                    if (gameObject is Agent) //Can change
                    {
                        Agent agent = gameObject as Agent;
                        foreach (var system in SystemsToAdd) {
                            AgentSystem systemClone = system.GetWorkingCopy();
                            agent.AddSystem(systemClone);
                        }
                    }
                }
            }

            //TODO: Add textbox if text != null
            //ActivityManager.Inst.AddToast("Text", 60);
            return null;
        }

        private Vector2 FixedVelocity(Vector2 velocity, Random random)
        {
            return velocity == Vector2.Zero ? FMath.ToCartesian(1, random.NextFloat() * MathHelper.TwoPi) : velocity.Normalized();
        }        

        private Vector2 FindPlaceOutOfScreen(Agent player, GameEngine gameEngine)
        {
            Vector2 velocitiy = FixedVelocity(player?.Velocity ?? Vector2.Zero, gameEngine.Rand);
            return gameEngine.Camera.Position  + velocitiy * SpawnPositionRad; 
        }

        private GameObject FindTargetPosition(Agent player, GameEngine gameEngine)
        {
            switch (Target)
            {
                case AgentTargetType.PassPlayer:
                    var dummyTarget = new DummyObject();
                    Vector2 velocitiy = FixedVelocity(player?.Velocity ?? Vector2.Zero, gameEngine.Rand);                    
                    dummyTarget.Position = gameEngine.Camera.Position - velocitiy * GoalPositionRad;
                    return dummyTarget;                    
                case AgentTargetType.Player:
                    return gameEngine.Scene.PlayerAgent;                    
                case AgentTargetType.PlayerMothership:
                    return gameEngine.Scene.GetPlayerFaction().Mothership;                    
                default:
                    break;
            }
            return gameEngine.Scene.PlayerAgent;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            SpawnAgents spwanner = GetWorkingCopy() as SpawnAgents;
            spwanner.Faction = faction;
            gameEngine.AddGameProcces(spwanner);
            return null;
        }

        public override GameProcess GetWorkingCopy()
        {
            var result = MemberwiseClone() as SpawnAgents;
            if(ActivationCheck != null)
                result.ActivationCheck = ActivationCheck.GetWorkingCopy();
            return result;
        }

    }
}

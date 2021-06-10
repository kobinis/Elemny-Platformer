using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.InGameEvent.GenericProcess
{
    [Serializable]
    public class WaveProcces : GameProcess
    {
        public IMissionGenerator missionGenerator;
        private Random rand;
        private int waveLevel = 1;
        private List<string> loadouts = new List<string> { "GustavFleet1", "GustavFleet2", "GustavFleet3"};
        private Dictionary<int, List<IEmitter>> emittersPerSize;
        private GroupEmitter loot;
        private int lifetime;
        private List<GameObject> enemys;

        private Mission mission;

        public WaveProcces(Mission missionGenerator)
        {
            rand = new Random();
            enemys = new List<GameObject>();
            loot = new GroupEmitter();
            this.missionGenerator = missionGenerator;
            loot.EmitType = GroupEmitter.EmitterType.RandomOne;
            loot.AddEmitter("RepairKit3", 1f);
            loot.AddEmitter("EnergyKit3", 2f);
            lifetime = 0;
            InitObject();
        }

        public override void Update(GameEngine gameEngine)
        {
            if (lifetime == 0)
            {
                Init(gameEngine);
            }
            bool newWave = true;
            foreach (var enemy in enemys)
            {
                if (enemy.IsActive)
                {
                    newWave = false;
                    break;
                }
            }
            if (newWave && gameEngine.PlayerAgent != null && gameEngine.PlayerAgent.IsActive)
            {
                newWave = false;
                //gameEngine.PlayerAgent.AddMeterValue(MeterType.Shield, 10000);            
                InitWave(waveLevel, gameEngine);
                waveLevel++;
                if (mission != null && mission.IsFinished)
                {
                    Finished = true;
                }
            }
            lifetime++;
        }

        public override GameProcess GetWorkingCopy()
        {
            throw new NotImplementedException();
        }

        public void InitWave(int level, GameEngine gameEngine)
        {
            enemys.Clear();
            int[] shipCountPerSize = GetShipCountPerSize(level);
            for (int shipSize = 0; shipSize < shipCountPerSize.Length; shipSize++)
            {
                int shipCount = shipCountPerSize[shipSize];
                var agentFactorys = GetAgentGeneratorsBySize(shipSize);
                if (agentFactorys.Count > 0)
                {
                    int offset = rand.Next(agentFactorys.Count);
                    for (int i = 0; i < shipCount; i++)
                    {
                        Vector2 pos = gameEngine.PlayerAgent.Position + FMath.ToCartesian(6000, rand.NextFloat() * MathHelper.TwoPi);
                        float rotation = rand.NextFloat() * MathHelper.TwoPi;
                        int factoryIndex = (i + offset) % agentFactorys.Count;
                        var agent = agentFactorys[factoryIndex].Emit(gameEngine, null, FactionType.Federation, pos, Vector2.Zero, rotation, param: rand.Next(waveLevel / 3) + 1) as Agent;
                        //SlotItemDropSystem dropSystem = new SlotItemDropSystem(ControlSignals.OnDestroyed);
                        //agent.AddSystem(dropSystem); //
                        agent.SetAggroRange(12000, 50000, TargetType.Enemy);
                        agent.SetTarget(gameEngine.PlayerAgent, TargetType.Enemy);

                        BasicEmitterCallerSystem lootSystem = new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, loot);
                        agent.AddSystem(lootSystem);
                        enemys.Add(agent);
                    }
                }
            }
        }

        private void Init(GameEngine gameEngine)
        {
            if (missionGenerator != null)
            {
                mission = missionGenerator.GenerateMission();
                gameEngine.Scene.AddMission(mission);
            }
        }



        private List<IEmitter> GetAgentGeneratorsBySize(int size)
        {
            if (emittersPerSize.ContainsKey(size))
            {
                return emittersPerSize[size];
            }
            return new List<IEmitter>();
        }

        private int[] GetShipCountPerSize(int level)
        {
            Dictionary<int, int[]> sizes = new Dictionary<int, int[]>
            {
                { 0, new int[] { 5,0,0 } },
                { 1, new int[] { 10,0,0 } },
                { 2, new int[] { 10,3,0 } },
                { 3, new int[] { 15,0,0 } },
                { 4, new int[] { 15,3,0 } },
                { 5, new int[] { 20,3,0 } },
                { 6, new int[] { 20,0,0 } },
                { 7, new int[] { 5,4,1 } },
            };
            if (sizes.ContainsKey(level))
                return sizes[level];

            return new int[] { 20, 1, 0 };
        }

        private void InitObject() //Change it
        {
            emittersPerSize = new Dictionary<int, List<IEmitter>>();
            foreach (string loadoudID in loadouts)
            {

                EquippedAgentGenerator agentGenerator = ContentBank.Inst.GetEmitter(loadoudID) as EquippedAgentGenerator;
                int size = 0;
                if (agentGenerator != null)
                { size = (int)agentGenerator.SizeType; }
                else
                {
                    var loadout = ContentBank.Inst.GetEmitter(loadoudID) as AgentLoadout;
                    size = (int)loadout.Agent.SizeType;
                }

                if (!emittersPerSize.ContainsKey(size))
                    emittersPerSize.Add(size, new List<IEmitter>());
                emittersPerSize[size].Add(ContentBank.Inst.GetEmitter(loadoudID));
            }
        }
         
    }
}

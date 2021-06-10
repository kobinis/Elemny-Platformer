using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Utils;
using XnaUtils.Graphics;
using System.Diagnostics;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc
{

    public enum FleetCommandType { None, FlagshipTargetAndFlagship, Flagship, MothershipEnemy, MothershipGoal, Mothership, Patrol, Group } //Mothership target to enemy

    /// <summary>
    /// FleetSystem is used to re-spawn ships
    /// </summary>
    [Serializable]
    public class FleetSystem : AgentSystem //Add Warp effect
    {
        public const int MinRebuildTime = 1 * Utility.FramesPerSecond; //


        [Serializable]
        public class FleetSlot
        {
            public Agent Agent;
            public SizeType MaxSizeClass;
            public bool IsRebuild;
            public int Cooldown;
            public FleetCommandType Command;
            public FleetCommandType SecondaryCommand;
            public FleetCommandType GetCommand()
            {
                if (Command != FleetCommandType.None)
                    return Command;
                return SecondaryCommand;
            }            

            public FleetSlot(SizeType maxSizeClass, IGameObjectFactory agentFactory = null)
            {
                if (agentFactory != null)
                {
                    Agent agent = agentFactory.MakeGameObject(null) as Agent;
                    agent.IsActive = false;
                    Cooldown = GetBuildTime(agent);
                }
                MaxSizeClass = maxSizeClass;
                IsRebuild = true;
                Command = FleetCommandType.None;
                Cooldown = -1;
            }

            public FleetSlot GetWorkingCopy()
            {
                FleetSlot copy = (FleetSlot)MemberwiseClone();
                copy.Agent = null;
                Cooldown = 0;
                return copy;
            }

            public string GetTooltip()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Main Command: " + Color.Yellow.ToTag(Command.ToString()));

                //sb.AppendLine("Secondary Command:" + SecondaryCommand.ToString());
                return sb.ToString();
            }

            public void UpdateTargetByCommand(int index, GameEngine gameEngine, FleetSystem fleetSystem, Agent mothership, Agent flagship, bool isForceTarget = true)
            {
                if (Agent != null && Agent.IsActive)
                {
                    GameObject goal = null;
                    GameObject enemy = null;
                    switch (GetCommand())
                    {
                        case FleetCommandType.FlagshipTargetAndFlagship:
                            if (flagship != null && flagship.IsActive)
                            {
                                goal = flagship;
                                enemy = flagship.GetTarget(gameEngine, TargetType.Enemy);
                            }
                            break;
                        case FleetCommandType.Flagship:
                            goal = flagship;
                            break;
                        case FleetCommandType.MothershipEnemy:
                            goal = mothership.GetTarget(gameEngine, TargetType.Enemy);
                            enemy = goal;
                            break;
                        case FleetCommandType.MothershipGoal:
                            goal = mothership.GetTarget(gameEngine, TargetType.Goal);
                            enemy = goal;
                            break;
                        case FleetCommandType.Mothership:
                            goal = mothership;
                            break;
                        case FleetCommandType.Patrol: //Change
                            int start = gameEngine.Rand.Next(fleetSystem.FleetSlots.Count);
                            
                            for (int i = 1; i < fleetSystem.FleetSlots.Count; i++)
                            {
                                if (gameEngine.FrameCounter % 500 == 0)
                                {
                                    int ind = (index + i + start) % fleetSystem.FleetSlots.Count;
                                    goal = fleetSystem.FleetSlots[ind].Agent;
                                    if (goal != null && goal.IsActive && ind != index)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                        case FleetCommandType.Group:
                            for (int i = 1; i < fleetSystem.FleetSlots.Count; i++)
                            {
                                int ind = (index + i) % fleetSystem.FleetSlots.Count;
                                goal = fleetSystem.FleetSlots[ind].Agent;
                                if (goal != null && goal.IsActive)
                                {
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if ((isForceTarget || Agent.GetTarget(gameEngine, TargetType.Goal) == null) && goal != null && goal != Agent && goal.IsActive)
                        Agent.SetTarget(goal, TargetType.Goal);
                    var faction = gameEngine.GetFaction(Agent.FactionType);

                    if (Agent.GetTarget(gameEngine, TargetType.Enemy) == null && enemy != null)
                        Agent.SetTarget(enemy, TargetType.Enemy);
                }
            }
        }


        public float RebuildCostMultiplier { get; private set; }
        public List<FleetSlot> FleetSlots { get; private set; }

        public List<AgentSystem> _factionSystems;

        //Fleet commands
        public Agent flagship;



        public FleetSystem()
        {
            FleetSlots = new List<FleetSlot>();
            RebuildCostMultiplier = 0.1f;
            _factionSystems = new List<AgentSystem>();
        }

        public void AddAgentSystem(AgentSystem system)
        {
            _factionSystems.Add(system);
        }

        public Agent AddShipCopyToSlot(int index, string factoryID)
        {
            var factory = ContentBank.Inst.GetGameObjectFactory(factoryID);
            Agent agent = factory.MakeGameObject(null) as Agent;
            Agent.EquipAgent(agent, 0, true);
            agent.IsActive = false;
            FleetSlots[index].Agent = agent;
            FleetSlots[index].Cooldown = -1;
            foreach (var item in _factionSystems)
            {
                agent.AddSystem(item.GetWorkingCopy());
            }
            return agent;
        }

        public Agent AddShipCopyToSlot(int index, IGameObjectFactory agentFactory)
        {
            Agent agent = agentFactory.MakeGameObject(null) as Agent;
            foreach (var item in _factionSystems)
            {
                agent.AddSystem(item.GetWorkingCopy());
            }
            agent.IsActive = false;
            FleetSlots[index].Agent = agent;
            FleetSlots[index].Cooldown = -1;
            return agent;
        }

        public bool AddShipToFreeSlot(Agent agent)
        {
            Debug.Assert(agent.IsActive);
            var slot = FindFreeSlot(agent.SizeType);
            if (slot != null)
            {
                slot.Agent = agent;
                foreach (var item in _factionSystems)
                {
                    agent.AddSystem(item.GetWorkingCopy());
                }
                return true;
            }
            return false;

        }

        public void SetSlotShip(int index, Agent agent)
        {
            Debug.Assert(agent.IsActive);
            FleetSlots[index].Agent = agent;
            foreach (var item in _factionSystems)
            {
                FleetSlots[index].Agent.AddSystem(item.GetWorkingCopy());
            }
        }

        public List<Agent> FleetShips => FleetSlots.Select(s => s.Agent).Where(a => a != null).Where(a => a.IsActive).ToList();

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            bool wasActive = false;
            var increment = MathHelper.TwoPi / FleetSlots.Count;
            var angle = 0f;
            flagship = null;
            foreach (var slot in FleetSlots)
            {
                angle += increment;

                if (slot.Agent != null && slot.Agent.IsNotActive)
                {
                    if (slot.Cooldown < 0)
                        slot.Cooldown = GetBuildTime(slot.Agent); //TODO: add cost    

                    //TODO: change that you can only build one ship if no money
                    var canPay = agent.GetFactionType() == FactionType.Player || gameEngine.GetFaction(agent.GetFactionType()).GetMeter(MeterType.Money).Value >= CalcRebuildCost(slot.Agent);
                    slot.Cooldown--;
                    if (slot.IsRebuild && slot.Cooldown <= 0 && canPay)
                    {
                        slot.Agent.Revive();
                        slot.Agent.SetFactionType(agent.GetFactionType());
                        slot.Agent.Position = agent.Position + FMath.ToCartesian(agent.Size + slot.Agent.Size, angle);
                        gameEngine.AddList.Add(slot.Agent);
                        slot.Cooldown = -1;
                        slot.Agent.SetControlType(AgentControlType.AI);
                        if (agent.GetFactionType() != FactionType.Player)
                            gameEngine.GetFaction(agent.GetFactionType()).GetMeter(MeterType.Money).AddValue(-CalcRebuildCost(slot.Agent));
                        wasActive = true;
                    }
                }

                if (slot.Agent != null && slot.Agent.IsActive) //TODO: change
                {
                    if (slot.Agent.GetControlType() == AgentControlType.Player || flagship == null)
                        flagship = slot.Agent;
                }

            }
            UpdateFleetLogic(gameEngine, agent);
            return wasActive;
        }

        private void UpdateFleetLogic(GameEngine gameEngine, Agent agent)
        {
            if (agent.Lifetime % 20 == 0) //Change flagship logic to be set
            {
                for (int i = 0; i < FleetSlots.Count; i++)
                {
                    FleetSlots[i].UpdateTargetByCommand(i, gameEngine, this, agent, flagship);
                }
            }
        }

        //public override void Draw(Camera camera, Agent agent, Vector2 initPosition, float initRotation, DrawType drawType = DrawType.Alpha)
        //{
        //    camera.CameraDraw(Sprite.Get("marker"), flagship.Position, 0, 1, Color.CadetBlue);
        //}

        //Removes Ship
        public static void RemoveAgent(FleetSystem.FleetSlot slot, GameEngine gameEngine)
        {
            if (slot.Agent != null)
            {
                var agent = slot.Agent;
                Faction faction = gameEngine.GetFaction(agent.FactionType);

                if (faction.Mothership != null)
                {
                    var mothershipInventory = faction.Mothership.Inventory;
                    if (agent.Inventory != null)
                    {
                        for (int i = 0; i < agent.Inventory.Size; i++)
                        {
                            if (!agent.Inventory.TryTransfer(mothershipInventory, i))
                            {
                                var item = agent.Inventory.Items[i];
                                item.Position = faction.Mothership.Position;
                                gameEngine.AddList.Add(item);
                                agent.Inventory.Items[i] = null;
                            }
                        }
                    }
                    if (agent.ItemSlotsContainer != null)
                    {
                        for (int i = 0; i < agent.ItemSlotsContainer.Count; i++)
                        {
                            Item item = agent.ItemSlotsContainer[i].Item;
                            if (item != null && item.Level > 0)
                            {
                                if (!mothershipInventory.AddItem(item))
                                {
                                    item.Position = faction.Mothership.Position;
                                    gameEngine.AddList.Add(item);
                                }
                                agent.ItemSlotsContainer[i].Item = null;
                            }
                            //mothershipInventory
                        }
                    }
                }
                gameEngine.RemoveGameObject(slot.Agent);
            }
            slot.Agent = null;
        }

        public int? FindFreeSlotIndex(SizeType sizeNeeded = SizeType.Small)
        {
            for (int i = 0; i < FleetSlots.Count; i++)
            {
                var slot = FleetSlots[i];
                if (slot.Agent == null && slot.MaxSizeClass >= sizeNeeded)
                    return i;
            }
            return null;
        }

        public FleetSlot FindFreeSlot(SizeType sizeNeeded = SizeType.Small)
        {
            foreach (var slot in FleetSlots)
            {
                if (slot.Agent == null && slot.MaxSizeClass >= sizeNeeded)
                    return slot;
            }
            return null;
        }

        public int CalcRebuildCost(Agent agent)
        {
            return (int)(agent.GetCost() * RebuildCostMultiplier);
        }

        private static int GetBuildTime(Agent agent)
        {
            if (agent.GetFactionType() == FactionType.Player)
                return Math.Max(MinRebuildTime, (int)Math.Min(agent.GetCost() * 0.02f, 6 * 60)); //TODO: think about
            return Math.Max(MinRebuildTime * 10, (int)Math.Min(agent.GetCost() * 0.1f, 60 * 60)); //TODO: think about
        }

        public void SetCommand(FleetCommandType command)
        {
            foreach (var slot in FleetSlots)
            {
                slot.Command = command;
            }
        }

        public void SetTarget(GameObject target, TargetType type)
        {
            foreach (var slot in FleetSlots)
            {
                slot.Agent?.SetTarget(target, type);
            }
        }


        public override AgentSystem GetWorkingCopy()
        {
            FleetSystem clone = (FleetSystem)MemberwiseClone();
            clone.FleetSlots = new List<FleetSlot>();
            foreach (var slot in FleetSlots)
            {
                clone.FleetSlots.Add(slot.GetWorkingCopy());
            }
            return clone;
        }

    }
}

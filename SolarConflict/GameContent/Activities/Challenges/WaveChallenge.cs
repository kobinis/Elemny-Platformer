using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.InGameEvent.Content;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using SolarConflict.Generation;
using SolarConflict.Session.World.Generation.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.Activities.Challenges
{           
    public class WaveChallenge:Scene
    {
        private Random rand;
        private int waveLevel = 1;
        private List<string> loadouts = new List<string> { "Empire1","Empire2", "Empire3","Empire4", "Federation1", "Federation2", };

        private Dictionary<int, List<IEmitter>> emittersPerSize;
        Agent shop;
        private int topScore;
        private const string WAVESCORE = "wavescore";
        public WaveChallenge():base(null, false)
        {

        }

        public override void InitScript(string parameters = null, ActivityParameters activityParameters = null)
        {
           // this.AddObjectRandomlyInLocalCircle("LavaAsteroid1", 10, 6000, player.Position, 500);
            AddGameObject("Sun", -Vector2.One *100000);
            GameEngine.PermanentLights.AddRange(GameEngine.AddList);
            GameEngine.AddList.Clear();

            HudManager.AddComponent(new TargetIndicator());
            HudManager.AddComponent(new AnnouncerEffects());
            SceneComponentSelector.AddComponent(Framework.Scenes.SceneComponentType.Inventory);
            rand = new Random();
            InitEmitters();
            Agent player = AddGameObject("PirateShip4", FactionType.Player, Vector2.Zero,0 , AgentControlType.Player, 2) as Agent;          
            shop = AddGameObject("SmallShop1", FactionType.Neutral, Vector2.UnitX * 1000, 90) as Agent;            
            shop.collideWithMask = GameObjectType.None;            
            for (int i = 1; i < 7; i++)
            {
                this.AddObjectRandomlyInCircle("SmallAsteroid"+i, 100, 10000 + i * 3000, 3000 + i * 3000);
            }
           // topScore = (int)SettingsManager.Inst.Preferences.GetFloat(WAVESCORE, 0);
        }

        bool waveHasStarted = false;
        public override void UpdateScript(InputState inpuState)
        {
            if(GameEngine.FrameCounter % 30 == 0 &&  !this.PauseGameEngine && GameEngine.GetSoleFaction() != FactionType.None && PlayerAgent != null && PlayerAgent.IsActive)
            {                
                PlayerAgent.AddMeterValue(MeterType.Shield, 10000);
                PlayerAgent.AddMeterValue(MeterType.Hitpoints, 10000);
                PlayerAgent.Velocity = Vector2.Zero;
                if(GameObject.DistanceFromEdge(shop, PlayerAgent) > 1000)
                    ContentBank.Get("TeleportWithEffect").Emit(GameEngine, PlayerAgent, FactionType.Player, Vector2.Zero, Vector2.Zero, 0);
                ShopSystem shopSystem = new ShopSystem();
                ItemCategory category = ItemCategory.Gun | ItemCategory.Shield | ItemCategory.EnergyConsumingWeapon;
                ShopFeature.GenerateShopItems(shopSystem, category, ItemCategory.AmmoWeapon, Math.Min( waveLevel, 10) , 10, ContentBank.Inst.GetAllItems(), FMath.Rand);
                for (int i = Math.Min(waveLevel, 10); i > 3; i--)
                {
                    ShopFeature.GenerateShopItems(shopSystem,ItemCategory.Utility | ItemCategory.Gun | ItemCategory.EnergyConsumingWeapon | ItemCategory.Shield,ItemCategory.Material | ItemCategory.AmmoWeapon,  i, 4, ContentBank.Inst.GetAllItems(), FMath.Rand);
                }
                shopSystem.AddItem("MatA1");
                shopSystem.AddItem("MatB1");
                shopSystem.AddItem("InhibitorCore");
                shop.InteractionSystem = shopSystem;
                waveHasStarted = true;
            }
            if (waveHasStarted && GameEngine.FrameCounter % 30 == 29)
            {
                waveHasStarted = false;
                DialogManager.AddDialog(new Dialog("Wave " + waveLevel + "\nPress #color{255,255,0}Space#dcolor{} to start", "Ai_helper"));
                InitWave(waveLevel);
                waveLevel++;
            }
            int kills = (int)GetPlayerFaction().GetMeter(MeterType.FactionKills).Value;
            if (kills > topScore)
                topScore = kills;
            GameEngine.Text = "Kills: " + kills + "  -  High Score: " + topScore;
        }

        public override ActivityParameters OnBack()
        {
           // SettingsManager.Inst.Preferences.SetFloat(WAVESCORE, topScore);
           // SettingsManager.Inst.Save();
            return base.OnBack();
        }
      

        public void InitWave(int level)
        {
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
                        Vector2 pos = FMath.ToCartesian(7000, rand.NextFloat() * MathHelper.TwoPi);
                        float rotation = rand.NextFloat() * MathHelper.TwoPi;
                        int factoryIndex = (i + offset) % agentFactorys.Count;
                        var agent = agentFactorys[factoryIndex].Emit(GameEngine, null, FactionType.Pirates1, pos , Vector2.Zero, rotation, param: Math.Min( Math.Max(level / 2 - rand.Next(2), 0),10) ) as Agent;
                        agent.targetSelector.SetAggroRange(100000, 200000, TargetType.Enemy);
                        if (level > 4)
                        {
                            SlotItemDropSystem dropSystem = new SlotItemDropSystem(ControlSignals.OnDestroyed, 0.3f);
                            dropSystem.AlwaysDrop = false;
                            agent.AddSystem(dropSystem);
                        }
                        
                        LootEmitter loot = new LootEmitter();
                        loot.AddEmitter("TurretALoot",1);
                        if(level < 4)
                            loot.AddEmitter("RepairKit1",0.5f);
                        
                        if (level > 4)
                        {
                            loot.AddEmitter("RepairKit3", 0.5f);
                            loot.AddEmitter("RepairKit6", 0.1f);
                        }
                        
                        BasicEmitterCallerSystem lootSystem = new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, loot);
                        agent.AddSystem(lootSystem);
                    }
                }
            }
        }

        

        private List<IEmitter> GetAgentGeneratorsBySize(int size)
        {
            if(emittersPerSize.ContainsKey(size))
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
                { 1, new int[] { 6,0,0 } },
                { 2, new int[] { 5,1,0 } },
                { 3, new int[] { 7,1,0 } },
                { 4, new int[] { 5,2,0 } },
                { 5, new int[] { 5,2,0 } },
                { 6, new int[] { 5,2,0 } },
                { 7, new int[] { 5,2,1 } },
            };
            if (sizes.ContainsKey(level))
                return sizes[level];

            return new int[] { 6, 2, 1 };
        }

        private void InitEmitters()
        {
            emittersPerSize = new Dictionary<int, List<IEmitter>>();
            foreach (string loadoudID in loadouts)
            {
                EquippedAgentGenerator agentGenerator = ContentBank.Inst.GetEmitter(loadoudID + "_Gen") as EquippedAgentGenerator;
                int size = (int)agentGenerator.SizeType;
                if (!emittersPerSize.ContainsKey(size))
                    emittersPerSize.Add(size, new List<IEmitter>());
                emittersPerSize[size].Add(agentGenerator);
            }
        }

        public static Activity ActivityProvider(string parameters) //TODO: change
        {
            return new WaveChallenge();
        }
    }
}

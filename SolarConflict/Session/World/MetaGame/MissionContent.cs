using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.World.MetaGame
{
    [Serializable]
    public class Patrol : AgentSystem
    {
        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            agent.SetTarget(gameEngine.PlayerAgent, TargetType.Goal);
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }

        
    }

    public class MissionContent
    {

        public static void AddMissions(Scene scene, NodeInfo info, int index)
        {

            //Revelations - 
            if (index == 2)
            {                
                var voidEnemy = scene.AddGameObject("MegaBoss", scene.PlayerStartingPoint * 1.3f, 123, FactionType.Void) as Agent;
                LootSystem lootSystem = new LootSystem();             
                LootEmitter loot = new LootEmitter();
                loot.AddEmitter("StarDestroyerItem");
                loot.AddEmitter("DevastationCore", 1, 4, 5);
                loot.AddEmitter("LivingCore", 1, 4, 5);
                loot.AddEmitter("InhibitorCore", 1, 4, 5);
                lootSystem.LootEmitter = loot;
                voidEnemy.AddSystem(lootSystem);

                for (int i = 0; i < 5; i++)
                {
                    var ship = scene.AddGameObject("ArtilleryShip2", scene.PlayerStartingPoint * 1.3f + Vector2.One * (i * 200), 123, FactionType.Void) as Agent;
                    ship.AddSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed, 0.5f));
                    ship.SetTarget(voidEnemy, TargetType.Goal);
                    //var system = new BlueprintEmitterSystem()
                }

                for (int i = 0; i < 4; i++)
                {
                    var ship = scene.AddGameObject("SmallShip4Support", scene.PlayerStartingPoint * 1.3f - Vector2.One * (i * 200), 123, FactionType.Void) as Agent;
                    ship.AddSystem(new SlotItemDropSystem(ControlSignals.OnDestroyed, 0.5f));
                    ship.SetTarget(voidEnemy, TargetType.Goal);
                    ship.SetTarget(voidEnemy, TargetType.Ally);
                }

                for (int i = 0; i < 1; i++)
                {
                    var ship = scene.AddGameObject("VoidHealingShip", scene.PlayerStartingPoint * 1.3f - Vector2.One * (i * 200), 123, FactionType.Void) as Agent;
                    ship.SetTarget(voidEnemy, TargetType.Goal);
                    ship.SetTarget(voidEnemy, TargetType.Ally);
                }


                //Change void factio or change void relations
                //add healing ships
                //Change void shield
                //Chenge void boos


                Mission goToVoid = MissionFactory.MissionQuickStart("glGoToVoid");
                goToVoid.Objective = new GoToTargetObjective(voidEnemy, 2500);
                goToVoid.Color = Color.Magenta;
                scene.AddMission(goToVoid);

                Mission destroyVoid = MissionFactory.MissionQuickStart("glDestroyVoid");
                destroyVoid.Objective = new DestroyTargetObjective(voidEnemy);
                destroyVoid.Color = Color.Red;
                goToVoid.NextMissionOnComplete = destroyVoid;
            }

            ///
            if (index != 0 && !MetaWorld.Inst.Blackboard.ContainsKey("starport"))
            {
                Mission goToStarport = MissionFactory.MissionQuickStart("glStarport");
                goToStarport.Objective = new GoToObjectTypeObjective(GameObjectType.Starport);
                goToStarport.Color = Color.Magenta;
                scene.AddMission(goToStarport);

                Mission goToOracle = MissionFactory.MissionQuickStart("glToOracleSector", true);
                goToOracle.Objective = new GoToNodeObjective(164);
                goToOracle.DestenationNode = 164;
                goToStarport.NextMissionOnComplete = goToOracle;

                MetaWorld.Inst.Blackboard["starport"] = "t";
            }
            else
            {
                var m = new Mission("Go to the Starport", new TextAsset("Dock with the starport to get missions"));
                m.IsDismissable = true;
                m.Objective = new GoToObjectTypeObjective(GameObjectType.Starport);
                scene.AddMission(m);
            }

            if(info.Type == NodeType.Vile)
            {
                if(!MetaWorld.Inst.Blackboard.ContainsKey("drifter"))
                {
                    MetaWorld.Inst.AddToBlackboard("drifter");
                    var guildDrifter = scene.AddGameObject(Consts.GuildDrifter, scene.PlayerStartingPoint* 1.15f, 10, FactionType.TradingGuild) as Agent;
                    guildDrifter.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, "WormEgg"));
                    
                    var timerMission = new Mission();
                    timerMission.ID = "DrifterTimerMission";
                    timerMission.Objective = new TimeObjective(10);
                    var ch = new MissionChainingHelper(scene);
                    ch.Add(timerMission);
                    var mission = MissionFactory.MissionQuickStart("glMeetDrifter");
                    mission.Objective = new GoToTargetObjective(guildDrifter);              
                    ch.Add(mission);

                    var saveMission = MissionFactory.MissionQuickStart("glSaveDriffter");
                    saveMission.MissionGiver = guildDrifter;
                    saveMission.Reward = new Reward(1000);
                    saveMission.Reward.Items.Add(new Tuple<string, int>("HumanRemains", 2));
                    saveMission.Reward.Items.Add(new Tuple<string, int>("Biomass", 10));
                    saveMission.Reward.Items.Add(new Tuple<string, int>("EmpRecoveryKit1", 5));

                    saveMission.Objective = new ObjectiveGroup();
                    saveMission.AddObjective(new AcquireItemObjective(Consts.DrifterItem));
                    saveMission.AddObjective(new GoToTargetObjective(guildDrifter));
                    saveMission.OnMissionCompletion += (m, s) => { 
                        Agent giver = m.MissionGiver as Agent;
                       // giver.Mass = giver.Mass * 0.1f;
                        //for (int i = 0; i < 3; i++)
                        //{
                        //    giver.ItemSlotsContainer.EquipItem(Consts.DrifterItem);
                        //}
                        s.PlayerAgent?.Inventory.RemoveItem(Consts.DrifterItem);
                        var demonAlter = s.FindClosestByID("DemonAlter", Vector2.Zero, true);
                        giver.SetTarget(demonAlter, TargetType.Goal);
                    };
                    //saveMission.Agent = guildDrifter as Agent;
                    ch.Add(saveMission);


                }
            }

            if (index == 164)
            {
                var portal = GenerationUtils.AddPortal(scene.GameEngine, Vector2.One * 30000, "SecondGateLevel", false);
                var goToPortal = MissionFactory.MissionQuickStart("glOraclePortal");
                var missionGroup = new ObjectiveGroup();
                missionGroup.AddObjective(new GoToTargetObjective(portal, 1000));
                missionGroup.AddObjective(new BlackboardObjective("oracle", "won"));
                goToPortal.Objective = missionGroup;
                scene.AddMission(goToPortal);


                Agent shop = scene.AddGameObject("SmallShop1", portal.Position + new Vector2(-3000, 3000), 90) as Agent;
                shop.ID = "shop";
                shop.collideWithMask = GameObjectType.None;
                scene.AddGameObject("ImbuingStation", shop.Position + Vector2.One * 1000, 90);

                var shopSystem = new ShopSystem();
                shopSystem.shopData.Portrait = Sprite.Get("Gustav_Ally");
                shopSystem.AddItem("DecoyItem");
                shopSystem.AddItem("FlareItem");
                shopSystem.AddItem("MiniMissileLauncher");
                shopSystem.AddItem("SmallEngine3");
                shopSystem.AddItem("EchoSprintItem");
                shopSystem.AddItem("CloakingImbue");
                //shopSystem.AddItemsFromAsset("GustavShopInv");
                shop.AddSystem(shopSystem);


                var hiddenPortal = MissionFactory.MissionQuickStart("glOracleHidden");
                hiddenPortal.Objective = new GoToTargetObjective(portal);
                scene.AddMission(hiddenPortal);
                // scene.AddGameObject("Drifter",Vector2.One * 2)

                var completeGateMission = MissionFactory.MissionQuickStart("glOracleWon");
                completeGateMission.Objective = new BlackboardObjective("oracle", "won");
                completeGateMission.Reward = new Reward();
                completeGateMission.Reward.Items.Add(new Tuple<string, int>("StunImuneImbue", 3));
                completeGateMission.Reward.Items.Add(new Tuple<string, int>("LifelineImbue", 1));
                completeGateMission.Reward.Items.Add(new Tuple<string, int>("ImbuingStationKit", 1));
                scene.AddMission(completeGateMission);

                var goToSol = new Mission("Go to Sol", new TextAsset("Go to Sol, Sol needs you"));
                goToSol.Color = Color.Yellow;
                goToSol.IsGlobal = true;
                goToSol.DestenationNode = 1;
                goToSol.Objective = new GoToNodeObjective(1);
                completeGateMission.NextMissionOnComplete = goToSol;
                //scene.AddMission(goToSol);

            }

            if(index == 1)
            {
                var leonid = scene.AddGameObject("VoidHelperBoss1", new Vector2(0f, 50000f)) as Agent;
                leonid.ID = "VoidHelper1";
                leonid.Name = "Gmork";
                leonid.AddSystem(new WarpInhibitorSystem());


                // Meet the servant
                var meetServant = MissionFactory.MissionQuickStart("solMeetVoid");
                meetServant.Objective = new GoToTargetObjective(leonid, 1600f);

                scene.AddMission(meetServant);

                var mission = MissionFactory.MissionQuickStart("glGoToVoidSector", true);
                //mission.IsGoalHidden = true;        
                mission.Objective = new GoToNodeObjective(2);
               // scene.AddMission(mission);



                var destroy = new Mission();
                destroy.IsHidden = true;
                destroy.IsGoalHidden = true;
                destroy.Objective = new DestroyTargetObjective(leonid);
                scene.AddMission(destroy);
                destroy.NextMissionOnComplete = mission;



            }

            if ( index == 201)
            {
                //MissionChainingHelper ch = new MissionChainingHelper(scene);
                //var arkShip = scene.AddGameObject("PlayerClone", scene.PlayerStartingPoint + new Vector2(3000, 5000));
                //var mission = MissionFactory.MissionQuickStart("glArk");
                //mission.Objective = new GoToTargetObjective(arkShip);
                //ch.Add(mission);

            }

            if(index == 51)
            {
                //var solShip = scene.AddGameObject("SolShip", Vector2.One * 50000, 0, FactionType.Neutral) as Agent;
                //solShip.Light = Lights.HugeLight(Color.Pink);
                //solShip.AddSystem(new Patrol());
                
                //var mission = MissionFactory.MissionQuickStart("glMeetSol");
                //mission.Objective = new GoToTargetObjective(solShip);
                //mission.Color = Color.Purple;
                //scene.AddMission(mission);

            }
            
        }

        //public static Mission 

        /// <summary>
        /// Creates a global hidden mission that summons a mothership attack when you are holding a warp kit and not near mothership
        /// </summary>
        /// <returns></returns>
        public static Mission MothershipAttack()
        {
            throw new NotImplementedException();
        }

        //public static Mission CraftItem(string itemID)
        //{
        //    Item item = ContentBank.Inst.GetItem(itemID, false);
        //    Mission mission = new Mission("Craft " + item.Name, item.Sprite.ID);
        //    mission.Objective = new AcquireItemObjective(itemID, 1, true, tutorialGoal: new TutorialGoal(itemID, false, true, false));
        //    return mission;
        //}
    }
}

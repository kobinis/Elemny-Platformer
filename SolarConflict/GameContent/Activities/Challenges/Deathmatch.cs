//Input
 //loadouts
 //
 //Arena type



//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using SolarConflict.NewContent.Ships;
//using XnaUtils;
//using SolarConflict.GameContent.Ships;

//namespace SolarConflict.NewContent.Levels
//{
//    class Deathmatch :SceneScript //remove class
//    {
//        int playerFaction = 1;
//        int playerIndex = 0;
//        List<GameObject> enemyAgents = new List<GameObject>();
//        protected List<GameObject> playerFactionAgents = new List<GameObject>();
//        Scene scene;

//        int respawnDelay = 3 * 60;
//        //
//        string[] shipsToAdd = {typeof(Avenger).Name, typeof(Marauder).Name, typeof(ShipClass4).Name, typeof(Jaguar).Name, typeof(SupportShip1).Name };
//      //  string[] shipsToAdd = { typeof(Vaporizer).Name, typeof(ShipClass4).Name };

//        public override void InitScript(Scene scene, string parameters)
//        {
//            this.scene = scene;

//            playerIndex = 0;
//            playerFactionAgents.Clear();
//            enemyAgents.Clear();

//            for (int i = 0; i < shipsToAdd.Length; i++)
//            {
//                AddGameObject(shipsToAdd[i], 1, Vector2.Zero, 0);
//            }

//            for (int i = 0; i < shipsToAdd.Length; i++)
//            {
//                Agent ship = (Agent)AddGameObject(shipsToAdd[i], 2, Vector2.Zero, 0);              
//            }


//        /*    for (int i = 0; i < shipsToAdd.Length; i++)
//            {
//                AddGameObject(typeof(Avanger).Name, 2, Vector2.Zero, 0);
//            }*/



//            playerFactionAgents[0].SetControlType(AgentControlType.Player);

//            scene.SetPositionsInCircle(3100, 180);

//            //scene.AddObjectRandomlyInCircle("RepairItem", 50, 3000, 0, 100202);
//            scene.AddObjectRandomlyInCircle("BigAsteroid", 100, 6000, 4000 , 10002);
//            scene.AddObjectRandomlyInCircle("Asteroid", 100, 6500, 4000 , 12231);

//            for (int i = 0; i < 20; i++)
//            {

//                for (int j = 0; j < 5; j++)
//                {
//                    float angle = i / 20f * MathHelper.TwoPi;
//                    scene.AddGameObject("SpeedBooster", 0, FMath.ToCartesian(3000 - j * 100, angle), MathHelper.ToDegrees(angle) + 180);
//                }

//            }

//            scene.AddGameObject("Arena1", 0, -Vector2.UnitY  * 1500, 0);

//            scene.AddObjectRandomlyInCircle("ExplosiveBall", 100, 6000);
//            //SpeedBooster
//        }


//        public GameObject AddGameObject(string id, int faction, Vector2 position, float rotation, AgentControlType controlType = AgentControlType.AI)
//        {
//            GameObject gameObject = scene.AddGameObject(id, faction, position, rotation, controlType);
//            if (gameObject is Agent)
//            {
//                if (gameObject.GetFaction() == playerFaction)
//                {
//                    playerFactionAgents.Add(gameObject);
//                }
//                else
//                {
//                    enemyAgents.Add(gameObject);
//                }
//            }

//            return gameObject;
//        }


//        public override void GameObjectAddedHandler(GameObject gameObject) 
//        {               

//            if (gameObject is Agent) //fix it
//            {
//                Agent agent = (Agent)gameObject;
//                //agent.AddSystem(new FactionMeterBinder(MeterType.FactionKills));   //add it by default             
//                if (agent.GetControlType() == AgentControlType.Player)
//                {
//                    scene.gameEngine.cameraLogic.mainFocus = agent;              
//                }

//                //return agent;
//            }

//        }




//        int switchShipCooldown = 0;
//        public override int UpdateScript(Scene scene)
//        {


//            for (int i = 0; i < enemyAgents.Count; i++)
//            {
//                if (enemyAgents[i].IsNotActive)
//                {
//                    //add AddMeterType -- returns value
//                    enemyAgents[i].SetMeterValue(MeterType.Timer, enemyAgents[i].GetMeterValue(MeterType.Timer) + 1);
//                    if (enemyAgents[i].GetMeterValue(MeterType.Timer) > respawnDelay)
//                    {
//                        enemyAgents[i] = scene.AddGameObject(enemyAgents[i].GetId(), enemyAgents[i].GetFaction(), FMath.ToCartesian(3100, 0), 0, enemyAgents[i].GetControlType());
//                        Agent ship = (Agent)enemyAgents[i];
//                    }
//                }
//            }

//            for (int i = 0; i < playerFactionAgents.Count; i++)
//            {
//                if (playerFactionAgents[i].IsNotActive)
//                {
//                    playerFactionAgents[i].SetMeterValue(MeterType.Timer, playerFactionAgents[i].GetMeterValue(MeterType.Timer) + 1);
//                    if (playerFactionAgents[i].GetMeterValue(MeterType.Timer) > 1.5f * 60)
//                    {
//                        float killNum = playerFactionAgents[i].GetMeterValue(MeterType.Kills);
//                        playerFactionAgents[i] = scene.AddGameObject(playerFactionAgents[i].GetId(), playerFactionAgents[i].GetFaction(), FMath.ToCartesian(3100, MathHelper.Pi), 0, playerFactionAgents[i].GetControlType());
//                        playerFactionAgents[i].SetMeterValue(MeterType.Kills, killNum); //??
//                    }
//                }
//            }

//            for (int i = 0; i < playerFactionAgents.Count; i++)
//            {
//                if (playerFactionAgents[i].GetControlType() == AgentControlType.Player)
//                {
//                    playerIndex = i;
//                }
//            }

//            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && switchShipCooldown <=0)
//            {
//                switchShipCooldown = 20;
//                playerFactionAgents[playerIndex].SetControlType(AgentControlType.AI);
//                playerIndex = (playerIndex + 1) % playerFactionAgents.Count;
//                playerFactionAgents[playerIndex].SetControlType(AgentControlType.Player);
//                scene.gameEngine.cameraLogic.mainFocus = playerFactionAgents[playerIndex];

//            }
//            switchShipCooldown--;



//            scene.SetText("Your Team Kills:" + scene.gameEngine.GetFaction(1).GetMeter(MeterType.FactionKills).Value.ToString() +
//            "    Enemy Team Kills:" + scene.gameEngine.GetFaction(2).GetMeter(MeterType.FactionKills).Value.ToString());
//            return 0;
//        }

//        public override void EndScript(Scene scene)
//        {

//        }



//        public static Activity ActivityProvider(ActivityManager activityManager, string parameters)
//        {
//            return new Scene(activityManager, new Deathmatch());
//        }
//    }
//}

/*
   private void RespawnUpdate()
        {
            //foreach (var factionKeyValue in gameEngine.factions)
            //{
            //    Faction faction = factionKeyValue.Value;

            //    for (int i = 0; i < faction.shipsToRespawn.Count; i++)
            //    {
            //        GameObject agent = faction.shipsToRespawn[i];
            //        if (agent.IsNotActive)
            //        {
            //            agent.SetMeterValue(MeterType.Timer, agent.GetMeterValue(MeterType.Timer) + 1); //maybe add an outside counter?

            //            if (agent.GetMeterValue(MeterType.Timer) > 1.5f * 60)
            //            {

            //                faction.shipsToRespawn[i] = AddGameObject(agent.GetId(), agent.GetFaction(), faction.GetStartingPoint(), 0, agent.GetControlType());
            //                faction.shipsToRespawn[i].SetMeterValue(MeterType.Kills, agent.GetMeterValue(MeterType.Kills)); //??
            //            }
            //        }
            //    }

            //}            
        }
        */

//public void AddShipsToRespanList() //add all game objects to respan list
//{
//    foreach (var gameObject in _gameEngine.potentialTargets)
//    {
//        int factionIndex = gameObject.GetFaction();
//        Faction faction = _gameEngine.GetFaction(factionIndex);
//        faction.shipsToRespawn.Add(gameObject);
//    }
//}

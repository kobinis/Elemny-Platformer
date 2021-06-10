using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Session.World.MissionManagment;
using XnaUtils.Graphics;

namespace SolarConflict.NodeGeneration.Features
{
    class DestroyTheBaseFeature:GenerationFeature
    {
        //Type need, clock
        public GenerationFeature CenterFeature;
        public GenerationFeature Shop;
        List<GenerationFeature> ringElementFeatures;
        public int NumberOfRinges { get { return ringElementFeatures.Count; } }
        
        public DestroyTheBaseFeature()
        {
            //The same one with homing missiles
            //The same one with cloacked /mines/ ships
            //The same one with EMP,
            //One higher levels
            
            ringElementFeatures = new List<GenerationFeature>();            
            ringElementFeatures.Add(new EmitterFeature(ContentBank.Inst.GetEmitter("TurretA_Gen")));
            ringElementFeatures.Add(new EmitterFeature(ContentBank.Inst.GetEmitter("TurretA_Gen")));
            ringElementFeatures.Add(new EmitterFeature(ContentBank.Inst.GetEmitter("TurretA_Gen")));
            ringElementFeatures.Add(new EmitterFeature(ContentBank.Inst.GetEmitter("TurretA_Gen")));
            //ringElementFeatures.Add(new EmitterFeature(ContentBank.Inst.GetEmitter("TurretA_Gen")));

            //The same ID to all the mission, checks if there is an ID 
            //TODO: inside a position emitter, towards the player in a fixed radius
            AgentFeature shopFeature = new AgentFeature();
            shopFeature.SetFaction(Framework.FactionType.TradingGuild);
            ShopSystem shopSystem = new ShopSystem(); //TODO: add feature that generates items acoording to level from category
            shopSystem.AddItem("Cloack2"); //Change to activve cloking device
            shopSystem.AddItem("EchoSprintItem");
            shopSystem.AddItem("Shotgun5");
            shopSystem.AddItem("HomeBeaconKit");
            shopFeature.AddAgentSystem(shopSystem);
            shopFeature.AgenEmitterID = "SmallShop1";
            shopFeature.LocalPosition = -Vector2.UnitY * 15000;
                        

            Shop = shopFeature;
            //AddChild(Shop);
            var mainBase = new AgentFeature();
            mainBase.AgenEmitterID = "PirateBase_Gen";
            CenterFeature = mainBase;
            //AddChild(CenterFeature);
        }

        public override GameObject Generation(Scene scene, SceneGenerator generator)
        {
            CenterFeature.Parent = this;
            Shop.Parent = this;
            var mainBase = CenterFeature.Generation(scene, generator);
            var shopObject = Shop.Generation(scene, generator) as Agent;
            shopObject.Name = "Gepetto's Weapon Shop";
            var id = scene.GetNewMissionID();

            //var mission = MissionFactory.GoToTargetMission(shopObject, "Help old Gepetto", "You need to help old Gepetto with a Pirate infestation");
            //mission.Icon = Sprite.Get("Gepetto");
            //mission.ID = id;
            //var destroyBaseMission = MissionFactory.DestroyTargetObjective(mainBase, "Destroy the pirate base", "Destroy the evil pirate base\nYou can buy stuff in my shop, cha-ching!");
            //destroyBaseMission.ID =  id;
            //var goBackMission = MissionFactory.GoToTargetMission(shopObject, "Tell Gepetto about the carnage", "Tell the story to Gepetto, so you can become a real boy"); //Plus survive
            //goBackMission.OnMissionCompletion += delegate (Mission mission_, Scene scene_)
            //{ scene_.DialogManager.AddDialogBox("You are on your way to greatness.", "Gepetto"); };
            //destroyBaseMission.NextMissionOnComplete = goBackMission;

            //AgentDialogSystem dialog = new AgentDialogSystem(); //You can add dialog to mission
            //dialog.DefaultPotratitID = "Gepetto";
            //dialog.AddText("The pirates are bothering me");
            //dialog.AddText("Will you kill the fucking pirate base?");
            //dialog.AddText("It is well defended, maybe you can buy some stuff in my shop to help");
            //dialog.MissionToAdd = destroyBaseMission;
            //dialog.DialogStart += delegate (Scene scene_)
            //{
            //     scene.MissionManager.AddMission(mission.ChainedMission);
            //};
           // shopObject.AddSystem(dialog);           
           // scene.AddMissionGenerator(mission);


            for (int i = 0; i < NumberOfRinges; i++)
            {
                var feature = ringElementFeatures[i];
                feature.SetLevel( Math.Min( Level + i, 10));
                feature.Parent = this;
                AddRing(feature, i, scene, generator);
            }
            return base.Generation(scene, generator);
        }

        private void AddRing(GenerationFeature feature, int ringIndex, Scene scene, SceneGenerator generator)
        {
            float radMult = 1200;
            int numberOfCalls = (ringIndex + 1) * 3;
            for (int i = 0; i < numberOfCalls; i++)
            {
                float delta = MathHelper.TwoPi / numberOfCalls;
                float angle = (ringIndex * MathHelper.Pi / NumberOfRinges ) + delta * i;
                float rad = 2000 + ringIndex * radMult;
                Vector2 deltaPos = FMath.ToCartesian(rad, angle);
                feature.Position = Position + deltaPos;
                feature.SetFaction(Faction);
                feature.Generation(scene, generator);
            }
        }

        

    }
}

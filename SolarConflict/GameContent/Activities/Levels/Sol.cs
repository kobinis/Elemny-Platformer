using Microsoft.Xna.Framework;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.Framework;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.Framework.Emitters;
using SolarConflict.Session.World.MissionManagment.GlobalObjectives;

namespace SolarConflict.GameContent.Activities.Levels {

    /// <remarks>TODO: move</remarks>
 
    class Sol {

        public static void AddSceneSpecificContent() {

            // Emitter for revealing the entire galaxy map
            ContentBank.Inst.AddContent(new CallbackEmitter("solRevealMap", (gameEngine, parent, faction, refPosition, refVelocity, refRotation) => {
                GalaxyMap.Inst.Nodes[85].Reveal();
                GalaxyMap.Inst.Nodes[180].Reveal();
                GalaxyMap.Inst.Nodes[26].Reveal();
                return null;
            }));            
            
            // Emitters for obeying/renouncing the Void
            ContentBank.Inst.AddContent(new CallbackEmitter("solRenounceVoid", (gameEngine, parent, faction, refPosition, refVelocity, refRotation) => {
                // Make Void hostile, align the servant with the Void
                var voidFaction = gameEngine.Factions[(int)FactionType.Void];
                voidFaction.SetRelationToFaction(gameEngine, FactionType.Player,
                    Math.Min(voidFaction.GetRelationToFaction(FactionType.Player), -1));

                var voidShip = gameEngine.Scene.FindGameObjectByID("VoidHelper1", true);
                voidShip?.SetFactionType(FactionType.Void);
                voidShip?.SetTarget(gameEngine.PlayerAgent, TargetType.Enemy);

                // Add mission to destroy the servant, reveal map on success
                var destroyServant = MissionFactory.MissionQuickStart("solDestroyVoid");
                destroyServant.Objective = new DestroyTargetObjective(voidShip);

                destroyServant.EmitterOnComplete = ContentBank.Inst.GetEmitter("solRevealMap");

                gameEngine.Scene.AddMission(destroyServant);
                // TODO: maybe also reveal map if servant is somehow destroyed before being spoken to? That's some extreme sequence breaking

                return null;
            }));

            ContentBank.Inst.AddContent(new CallbackEmitter("solObeyVoid", (gameEngine, parent, faction, refPosition, refVelocity, refRotation) => {
                // Make Void friendly, align the servant with the Void
                var voidFaction = gameEngine.Factions[(int)FactionType.Void];
                voidFaction.SetRelationToFaction(gameEngine, FactionType.Player,
                    Math.Max(voidFaction.GetRelationToFaction(FactionType.Player), 1));

                var voidShip = gameEngine.Scene.FindGameObjectByID("VoidHelper1", true);
                voidShip?.SetFactionType(FactionType.Void);
                voidShip?.SetTarget(null, TargetType.Enemy);
                voidShip.SetFactionType(FactionType.Player);

                MetaWorld.Inst.GetFaction(FactionType.TradingGuild).SetRelationToFaction(gameEngine, FactionType.Player, -1);
                MetaWorld.Inst.GetFaction(FactionType.Empire).SetRelationToFaction(gameEngine, FactionType.Player, -1);
                MetaWorld.Inst.GetFaction(FactionType.Federation).SetRelationToFaction(gameEngine, FactionType.Player, -1);

                // Reveal map
                ContentBank.Inst.GetEmitter("StarDestroyerItem").Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation);

                return null;
            }));
        }

        public static void Initialize(Scene scene) {
            // Add Void servant, make it a warp inhibitor

            
        }
    }
}

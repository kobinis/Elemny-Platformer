using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.XnaUtils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject {
    class AgentUtils {

        public static int CalculateAgentHullCost(Agent agent)
        {
            Dictionary<SlotType, float> slotTypeMult = new Dictionary<SlotType, float>
            { { SlotType.Weapon, 1.2f }, { SlotType.Turret, 1.2f } ,{ SlotType.Shield, 1.5f },
              {SlotType.Engine, 1f }, {SlotType.MainEngine, 1f }, {SlotType.Rotation, 0f } };
            float totalMultiplyer = 0.5f;
            float hitpointsWeight = 1f;
            float cost = agent.MaxHitpoints * hitpointsWeight;
            float baseItemSlotCost = 10f;
            for (int i = 0; i < agent.ItemSlotsContainer.Count; i++)
            {
                float multiplyer = 1;    
                foreach (var keyValue in slotTypeMult)
                {
                    if(agent.ItemSlotsContainer[i].Type.HasFlag(keyValue.Key))
                    {
                        multiplyer = Math.Max(multiplyer, keyValue.Value);
                    }
                }
                cost += baseItemSlotCost * multiplyer;
            }            
            return (int)Math.Round(cost * totalMultiplyer);
        }

        public static string DescribeAgentHull(Agent agent)
        {
            StringBuilder sb = new StringBuilder();
            if(agent.Name != null)
                sb.AppendLine("Class: " + agent.Name);
            else
                sb.AppendLine("ID: " + agent.ID);
            if(DebugUtils.Mode == ModeType.Debug)
                sb.AppendLine("ID: " + agent.ID);
            sb.Append("#line{}");
            if ((agent.gameObjectType & GameObjectType.NonRotating) > 0)
            {
                sb.AppendLine("#image{absIcon} Control type: #color{255,255,0}Absolute Control#dcolor{}");
            }
            else
            {
                sb.AppendLine("#image{relIcon} Control type: #color{255,255,0}Tank Control#dcolor{}");
            }
            sb.AppendLine("Hull Cost: " + agent.HullCost.ToString() + "#image{coin}");
            sb.AppendLine("HitPoints: " +Palette.Hitpoints.ToTag(agent.MaxHitpoints.ToString()));
            sb.AppendLine("Size Class: " + agent.SizeType.ToString());
            sb.AppendLine("Faction: " +MetaWorld.Inst.GetFaction(agent.FactionType).ToTag());
            sb.Append("#line{}");
            if (agent.Inventory != null)
                sb.AppendLine("Inventory Size: " + Color.Yellow.ToTag(agent.Inventory.Size.ToString()));
            if (agent.ItemSlotsContainer != null)
            {
                int shieldNum = agent.ItemSlotsContainer.GetSlotNumber(SlotType.Shield);
                sb.AppendLine("Shield Slots: " + Palette.Shield.ToTag(shieldNum.ToString()));
                int genNum = agent.ItemSlotsContainer.GetSlotNumber(SlotType.Generator);
                sb.AppendLine("Generator Slots: " + Palette.Energy.ToTag(genNum.ToString()));
                int weapon = agent.ItemSlotsContainer.GetSlotNumber(SlotType.Weapon | SlotType.Turret);
                sb.AppendLine("Weapon & Turret Slots: " + Palette.Damage.ToTag(weapon.ToString()));
                sb.AppendLine("Utility Slots: " + Color.Gray.ToTag((agent.ItemSlotsContainer.Count() - weapon - genNum - shieldNum).ToString()));
            }
            //Cloacking/Shiled/ rotation type/ slot ypes?            
            return sb.ToString();
        }

        /// <param name="showTransientState">If true, show changeable Agent-specific state like current HP. Doesn't quite work at the moment</param>
        public static string DescribeStatsAndAbilities(Agent agent, IEnumerable<Tuple<Item, ControlSignals>> itemsAndActivations = null, bool showTransientState = false) { 
            itemsAndActivations = itemsAndActivations ?? agent.ItemSlotsContainer?.Where(s => s.Item != null)
                .Select(s => Tuple.Create(s.Item, s.ActivationSignal));

            var items = itemsAndActivations.Select(iAa => iAa.Item1);

            var result = "";

            // Stats            
            Func<MeterType, string> displayStat = (t)
                => (showTransientState ? (agent.GetMeterValue(t).ToString() + "/") : "") + agent.GetMeter(t).MaxValue.ToString();

            result += $" #image{{HP}} Hitpoints: {displayStat(MeterType.Hitpoints)}";
                        
            //if (_agent.GetMeterValue(MeterType.Shield) > 0)
            //    description += $"\n#image{{Shield}} Shields: {displayStat(MeterType.Shield)}";


                        
            var shields = items.Where(i => (i?.Profile?.SlotType & SlotType.Shield) > 0);

            if (shields.Count() > 0) {
                var strength = 0f;
                var generation = 0f;

                shields.Do(s => {
                    var system = (s.System as SystemGroup).GetSystem<MeterGenerator>();
                    if (system != null) {
                        strength += system.MaxValue;
                        generation += system.GenerationAmountPerSec;
                    }
                });

                // ^ TODO: delegate to some other class, either a Shield class, or StatExtractor
                if (strength > 0f || generation > 0f) {
                    result += $"\n #image{{Shield}} Shield: {(int)Math.Round(strength)} Gen:{(int)Math.Round(generation)} points/s";
                }
            }

            var generator = items.FirstOrDefault(i => (i?.Profile?.SlotType & SlotType.Generator) > 0);
            if (generator != null) {
                // Power generation. ASSUMPTION: at most one generator                
                var system = (generator.System as MeterGenerator);
                if(system != null)
                    result += $"\n #image{{Energy}} Energy: {(int)Math.Round(system.MaxValue)} Gen:{(int)Math.Round(system.GenerationAmountPerSec)} units/s";
            }

            var rotationEngine = items.FirstOrDefault(i => (i?.Profile?.SlotType & SlotType.Rotation) > 0);
            if (rotationEngine != null) {

                // Rotation rate. ASSUMPTION: at most one rotation engine
                var system = rotationEngine.System as AgentRotationEngine;
                if (system != null)
                {
                    var rate = (system.RotationForce / agent.RotationMass) * Utility.Frames(1f) / (2 * Math.PI) * 360;
                    result += $"\n {rotationEngine.Profile.IconSprite.ToTag()} Rotation rate: {(int)rate} deg/s";
                }
            }

            // Controls
            var manualControlSignals = new ControlSignals[] {
                    ControlSignals.Action1, ControlSignals.Action2, ControlSignals.Action3, ControlSignals.Action4,
                    ControlSignals.Brake, ControlSignals.Left, ControlSignals.Right, ControlSignals.Up, ControlSignals.Down,
                    ControlSignals.QuickUse1, ControlSignals.QuickUse2, ControlSignals.QuickUse3, ControlSignals.QuickUse4
                }.ToSet(); // control signals the player cares about


          //  result += "\nMOVEMENT:";

            // Thrusters
            var engineKeys = itemsAndActivations.Where(i => (i.Item1.Profile.SlotType & SlotType.Engine) > 0)
                .Where(i => manualControlSignals.Contains(i.Item2))
                .Select(i => KeysSettings.PrimaryKey(i.Item2).ToString()).ToSet(); // keys mapped to engine items

            var keyOrdering = "WASDBCEFGHIJKLMNOPQRTUVXYZ"; // If the letters W, A, S, or D appear, keep them in that order. Alphabetize other letters
            var ordered = engineKeys.OrderBy(k => keyOrdering.IndexOf(k[0]));

            //if (ordered.Count() > 0)
            //    result += $"\n    {ordered.Aggregate((l, r) => l + r)} - Thrusters\n";

            // Misc movement
       //     result += $"\n    {PlayerMouseAndKeys.PrimaryKey(ControlSignals.Brake)} - Toggle brakes";

            result += "\n Items:";

            var inputOrdering = new ControlSignals[] {
                ControlSignals.Action1,
                ControlSignals.Action2,
                ControlSignals.Action3,
                ControlSignals.Action4,
                ControlSignals.Up,
                ControlSignals.Left,
                ControlSignals.Down,
                ControlSignals.Right,
            };

            var pertinentItems = itemsAndActivations.Where(i => (i.Item1.IsShownInAgentTooltip == true) || manualControlSignals.Contains(i.Item2))
                .OrderBy(i => inputOrdering.IndexOfFirst(j => j == i.Item2))
                .ThenBy(i => KeysSettings.PrimaryKey(i.Item2).Key.ToString())
                .ThenBy(i => i.Item1.Profile.Name ?? i.Item1.Profile.Id);
            // ^ items that have keymappings and are important enough to show on the HUD, ordered by input (per the above order, defaults to alphabetical), then by item name

            pertinentItems.Do(i => result +=
            $"\n    {KeysSettings.PrimaryKey(i.Item2).GetTag()} - {i.Item1.IconTag} "
            + ItemProfile.GetLevelColor(i.Item1.Profile.Level).ToTag($"{i.Item1.Profile.Name ?? i.Item1.Profile.Id}"));

            return result;
        }
    }
}

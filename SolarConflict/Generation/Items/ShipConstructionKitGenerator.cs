using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.GameContent.Emitters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.ContentGeneration.Items
{
    class ShipConstructionKitGenerator
    {
        public static void MakeAll()
        {
            var loadouts = ContentBank.Inst.GetAllLoadout();
            foreach (AgentLoadout loadout in loadouts) //Just to it for faction ships?
            {
                ContentBank.Inst.AddContent(MakeItem(loadout.ID, loadout.GetSprite().ID, loadout.Cost,null,11, true));

            }
        }

        public static Item MakeItem(string emitterId, string textureId, float cost, string name, int level, bool addToFleet = false)
        {

            AgentLoadout loadout = ContentBank.Inst.GetEmitter(emitterId) as AgentLoadout;
            string description = "Adds a ship to your fleet if you have a free slot in your Hangar #action{Hangar}";
            if (name == null && loadout != null)
            {
                if (loadout.Name != null)
                    name = loadout.Name;
                else
                    name = loadout.ID;
                description += "\n" + loadout.FullDescription;
            }
            
            ItemProfile profile = ItemQuickStart.Profile(name + "", description, 0, null, null);                        
            Sprite texture = Sprite.Get(textureId);
            profile.IconTextureID = textureId;
            profile.TextureScale = 100 / (float)texture.Width;

            profile.SlotType = SlotType.None;
            profile.Category = ItemCategory.ConstructionKit;
            profile.ItemSize = SizeType.Small;
            profile.IsWorkingInInventory = true;
            profile.IsConsumed = true;
            profile.IsActivatable = true;
            profile.BuyPrice = cost;
            profile.SellPrice = cost * 0.5f;
            profile.Level = level;
            profile.Category = ItemCategory.ConstructionKit;
           // profile.SlotType = SlotType.Consumable;
            Item item = new Item(profile);
            item.ID = emitterId + "KitItem";
            item.Profile.SlotType |= SlotType.Consumable;

            AgentSystem agentSystem;

            if (addToFleet)
            {
                AddShipToFleetSystem system = new AddShipToFleetSystem();
                system.Emitter = ContentBank.Inst.GetEmitter(emitterId);
                system.Activation = ControlSignals.AlwaysOn;
                agentSystem = system;
            }
            else
            {
                agentSystem = new BasicEmitterCallerSystem(ControlSignals.None, emitterId);
            }                       
            item.System = new TurretSystemHolder(agentSystem, Vector2.Zero, null); //to make it shot to the diraction of the curser

            return item;
        }
    }
}

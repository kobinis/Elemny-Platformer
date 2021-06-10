using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes.HudEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.GUI
{
    public interface IInventoryControl
    {
        Inventory Inventory { get; }
        ItemControl[] ItemControls { get; }
    }

    [Serializable]
    public class TutorialGoal //Store this in a mission or other place in scene
    {
        public Sprite GuiMarking = Sprite.Get("invArrow"); //No need

        public bool IsDestInSlot;
        public bool IsDestAlly;
        public bool IsDestEmpty;
        public int? DestIndex;

        public string ItemID;
        public bool IsSourceCrafting; //Is source of the item can be crafting


        public TutorialGoal(string itemID, bool isDestEmpty, bool isDestSlot = true, bool isDestAlly = false, bool isSourceCrafting = true, int? destIndex = null)
        {
            ItemID = itemID;
            IsDestEmpty = isDestEmpty;
            IsDestInSlot = isDestSlot;
            IsSourceCrafting = isSourceCrafting; // add to constractor
            IsDestAlly = isDestAlly;
            DestIndex = destIndex;
        }

        public bool DrawTutorialGoal(CraftingControl craftingControl, ItemControl cursorItemControl, AgentSlotsControl playerSlotsControl, IInventoryControl playerInventoryControl, AgentSlotsControl allySlotControl, IInventoryControl allyInventoryControl)
        {
            if (IsDestAlly && allySlotControl == null)
                return false;

            var pertinentSlots = playerSlotsControl;
            var pertinentInventory = playerInventoryControl;

            if (IsDestAlly)
            {
                pertinentSlots = allySlotControl;
                pertinentInventory = allyInventoryControl;
            }

            // Search slots
            if (pertinentSlots.agent.ItemSlotsContainer.FindItem(ItemID).HasValue)
                // Item already installed, we're done
                return true;

            if ((!IsDestInSlot) && pertinentInventory.Inventory.FindItem(ItemID) >= 0)
                // Item needn't be installed; found in inventory, we're done
                return true;

            if (cursorItemControl.Item != null && cursorItemControl.Item.ID == ItemID)
            {
                // We're holding the item, where does it go?
                if (IsDestInSlot)
                {
                    // In an installed item slot
                    int? slotIndex = null;
                    if (DestIndex.HasValue)
                        slotIndex = DestIndex.Value;
                    else
                        slotIndex = pertinentSlots.agent.ItemSlotsContainer.FindItemSlot(ContentBank.Inst.GetItem(ItemID, false), IsDestEmpty);
                    if (slotIndex.HasValue)
                    {
                        var slotControl = pertinentSlots.GetItemSlot(slotIndex.Value);
                        var destinationSlotControl = slotControl;
                        GraphicsUtils.Line(Game1.sb, ActivityManager.Inst.InputState.Cursor.Position, destinationSlotControl.Position, Color.YellowGreen, 5, GraphicsUtils.DefaultSetPixel);
                        HudUtils.DrawArrow(Game1.sb, GuiMarking, destinationSlotControl.Position);

                    }
                    return false;
                }
                else
                {
                    // In an inventory
                    int slotIndex = -1;
                    if (DestIndex.HasValue)
                        slotIndex = DestIndex.Value;
                    else
                        slotIndex = pertinentInventory.Inventory.IndexOfFirstEmptySlot();

                    if (slotIndex >= 0)
                    {
                        GraphicsUtils.Line(Game1.sb, ActivityManager.Inst.InputState.Cursor.Position, pertinentInventory.ItemControls[slotIndex].Position, Color.YellowGreen, 5, GraphicsUtils.DefaultSetPixel);
                        HudUtils.DrawArrow(Game1.sb, GuiMarking, pertinentInventory.ItemControls[slotIndex].Position);
                    }
                    return false;
                }
            }

            // We're not holding the item, see if we can pick it up
            var sourceInventory = playerInventoryControl;
            var sourceIndex = playerInventoryControl.Inventory.FindItem(ItemID);
            if (sourceIndex < 0 && allyInventoryControl != null)
            {
                // Not in one inventory, check the other
                sourceInventory = allyInventoryControl;
                sourceIndex = allyInventoryControl.Inventory.FindItem(ItemID);
            }

            if (sourceIndex >= 0)
            {
                // Found in an inventory
                GraphicsUtils.Line(Game1.sb, ActivityManager.Inst.InputState.Cursor.Position, sourceInventory.ItemControls[sourceIndex].Position, Color.YellowGreen, 5, GraphicsUtils.DefaultSetPixel);
                HudUtils.DrawArrow(Game1.sb, GuiMarking, sourceInventory.ItemControls[sourceIndex].Position);
                return false;
            }

            // Not held and not in an inventory, can we craft it?
            if (IsSourceCrafting)
            {
                CraftingRecipeControl child = null;
                foreach (var control in craftingControl.GetChildren())
                {
                    CraftingRecipeControl recipeControl = control as CraftingRecipeControl;
                    if (recipeControl.CraftingRecipe.CraftedItem.ID == ItemID)
                    {
                        child = recipeControl;
                        break;
                    }
                }
                if (child != null)
                {
                    GraphicsUtils.Line(Game1.sb, ActivityManager.Inst.InputState.Cursor.Position, child.Position, Color.YellowGreen, 5, GraphicsUtils.DefaultSetPixel);
                    HudUtils.DrawArrow(Game1.sb, GuiMarking, child.Position);
                }
            }


            return false;
        }
    }
}

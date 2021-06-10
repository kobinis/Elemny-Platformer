using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Agents.Systems.Misc
{
    /// <summary>
    /// Combines an item with an ability(system)
    /// </summary>
    [Serializable]
    public class ImbueSystem:AgentSystem
    {
        public string PrefixText; //TODO: possibly change to ID
        public string DescriptionText;
        public string SecounderyIcon;

        public AgentSystem system;

       

        /// <summary>
        /// Trys to add the ability(system) to the item, returens true on sucsess
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ImbueItem(Item item, string imbuingItemID)
        {
            if ((item.ItemFlags & ItemFlags.Imbuable) == ItemFlags.Imbuable)
            {
                item.Profile = item.Profile.GetWorkingCopy();

                if (item.System is SystemGroup)
                {
                    (item.System as SystemGroup).AddSystem(system.GetWorkingCopy());                    
                }
                else
                {
                    SystemGroup systemGroup = new SystemGroup();
                    systemGroup.AddSystem(item.System);
                    systemGroup.AddSystem(system.GetWorkingCopy());
                    item.System = systemGroup;
                }
                if(SecounderyIcon == null)
                {                    
                    item.Profile.IconSecondarySprite = ContentBank.Inst.GetItem(imbuingItemID, false).Sprite;
                }
                else
                    item.Profile.IconSecondarySprite = Sprite.Get(SecounderyIcon);
                item.ItemFlags &= ~ItemFlags.Imbuable;
                item.ItemFlags |= ItemFlags.ShowOnHud;
                item.Profile.Name = $"{PrefixText} { item.Profile.Name}";
                item.Profile.DescriptionSuffix = DescriptionText;
                item.Profile.Id = $"#im:{item.Profile.Id}:{imbuingItemID}";
                return true;
            }
            return false;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            return false; //
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}

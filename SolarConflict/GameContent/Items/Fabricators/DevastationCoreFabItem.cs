using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Emitters;

namespace SolarConflict.GameContent.Items
{
    class DevastationCoreFabItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Devastation Core Fabricator", "Makes Devastation Cores", 4,
            "DevastationCoreFab", null) ;
            profile.SlotType = SlotType.Fabricator;            

            Item item = new Item(profile);
            EmitterCallerSystem system = new EmitterCallerSystem("DevastationCoreRecipe");
            system.CooldownTime = 30;
            system.velocity = Vector2.Zero;
            
            item.System = system;
            return item;
        }
    }
}

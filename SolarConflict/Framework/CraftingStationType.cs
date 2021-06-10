using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework
{    
    [Flags]
    public enum CraftingStationType : uint
    {
        None = 0,
        Basic = 1 << 1,
        CraftingStation = 1 << 2,
        AdvancedCraftingStation = 1 << 3,
        Forge = 1 << 4,
        AdvancedForge = 1 << 5,
        Meteor = 1 << 6,
        Sun = 1 << 7,
        Armory = 1 << 8,        
        Mothership = 1 << 9,
        Vanity = 1 << 10,
        Mining = 1 << 11,
        Uncraftable = 1 <<12,
        MissileAmmo = 1 << 13,
        MineAmmo = 1 << 14,
        BoomerangAmmo = 1<< 15,
        Rotating = 1 << 16,
        ResourceMine = 1<< 17,
        AdvancedArmory = 1 << 18,
        DemonAlter = 1 << 19,
        ImbuingStation = 1 << 20,
        BlackHole = 1 << 21,
        Nebula = 1 << 22,
        And = 1 << 30,
        All = (uint)(0xFFFFFFFF & ~(1 << 30)),
    };
}

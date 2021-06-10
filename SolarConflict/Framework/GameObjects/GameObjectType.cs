using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Flags]
    public enum GameObjectType : uint {
        None = 0,
        Agent = 1 << 1,
        Projectile = 1 << 2,
        Item = 1 << 3,
        Institute = 1 << 4,
        Open1 = 1 << 5,
        Open2 = 1 << 6,
        /// <summary>
        /// Is Visible in MiniMap
        /// </summary>
        VisibleInMiniMap = 1 << 7,
        /// <summary>
        /// An object that shows in the map,
        /// </summary>
        PotentialTarget = 1 << 8,
        Collectible = 1 << 9,
        Ship = 1 << 10,
        Asteroid = 1 << 11,
        Mothership = 1 << 12,
        Mine = 1 << 13,
        EnergyProjectile = 1 << 14,
        PhysicalProjectile = 1 << 15,
        CraftingStation = 1 << 16,
        //AdvancedCraftingStation = 1 << 17,
        Starport = 1 << 18,
        Portal = 1 << 19,
        Lab = 1 << 20,
        Factory = 1 << 21,
        Shipyard = 1 << 22,
        Sun = 1 << 23,
        PlaceOfIntrest = 1 << 24,
        NonRotating = 1 << 25,
        Ramming = 1 << 26,
        Turret = 1 << 27,
        Light = 1 << 28,
        Mineable = 1 << 29,
        AlwaysDraw = 1 << 30, //Temp
        IsProjectileTarget = (uint)1 << 31,
        All = 0xFFFFFFFF,
    };
}

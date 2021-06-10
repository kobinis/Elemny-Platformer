using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Flags]
    public enum PlayerCommand { None = 0, Use = 2, SwapUp = 4, SwapDown = 8, CallHelp = 16, Inventory = 32 }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict.Framework
{        
    //Maybe remove
    public class Player
    {
        int index;
        string name;
        IAgentControl control; //change to player control
        GameObject playerObject;       
    }
}

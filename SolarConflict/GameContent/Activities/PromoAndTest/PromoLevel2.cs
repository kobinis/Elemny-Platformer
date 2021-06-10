using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.GameContent.Activities
{
    public class PromoLevel2:Scene
    {
        

        public override void OnEnter(ActivityParameters parameters)
        {
            IsShipSwitchable = true;
            UpdatePlayerShip = false;
            AddGameObject("Player", FactionType.Player, Vector2.Zero, 0, AgentControlType.Player);
            //AddGameObject("FederationBaseAttacker", Vector2.UnitY * 2000, 0, Factions.Player);
            AddGameObject("Mine1", Vector2.One * 5000, 0);
        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new PromoLevel2();
        }
    }
}

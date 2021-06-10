using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict
{
    /// <summary>
    /// Interface for player control
    /// </summary>
    public interface  IPlayerControl
    {
        ControlSignals UpdateAgent(int index, Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections);
        void CommandUpdate(Scene scene); //this is for player commands

        bool IsCommandOn(PlayerCommand command);
        bool IsCommandClicked(PlayerCommand command);

        string GetCommandTag(PlayerCommand command);
        string GetControlTag(ControlSignals signal);
    }
}

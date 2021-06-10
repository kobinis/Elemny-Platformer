using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.PlayersManagement
{
    /// <summary>
    /// Manages input devices for players
    /// </summary>
    [Serializable]
    public class PlayersManager
    {
        public enum ControlType { KeysAndMouse, Xbox }; //??, Keys, Joystick,         
        private static PlayersManager instance = null;
        public static PlayersManager Inst
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayersManager();
                }
                return instance;
            }
        }

        const int MaxPlayers = 2;
        private int _playerRefereshTime = Utility.Frames(3);
        private Agent _player;


        public IPlayerControl[] players; //Maybe change to player control //change to private
        /// <summary>Cooldown on automatically switching to new player agent, if none selected</summary>
        int refreshPlayerCooldown = 0;

        public ControlType mainPlayerControlType;

        public ControlType MainPlayerControlType
        {
            get { return mainPlayerControlType; }
            set {
                if (value != mainPlayerControlType)
                {
                    mainPlayerControlType = value;
                    CreatePlayerControls();
                }
            }
        }    
        
        public void SwitchControlType(ControlType controlType)
        {
            MainPlayerControlType = controlType;
            CreatePlayerControls();
        }    

        public PlayersManager()
        {
            refreshPlayerCooldown = 1;
            players = new IPlayerControl[MaxPlayers];
            MainPlayerControlType = ControlType.KeysAndMouse;
            CreatePlayerControls();     
        }

        public void CreatePlayerControls()
        {
            if (mainPlayerControlType == ControlType.KeysAndMouse) //TODO:Or no controller connected
            {
                players[0] = new PlayerMouseAndKeys();
                for (int i = 1; i < MaxPlayers; ++i)
                    players[i] = new PlayerXbox((Microsoft.Xna.Framework.PlayerIndex)i - 1);
            }
            else
            {
                var gamepadState = GamePad.GetState(PlayerIndex.One);//, GamePadDeadZone.Circular);

                if (gamepadState.IsConnected == false)
                {
                    players[0] = new PlayerMouseAndKeys();
                }
                else
                    players[0] = new PlayerXbox(0);
                for (int i = 1; i < MaxPlayers; ++i)
                    players[i] = new PlayerXbox((Microsoft.Xna.Framework.PlayerIndex)i);
            }
        }


        public void Update(Scene scene)
        {
            if(scene.PlayerAgent != null)
            {
                _player = scene.PlayerAgent;
            }
            // If no player ship selected, try tabbing to one
            
            if (_player == null) // TODO: same for CoPlayer
            {
                if (refreshPlayerCooldown <= 0)
                {
                    scene.SwitchPlayerShip(ignoreMothership: true);
                }
            }
            else
            {
                if (scene.PlayerAgent == null && _player.IsActive)
                {
                    _player.SetControlType(AgentControlType.Player);
                    scene.PlayerAgent = _player;
                }
            }
            
            refreshPlayerCooldown--;

            foreach (var player in players)
            {
                player.CommandUpdate(scene);
            }
        }

        public static bool IsPlayerActive(Scene scene, int index)
        {
            if (index == 0)
                return true;
            if (scene.PlayersManager.MainPlayerControlType == ControlType.Xbox)
                return GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)index).IsConnected;
            return GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)index - 1).IsConnected;

        }

        public string GetCommandString(PlayerCommand command, int playerIndex = 0)
        {
            return players[playerIndex].GetCommandTag(command);
        }

        public IPlayerControl this[int i]
        {
            get
            {             
                return players[i];
            }         
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaUtils;
using SolarConflict.XnaUtils.Input;
using XnaUtils.Graphics;

namespace SolarConflict
{

    [Serializable]
    public class PlayerMouseAndKeys : IPlayerControl, IAgentControl
    {
        private PlayerCommand playerCommands, lastPlayerCommands;

        public int ID
        {
            get { return 255; }
            set { }
        }

        public PlayerMouseAndKeys()
        {
        }

        public string GetControlTag(ControlSignals controlSignals)
        {
            return KeysSettings.GetTag(controlSignals);
        }
        

        //public string GetTag(PlayerCommand command)
        //{
        //    return _GetTag(command);
        //}

        public ControlSignals UpdateAgent(int index, Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
        {
            InputState inputState = null;
            if(gameEngine.Scene!= null)
            {
                inputState = gameEngine.Scene.InputState;
            }
            else
            {
                inputState = InputState.EmptyState;
            }

            if (inputState == null)
            {
                inputState = InputState.EmptyState;
            }
            ControlSignals controlSignals = 0;


                controlSignals = 0;
                foreach (var item in KeysSettings.Data.KeyBindings)
                {
                    if (inputState.IsGKeyDown(item.Value))
                    {
                        controlSignals |= item.Key;
                    }
                }
           
           Vector2 relPos = gameEngine.Camera.GetWorldPos(inputState.Cursor.Position) - agent.Position;
            if (relPos != Vector2.Zero)
                relPos.Normalize();

            if((controlSignals & ControlSignals.MoveToCursor) > 0)
            foreach (var slot in agent.ItemSlotsContainer)
            {
                if (slot.Item != null && slot.Item.SlotType == SlotType.Engine)
                {
                    float value = Vector2.Dot(FMath.RotateVector(-FMath.ToCartesian(1, slot.rotation), agent.Heading), relPos);
                    if ( FMath.Bern(value * 1.42f , FMath.Rand))
                    {
                        controlSignals |= slot.ActivationSignal;
                    }
                    else
                    {
                        //if(slot.ActivationSignal != ControlSignals.Up && slot.ActivationSignal != ControlSignals.Down)
                        controlSignals &= ~slot.ActivationSignal;
                    }
                }
            }


           if (GameplaySettings.DirectionalControl)
           {


                Vector2 keysPos = Vector2.Zero;
                float rad = 1;
                if (inputState.IsGKeyDown(KeysSettings.Data.KeyBindings[ControlSignals.Up]))
                {
                    keysPos -= Vector2.UnitY * rad;
                }
                if (inputState.IsGKeyDown(KeysSettings.Data.KeyBindings[ControlSignals.Down]))
                {
                    keysPos += Vector2.UnitY * rad;
                }
                if (inputState.IsGKeyDown(KeysSettings.Data.KeyBindings[ControlSignals.Left]))
                {
                    keysPos -= Vector2.UnitX * rad;
                }
                if (inputState.IsGKeyDown(KeysSettings.Data.KeyBindings[ControlSignals.Right]))
                {
                    keysPos += Vector2.UnitX * rad;
                }
                if (keysPos != Vector2.Zero)
                {
                   // relPos = keysPos;
                    //if (Vector2.Dot(agent.Heading, relPos) > 0.9f)
                    //{
                    //    controlSignals |= ControlSignals.Up;
                    //}

                    foreach (var slot in agent.ItemSlotsContainer)
                    {
                        if(slot.Item != null && slot.Item.SlotType == SlotType.Engine)
                        {
                            if(Vector2.Dot(FMath.RotateVector(-FMath.ToCartesian(1, slot.rotation), agent.Heading), keysPos) > 0.7f )
                            {
                                controlSignals |= slot.ActivationSignal;
                            }
                            else
                            {
                                //if(slot.ActivationSignal != ControlSignals.Up && slot.ActivationSignal != ControlSignals.Down)
                                    controlSignals &= ~slot.ActivationSignal;
                            }
                        }
                    }

                }
                //controlSignals &= ~ControlSignals.Left;
                //controlSignals &= ~ControlSignals.Right;
                //controlSignals &= ~ControlSignals.Down;
               

            }

                if (inputState.IsGKeyDown(KeysSettings.Data.LockRotation))
                    analogDirections[0] = Vector2.Zero;
                else
                    analogDirections[0] = relPos; //if(Shift not pressed)                    


                analogDirections[1] = relPos; // Guns
            

            if(inputState.IsKeyPressed(KeysSettings.Data.KeyBindings[ControlSignals.Brake].Key))
            {
                controlSignals |= ControlSignals.Brake;
            }           
            return controlSignals;
        }

        public void CommandUpdate(Scene scene) //this is for player commands
        {
            lastPlayerCommands = playerCommands;
            playerCommands = 0;

            foreach (var commandKey in KeysSettings.Data.CommandBindings)
            {
                if (scene.InputState.IsGKeyDown(commandKey.Value)) //change
                {
                    playerCommands |= commandKey.Key;
                }
            }
        }

        public bool IsCommandOn(PlayerCommand command)
        {
            return (playerCommands & command) != PlayerCommand.None;
        }

        public bool IsCommandClicked(PlayerCommand command)
        {
            return ((playerCommands & command) != 0) && ((lastPlayerCommands & command) == 0);
        }

        public string GetCommandTag(PlayerCommand command)
        {
            return KeysSettings.GetCommandString(command);
        }

        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
        {
            return UpdateAgent(0, agent, gameEngine,ref analogDirections);
        }

        public IAgentControl GetWorkingCopy()
        {
            return this;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Scenes.HudEngine
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]    
    public class AnnouncerEffects: IHudComponent
    {
        const int multiKillTime = 120;

        IEmitter[] multiKillEmitters;
        int lasetKills, kills;
        int timeSinceKill;
        int multiKillCounter = 0;

        //can be changed to game object
        Agent lastAgent;




        public AnnouncerEffects()
        {
            multiKillEmitters = new IEmitter[8];
            lastAgent = null;
            multiKillEmitters[0] = new SoundEmitter((string)null, 0); //remove
            multiKillEmitters[1] = new SoundEmitter("kill2", 1);
            multiKillEmitters[2] = new SoundEmitter("kill3", 1);
            multiKillEmitters[3] = new SoundEmitter("kill4", 1);
            multiKillEmitters[4] = new SoundEmitter("kill5", 1);
            multiKillEmitters[5] = new SoundEmitter("kill6", 1);
            multiKillEmitters[6] = new SoundEmitter("kill7", 1);
            multiKillEmitters[7] = new SoundEmitter("kill8", 1);
        }

        

        private void OnKill(Agent agent, GameEngine gameEngine)
        {
            /*  if (kills == 1)
                  SoundEngine.AddSoundToQue(SoundEngine.GetSoundEffect("firstBlood"),1);*/

            if (timeSinceKill > multiKillTime)
                multiKillCounter = 0;
            multiKillCounter += kills - lasetKills;

            int effectIndex = Math.Min(multiKillCounter - 1, multiKillEmitters.Length - 1);
            multiKillEmitters[effectIndex].Emit(gameEngine, null, 0, agent.Position, Vector2.Zero, 0);
            timeSinceKill = 0;
        }

        public void Update(Scene scene, Agent agent)
        {
            if (agent != null)
            {
                GameEngine gameEngine = scene.GameEngine;
                if (agent != lastAgent)
                {
                    kills = (int)agent.GetMeterValue(MeterType.Kills);
                    lasetKills = kills;
                    lastAgent = agent;
                }
                timeSinceKill++;
                lasetKills = kills;
                kills = (int)agent.GetMeterValue(MeterType.Kills);
                if (kills > lasetKills)
                    OnKill(agent, gameEngine);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            
        }

        public Rectangle GetSize() { return new Rectangle(); }
    }
}

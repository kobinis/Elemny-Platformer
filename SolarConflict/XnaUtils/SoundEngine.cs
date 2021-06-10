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

namespace XnaUtils
{

    public struct SoundInst
    {
        public float volume;
        public float pan;
        //public float pitch;
        public SoundEffect soundEffect; 
        //Flags
        //int ID;

        public SoundInst(SoundEffect effect, float volume, float pan)
        {
            this.soundEffect = effect;
            this.volume = volume;
            this.pan = pan;
        }
    }

    public class SoundEngine
    {        
        const int MAX_SOUNDS = 30;
        public float maxRange = 6500; //                

        SoundInst[] playQue = new SoundInst[MAX_SOUNDS];
        int numOfEffects = 0;        

        public SoundEngine()
        {                        
        }

        public void AddSoundToQue(Camera camera, SoundEffect effect, Vector2 worldPosition, float volume)
        {
            if (camera != null)
            {
                worldPosition = worldPosition - camera.Position;
                float range = Math.Max(Math.Abs(worldPosition.X), Math.Abs(worldPosition.Y));
                if (numOfEffects < MAX_SOUNDS && effect != null && range < maxRange)
                {
                    SoundInst soundInst = new SoundInst(effect, (1 - range / maxRange) * volume, MathHelper.Clamp(worldPosition.X / maxRange * 2f, -1, 1));
                    playQue[numOfEffects] = soundInst;
                    numOfEffects++;
                }
            }
        }

        public void AddSoundToQue(string effectID, float volume) //Maybe change to play
        {
            AddSoundToQue(AudioBank.Inst.GetSound(effectID), volume);
        }

        public void AddSoundToQue(SoundInst soundInst)
        {

        }

        public void AddSoundToQue(SoundEffect effect, float volume)
        {
            if (numOfEffects < MAX_SOUNDS && effect != null)
            {
                playQue[numOfEffects].soundEffect = effect;
                playQue[numOfEffects].volume = volume;
                playQue[numOfEffects].pan = 0;
                numOfEffects++;
            }
        }

        public void Update(float effectVolume)
        {            
            for (int i = 0; i < numOfEffects; i++)
            {
                playQue[i].soundEffect.Play(effectVolume * playQue[i].volume, 0, playQue[i].pan);
            }
            numOfEffects = 0;
        }        
    }
}

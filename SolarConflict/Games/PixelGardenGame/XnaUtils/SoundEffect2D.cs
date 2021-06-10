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

namespace ThrustTest
{
    /*
    class SoundEffect2D
    {
        private SoundEffect soundEffect;
        private float volume,pitch;

        private float instPan, instVolume;



        public SoundEffect2D(SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect;
            volume = 1;
            pitch = 0;
        }

        public SoundEffect2D(SoundEffect soundEffect, float volume, float pitch)
        {
            this.soundEffect = soundEffect;            
            this.volume = volume;
            this.pitch = pitch;            
        }

        public void SetInst(float iPan, float iVolume)
        {
            this.volume = MathHelper.Clamp(volume, 0, 1);
            this.instPan = MathHelper.Clamp(iPan, -1, 1);
        }

        public void Play(float globalVolume)
        {
            soundEffect.Play(globalVolume * volume * instVolume, pitch, instPan);
        }

        
        
    }*/
}

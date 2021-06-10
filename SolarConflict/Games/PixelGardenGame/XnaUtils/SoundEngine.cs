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

namespace PaintPlay
{

    struct SoundInst
    {
        public float volume;
        public float pan;
        public SoundEffect soundEffect;
        //public float pitch;
    }

    static class SoundEngine //make a singelton ?
    {

        const int maxSounds = 40;

        public static Dictionary<String, Song> songs = new Dictionary<string, Song>(10);

        static Dictionary<String, SoundEffect> soundEffects = new Dictionary<string,SoundEffect>(10);

        static SoundInst[] playQue = new SoundInst[maxSounds];                

        static float maxRange = 1300;
        static float volume = 1f;

        static int numOfEffects = 0;

        static int time = 0;

           

        public static void AddSoundEffect(String key, SoundEffect effect)
        {
            soundEffects.Add(key, effect);
        }

        public static SoundEffect GetSoundEffect(String key)
        {
            return new SoundEffect(new byte[10], 8000, AudioChannels.Mono);
        }

        public static void AddSoundToQue(SoundEffect effect, Vector2 relPosition)
        {            
            if (numOfEffects<maxSounds && soundEffects != null && Math.Max(Math.Abs(relPosition.X),Math.Abs(relPosition.Y))<maxRange)
            {
                playQue[numOfEffects].soundEffect = effect;
                playQue[numOfEffects].volume = 1 - Math.Max(Math.Abs(relPosition.X), Math.Abs(relPosition.Y)) / maxRange;
                playQue[numOfEffects].pan = relPosition.X / maxRange;
                numOfEffects++;
            }                       
        }

        public static void AddSoundToQue(SoundEffect effect)
        {
            if (numOfEffects < maxSounds && soundEffects != null )
            {
                playQue[numOfEffects].soundEffect = effect;
                playQue[numOfEffects].volume = 1;
                playQue[numOfEffects].pan = 0;
                numOfEffects++;
            }
        }

        public static void Updade()
        {
         
            //if volume>0
            for (int i = 0; i < numOfEffects; i++)
            {
                playQue[i].soundEffect.Play(volume * playQue[i].volume, 0, playQue[i].pan);
            }           
            numOfEffects = 0;

        }

        public static void PlaySong(int index)
        {
           // MediaPlayer.Play(songs["s" + index.ToString()]);
        }
    }
}

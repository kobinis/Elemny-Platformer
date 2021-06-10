//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using XnaUtils;

//namespace SolarConflict
//{

//    public struct SoundInst
//    {
//        public float volume;
//        public float pan;
//        public SoundEffect soundEffect;
//        //public float pitch;
//    }

//    public static class SoundEngine 
//    {        
//        const int maxSounds = 30;
        
//        static Dictionary<String, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>(50);

//        static SoundInst[] playQue = new SoundInst[maxSounds];

//        static float maxRange = 2500 ; //
//        static float volume = 0.3f;
//        static float effectVolume = 0.3f;//0.1f;

//        static int numOfEffects = 0;



//        public static void AddSoundEffect(String key, SoundEffect effect)
//        {
//            soundEffects.Add(key, effect);
//        }

//        public static SoundEffect GetSoundEffect(String key)
//        {
//            return soundEffects[key];
//        }

//        public static void AddSoundToQue(Camera camera, SoundEffect effect, Vector2 worldPosition, float volume)
//        {
//            if (camera != null)
//            {
//                worldPosition = worldPosition - camera.Position;
//                float range = Math.Max(Math.Abs(worldPosition.X), Math.Abs(worldPosition.Y));
//                if (numOfEffects < maxSounds && effect != null && range < maxRange)
//                {
//                    playQue[numOfEffects].soundEffect = effect;
//                    playQue[numOfEffects].volume = (1 - range / maxRange) * volume * effectVolume;
//                    playQue[numOfEffects].pan = MathHelper.Clamp(worldPosition.X / maxRange * 2f, -1, 1);
//                    numOfEffects++;
//                }
//            }
//        }

//        public static void AddSoundToQue(string effect, float volume)
//        {
//            AddSoundToQue(soundEffects[effect], volume);
//        }

//        public static void AddSoundToQue(SoundEffect effect, float volume)
//        {
//            if (numOfEffects < maxSounds && effect != null)
//            {
//                playQue[numOfEffects].soundEffect = effect;
//                playQue[numOfEffects].volume = volume;
//                playQue[numOfEffects].pan = 0;
//                numOfEffects++;
//            }
//        }

//        public static void Updade()
//        {
//            for (int i = 0; i < numOfEffects; i++)
//            {
//                playQue[i].soundEffect.Play(volume * playQue[i].volume, 0, playQue[i].pan);
//            }
//            numOfEffects = 0;
//        }

//        public static void Dispose()
//        {
            
//            //playQue.c
//            foreach (var item in soundEffects)
//            {
//                item.Value.Dispose();
//            }
            
//        }
//    }
//}

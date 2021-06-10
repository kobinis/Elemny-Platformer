using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using SolarConflict;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;

namespace XnaUtils
{
    public enum MusicMode { Stop, Playing, Transition }

    public class MusicEngine
    {        
        public const int MENU_SONG = 2;
        public const int PROLOGUE_BEGINING_SONG = 4;
        public const int STARTING_NODE_BEGINING_SONG = 0;

        private static MusicEngine instance = null;

        public List<Song> Songs;
        public List<Song> SongsNotToPlayRandomly;

        public float CurrentVolume { get; set; }
        public float TargetVolume { get; set; }        
        public bool IsPlayNewSong { get; set; }

        private int currentSongIndex;
        private int targetSongIndex;

        private int timePlayingSong;

        public static MusicEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    //throw new Exception("You Must Run Init First");
                    instance = new MusicEngine();
                }
                return instance;
            }
        }

        private MusicEngine()
        {
            IsPlayNewSong = false;
           // preferences = SettingsManager.Inst.Preferences;
            if (instance == null)
            {
                instance = this;
            }
            else
            { 
                throw new Exception("You can't create this object twice");
            }

            Songs = new List<Song>();
            SongsNotToPlayRandomly = new List<Song>();
            CurrentVolume = 0;// 0.5f;
            TargetVolume = 0f;// 0.5f;
            currentSongIndex = 0;
            CurrentVolume = 0.5f;            
        }

        public void PlaySong(int index)
        {
            
            IsPlayNewSong = true;

            if (index < Songs.Count)
            {
                targetSongIndex = index;
            }
            timePlayingSong = 0;
            TargetVolume = 0;
        }

        public void PlayRandomSong()
        {
            int songIndex = FMath.Rand.Next(Songs.Count);            
            PlaySong(songIndex);
        }

        public void Pause()
        {
            try
            {
                MediaPlayer.Pause();
            }
            catch (Exception)
            {                
            }
            
        }

        public void Stop()
        {
            try
            {
                MediaPlayer.Stop();
            }
            catch (Exception)
            {                
            }
            
        }

        public void StopFade()
        {
            TargetVolume = 0;
        }

        public void Update(GameTime gameTime, float MasterVolume)
        {
            try
            {
                timePlayingSong++;                

                if (CurrentVolume <= 0 || MediaPlayer.State == MediaState.Stopped || timePlayingSong > 60 * 60 * 10)
                {
                    timePlayingSong = 0;
                    if (IsPlayNewSong)// currentSongIndex != targetSongIndex)
                    {
                        IsPlayNewSong = false;
                        TargetVolume = 1;
                        currentSongIndex = targetSongIndex;
                        if (currentSongIndex < Songs.Count)
                        {                            
                            MediaPlayer.Play(Songs[currentSongIndex]);
                        }
                    }
                    else
                    {
                        PlayRandomSong();
                    }
                }
                float deltaVolume = 0.1f; //delta fade in// delta fade out
                if (Math.Abs(TargetVolume - CurrentVolume) > deltaVolume)
                {
                    CurrentVolume += deltaVolume * Math.Sign(TargetVolume - CurrentVolume);
                }
                else
                {
                    CurrentVolume = TargetVolume;
                }

                MediaPlayer.Volume = CurrentVolume * MasterVolume * 0.5f; //* master volume
            }
            catch (Exception)
            {                
            }
        }
    }
}
    
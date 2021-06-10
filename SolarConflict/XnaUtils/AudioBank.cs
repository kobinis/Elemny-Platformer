using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections;
using Microsoft.Xna.Framework.Audio;
using SolarConflict.Framework;
using Microsoft.Xna.Framework.Content;

namespace XnaUtils
{
    public class AudioBank
    {                
        private readonly string[] SUPPORTED_TYPES = { ".wav" };
        private static AudioBank bank = null;
        private Dictionary<string, SoundEffect> _soundEffects;

        public static AudioBank Inst
        {
            get
            {
                if (bank == null)
                {
                    bank = new AudioBank();
                }
                return bank;
            }
        }

        public Dictionary<string, SoundEffect> GetSoundDictionary()
        {
            return _soundEffects;
        }

        private AudioBank()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void AddSound(string id, SoundEffect sound)
        {
            _soundEffects.Add(id.ToLower(), sound);            
        }

        public SoundEffect GetSound(string id)
        {
            if (id == null)
                return null;
            id = id.ToLower();

            if (_soundEffects.ContainsKey(id))
                return _soundEffects[id];
            else
                throw new Exception("SoundEffect: " + id + " not found!");
        }

        public void LoadContent(ContentManager manager, string path, bool ignoreDuplication = false)
        {
            string contentPath = "content\\";
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // Console.WriteLine(file);
                string fileExtension = Path.GetExtension(filePath).ToLower();
                string id = Path.GetFileNameWithoutExtension(filePath).ToLower();
                string assetName = Path.ChangeExtension(filePath, null).Substring(contentPath.Length);
                if (ignoreDuplication && _soundEffects.ContainsKey(id))
                    continue;
                AddSound(id, manager.Load<SoundEffect>(assetName));
            }
        }


        public void LoadSounds(string path)
        {
            
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // Console.WriteLine(file);
                string fileExtension = Path.GetExtension(filePath).ToLower();

                if (StringUtils.IsOneOf(fileExtension, SUPPORTED_TYPES))
                {
                    string filename = Path.GetFileNameWithoutExtension(filePath);
                    SoundEffect sound = LoadSound(filePath);

                    string id = Path.GetFileNameWithoutExtension(filePath);
                    // SoundEffect sound = DefalutSound;

                    AddSound(id, sound);
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in _soundEffects)
            {
                item.Value?.Dispose();
            }
            _soundEffects.Clear();
        }

        public static SoundEffect LoadSound(string path)
        {
            try
            {
                Stream stream = File.OpenRead(path);
                SoundEffect sound = SoundEffect.FromStream(stream);
                stream.Close();
                return sound;
            }
            catch (Exception)
            {
                return null;
            }
          
          
            /*Sprite fileTexture;
            using (FileStream fileStream = new FileStream(@"C:\Images\Box.png", FileMode.Open))
            {
                fileTexture = Sprite.FromStream(GraphicsDevice, fileStream);
            }*/
        }

    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using System.IO;
//using System.Collections;
//using Microsoft.Xna.Framework.Audio;
//using SolarConflict.Framework;
//using XnaUtils;

//namespace XnaUtils
//{
//    public class SoundBank
//    {
//        public bool LazyLoad = true;
//        public SoundEffect DefalutSound;

//        private readonly string[] SUPPORTED_TYPES = { ".wav" };
//        private static SoundBank bank = null;
//        private Dictionary<string, SoundEffect> _soundEffects;

//        public static SoundBank Inst
//        {
//            get
//            {
//                if (bank == null)
//                {
//                    bank = new SoundBank();
//                }
//                return bank;
//            }
//        }

//        private SoundBank()
//        {
//            _soundEffects = new Dictionary<string, SoundEffect>();
//        }

//        public void AddSound(string id, SoundEffect sound)
//        {
//            _soundEffects.Add(id.ToLower(), sound);            
//        }

//        public SoundEffect GetSound(string id)
//        {
//            if (id == null)
//                return null;
//            id = id.ToLower();

//            if (_soundEffects.ContainsKey(id))
//                return _soundEffects[id];
//            else
//                throw new Exception("SoundEffect: " + id + " not found!");
//        }


//        public void LoadSounds(string path)
//        {
//            //try
//            //{

//            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
//            foreach (string filePath in files)
//            {
//                // Console.WriteLine(file);
//                string fileExtension = Path.GetExtension(filePath).ToLower();

//                if (StringUtils.IsOneOf(fileExtension, SUPPORTED_TYPES))
//                {
//                    string filename = Path.GetFileNameWithoutExtension(filePath);
//                    SoundEffect sound = LoadSound(filePath);

//                    string id = Path.GetFileNameWithoutExtension(filePath);
//                    // SoundEffect sound = DefalutSound;

//                    AddSound(id, sound);
//                }
//            }
//        }

//        public void Dispose()
//        {
//            foreach (var item in _soundEffects)
//            {
//                item.Value.Dispose();
//            }
//            _soundEffects.Clear();

//        }

//        public static SoundEffect LoadSound(string path)
//        {
//            Stream stream = File.OpenRead(path);
//            SoundEffect sound = SoundEffect.FromStream(stream);
//            stream.Close();
//            return sound;
//            /*Sprite fileTexture;
//            using (FileStream fileStream = new FileStream(@"C:\Images\Box.png", FileMode.Open))
//            {
//                fileTexture = Sprite.FromStream(GraphicsDevice, fileStream);
//            }*/
//        }

//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections;
using SolarConflict.Framework;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using SolarConflict;
using Microsoft.Xna.Framework.Content;

namespace XnaUtils.Graphics
{
    public class TextureBank
    {
        private readonly string[] supportedFileTypes = new string[3] { ".png", ".jpg", ".jpeg" };

        private static TextureBank bank = null;

        public static TextureBank Inst
        {
            get
            {
                if (bank == null)
                {
                    bank = new TextureBank();
                }
                return bank;
            }
        }

        private Dictionary<string, Texture2D> _textureDictionary;
        private Dictionary<string, Sprite> _spriteDictionary;
        private bool _lazyLoading = false;
        Dictionary<string, string> _texturePathFromID;

#if DEBUG
        private List<string> _usedTextures;
#endif

        private TextureBank()
        {
            _textureDictionary = new Dictionary<string, Texture2D>();
            _spriteDictionary = new Dictionary<string, Sprite>();
            _texturePathFromID = new Dictionary<string, string>();
    }

    public void AddTexture(string id, Texture2D texture, bool overwrite = false)
        {
            if (texture == null && !_lazyLoading)
                throw new Exception();
            id = id.ToLower();
            if(texture != null)
                texture.Name = id;    
            _textureDictionary[id] = texture;
        }

        public void AddSprite(Sprite sprite)
        {
            _spriteDictionary.Add(sprite.ID, sprite);
        }

        public void SetSprite(Sprite sprite)
        {
            _spriteDictionary[sprite.ID] = sprite;
        }


        public Texture2D GetTexture(string id)
        {

            if (id == null)
                return null;
            id = id.ToLower();


            if (_textureDictionary.ContainsKey(id))
            {
                if (_lazyLoading)
                {
                    if (_textureDictionary[id] == null)
                    {
                        Texture2D texture = LoadTexture(ActivityManager.GraphicsDevice, _texturePathFromID[id]);
                        AddTexture(id, texture);
                    }
                    return _textureDictionary[id];
                }
                else
                    return _textureDictionary[id];
            }
            else
            {
               // if (DebugUtils.Mode == ModeType.Release)
                    return Sprite.Get("missing");
            //    else
                //    throw new Exception($"Texture {id} was not found!");
            }
        }

        public Texture2D TryGetTexture(string id)
        {
            if (id == null)
                return null;
            id = id.ToLower();


            if (_textureDictionary.ContainsKey(id))
            {
                if (_lazyLoading)
                {
                    if (_textureDictionary[id] == null)
                    {
                        Texture2D texture = LoadTexture(ActivityManager.GraphicsDevice, _texturePathFromID[id]);
                        AddTexture(id, texture);
                    }
                    return _textureDictionary[id];
                }
                else
                    return _textureDictionary[id];
            }
            else
                return null;
        }

        public Sprite GetSprite(string id)
        {
            if (id == null)
                return null;
            id = id.ToLower();
            Sprite sprite = null;
            if (!_spriteDictionary.TryGetValue(id, out sprite))
            {
                if (_textureDictionary.ContainsKey(id))
                {
                    sprite = new Sprite(id);
                    _spriteDictionary.Add(id, sprite);
                }
                else
                {
                    if(DebugUtils.Mode != ModeType.Test)
                        return Sprite.Get("missing");
                    else
                        throw new Exception($"Texture {id} was not found!");
                }
                //throw new Exception("No Texture or sprite named " + id + " not found!");
            }
            return sprite;
        }

        public void LoadTextures(GraphicsDevice graphicsDevice, string path, bool ignoreTextureDuplication = false)
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {
                // Console.WriteLine(file);
                string fileExtension = Path.GetExtension(filePath).ToLower();
                if (StringUtils.IsOneOf(fileExtension, supportedFileTypes))
                {
                    string id = Path.GetFileNameWithoutExtension(filePath).ToLower();
                    if (ignoreTextureDuplication && _textureDictionary.ContainsKey(id))
                        continue;

                    //string filename = Path.GetFileNameWithoutExtension(filePath);

                    if (_lazyLoading)
                    {
                        AddTexture(id, null);
                        _texturePathFromID.Add(id, filePath);
                    }
                    else
                    {
                        Texture2D texture = LoadTexture(graphicsDevice, filePath);
                        AddTexture(id, texture);
                    }
                }
            }
        }

        public void LoadContent(ContentManager manager, string path, bool ignoreTextureDuplication = false)
        {
            string contentPath = "content\\";
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string filePath in files)
            {                
                // Console.WriteLine(file);
                string fileExtension = Path.GetExtension(filePath).ToLower();
                    string id = Path.GetFileNameWithoutExtension(filePath).ToLower().Trim();
                    string assetName =  Path.ChangeExtension(filePath, null).Substring(contentPath.Length);
                    if (ignoreTextureDuplication && _textureDictionary.ContainsKey(id))
                        continue;
                    Texture2D texture = manager.Load<Texture2D>(assetName);
                    AddTexture(id, texture);                
            }
        }

        public void Dispose()
        {
            foreach (var item in _textureDictionary)
                item.Value?.Dispose();
            _textureDictionary.Clear();
        }

        public static Texture2D LoadTexture(GraphicsDevice graphicsDevice, string path)
        {
            //File.Copy(path, Path.Combine(@"C:\Projects\UsedTextures", Path.GetFileName(path).ToLower()));
            using (Stream stream = File.OpenRead(path))
            {
                Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
                GraphicsUtils.PremultiplyAlpha(texture);
                return texture;
            }
        }

    }
}

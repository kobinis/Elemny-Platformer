using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;

namespace XnaUtils.Graphics {
    
    /// <summary>
    /// An immutable class that holds a texture
    /// </summary>
    [Serializable]    
    public class Sprite : ICloneable
    {
        public const string NORMALMAP_SUFFIX = "_normal";
        public const string ROUGHNESSMAP_SUFFIX = "_roughness";
        public const string EMISSION_SUFFIX = "_emission";

        public static explicit operator Sprite(string id)
        {
            return TextureBank.Inst.GetSprite(id);
        }

        public static implicit operator Texture2D(Sprite sprite)
        {
            return sprite._texture;
        }

        public static Sprite Get(string id) //TODO: check usages
        {
            return TextureBank.Inst.GetSprite(id);
        }

        protected string _id;
        protected string _normalMapID;
        protected string _roughnessMapID;
        protected string _emissionID;

        [NonSerialized]
        protected Texture2D _texture;
        [NonSerialized]
        protected Texture2D _normalMap;
        [NonSerialized]
        protected Texture2D _roughnessMap;
        [NonSerialized]
        protected Texture2D _emissionMap;

        public Texture2D NormalMap { get { return _normalMap; } }
        public Texture2D RoughnessMap { get { return _roughnessMap; } }
        public Texture2D EmissionMap { get { return _emissionMap; } }
        public int Height { get { return _texture.Height; } }
        public int Width { get { return _texture.Width; } }
        public Texture2D Texture { get { return _texture; } }
        public Vector2 Origin { get { return new Vector2(_texture.Width / 2f, _texture.Height / 2f); } }

        public string ID
        {
            get { return _id; }
        }

        public Vector2 Size { get { return new Vector2(_texture.Width, _texture.Height); } }
        
        public Sprite(string id, string normalID = null)
        {
            _id = id.ToLower();
           // _normalMapID = normalID?.ToLower();

            Init();
        }

        protected void Init()
        {
            Debug.Assert(_id != null, "ID can't be null when serializing or initilazing via ID");
            _texture = TextureBank.Inst.GetTexture(_id);
            var normalId = _normalMapID == null ? _id + NORMALMAP_SUFFIX : _normalMapID;
            _normalMap =TextureBank.Inst.TryGetTexture(normalId);

            var roughnessId = _roughnessMapID == null ? _id + ROUGHNESSMAP_SUFFIX : _roughnessMapID;
            _roughnessMap = TextureBank.Inst.TryGetTexture(roughnessId);

            
            var emissionId = _emissionID == null ? _id + EMISSION_SUFFIX : _emissionID;
            _emissionMap = TextureBank.Inst.TryGetTexture(emissionId);
        }

        public object Clone()
        {
            return this;
        }

        public virtual void Draw(SpriteBatch batch, Rectangle destination, Color color)
        {
            batch.Draw(_texture, destination, color);
        }

        //public virtual void Draw(SpriteBatch sb, Vector2 position, float rotation, float scale, Color color)
        //{
        //    sb.Draw(_texture, position,  null, color, rotation, Origin, scale, SpriteEffects.None, 0);
        //}

        /// <returns>The size of the icon drawn</returns> //Refactor this function
        public static Vector2 DrawIcon(SpriteBatch sb, Rectangle rectangle, Color color, bool centered, float scalingFactor, params Sprite[] sprites) {
            var result = new Vector2(0, 0);
            foreach (var sprite in sprites) {
                if (sprite == null)
                    continue;

                var size = FMath.FitSize(new Vector2(sprite.Width, sprite.Height), new Vector2(rectangle.Width * scalingFactor, rectangle.Height * scalingFactor));

                result = new Vector2(Math.Max(result.X, size.X), Math.Max(result.Y, size.Y));

                var rect = centered ? new Rectangle(rectangle.X, rectangle.Y, (int)size.X, (int)size.Y) :
                    new Rectangle(rectangle.X + ((int)size.X / 2), rectangle.Y + ((int)size.Y / 2), (int)size.X, (int)size.Y);

                sb.Draw(sprite.Texture, rect, null, color, 0, new Vector2(sprite.Width / 2, sprite.Height / 2), SpriteEffects.None, 0);
            }

            return result;
        }

        public static string ToTag(string id)
        {
            return $"#image{{{id}}}";
        }

        /// <summary>Convert to a tag for our text formatting system</summary>            
        public string ToTag() {
            return $"#image{{{ID}}}";
        }

        public string ToTag(Color color)
        {
            return $"#image{{{color.R},{color.G},{color.B},{ID}}}";
        }

        public bool HasNormalMap
        {
            get { return _normalMap != null; }            
        }

        public bool HasRoughnessMap
        {
            get { return _roughnessMap != null; }
        }

        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context)
        {
            Init();
        }
    }            
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Graphics;
using System.Linq;
using System.Runtime.Serialization;
using SolarConflict;
using SolarConflict.XnaUtils;

namespace XnaUtils {
    //TODO: refactor
    [Serializable]
    public class Camera
    {

        public static void Init(Effect normalEffect, Effect backgroundEffect)
        {

            NormalMapEffect = normalEffect;

            Camera.NormalEffectRotation = Camera.NormalMapEffect.Parameters["Rotation"];
            Camera.NormalEffectNormal = Camera.NormalMapEffect.Parameters["NormalMap"];
            Camera.NormalEffectRoughness = Camera.NormalMapEffect.Parameters["RoughnessMap"];
            Camera.NormalEffectEmission = Camera.NormalMapEffect.Parameters["EmissionMap"];

            Camera.NormalEffectNumLights = Camera.NormalMapEffect.Parameters["NumLights"];

            Camera.NormalTechniqueWithLights = Camera.NormalMapEffect.Techniques["Normalmap"];
            Camera.NormalTechniqueBasicSprite = Camera.NormalMapEffect.Techniques["BasicSprite"];
            Camera.NormalTechniqueFlat = Camera.NormalMapEffect.Techniques["Flatlighting"];

            BackgroundEffect = backgroundEffect;
            Camera.BackgroundEffectViewport = Camera.BackgroundEffect.Parameters["Viewport"];
            Camera.BackgroundEffectSourceUV = Camera.BackgroundEffect.Parameters["SourceUV"];
        }

        public static Vector3 AmbientColor;

        public static Effect NormalMapEffect;
        public static Effect BackgroundEffect;

        public static EffectParameter NormalEffectRotation;
        public static EffectParameter NormalEffectNormal;
        public static EffectParameter NormalEffectRoughness;
        public static EffectParameter NormalEffectEmission;



        public static EffectParameter NormalEffectNumLights;
        public static EffectParameter FlatEffectNumLights;

        public static EffectTechnique NormalTechniqueBasicSprite;
        public static EffectTechnique NormalTechniqueWithLights;
        public static EffectTechnique NormalTechniqueFlat;

        public static EffectParameter BackgroundEffectViewport;
        public static EffectParameter BackgroundEffectSourceUV;


        public float Zoom;
        public Vector2 Position;
        [NonSerialized]
        private Rectangle Bounds;
        //[NonSerialized]
        //private Rectangle VisibleArea;
        [NonSerialized]
        public Matrix Transform;

        /// <summary>Camera position(center) in world coordinates.</summary>        

        public bool DrawLit = false;
        public List<GameObject> IncomingLightsObjects;

        [NonSerialized]
        public SpriteBatch SpriteBatch;
        [NonSerialized]
        private Texture2D defaultNormals;
        [NonSerialized]
        private Texture2D defaultRoughness;
        [NonSerialized]
        private Texture2D defaultEmission;


        public Camera()
        {            
            Position = Vector2.Zero;
            Zoom = 0.75f;
            
            Zoom = 1f;
            Position = Vector2.Zero;

            InitNonSerializable();
        }

        private void InitNonSerializable()
        {
            this.SpriteBatch = Game1.sb;
            defaultNormals = TextureBank.Inst.GetTexture("defaultnormals");
            defaultRoughness = TextureBank.Inst.GetTexture("defaultroughness");
            defaultEmission = TextureBank.Inst.GetTexture("defaultEmission");
            Bounds = ActivityManager.GraphicsDevice.Viewport.Bounds;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            InitNonSerializable();
        }

        public bool IsOnScreen(Vector2 worldPos, float size, float sizeMult = 1.5f)
        {
            int maxSize = (int)(size * sizeMult);
            Rectangle inflatedScreenSize = new Rectangle(0, 0, ActivityManager.ScreenWidth, ActivityManager.ScreenHeight);
            inflatedScreenSize.Inflate(maxSize, maxSize);
            Vector2 screenPos = ActivityManager.ScreenCenter + (worldPos - Position) * Zoom;
            return inflatedScreenSize.Contains((int)screenPos.X, (int)screenPos.Y);
        }

        public void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            //UpdateVisibleArea();
        }

        public Vector2 GetScreenPos(Vector2 worldPos)
        {
            return ActivityManager.ScreenCenter + (worldPos - Position) * Zoom;
        }

        public Vector2 GetWorldPos(Vector2 screenPos)
        {
            return (screenPos - ActivityManager.ScreenCenter) / Zoom + Position;
        }
        
        public void SetCameraPosToDrawPosOnScreenAt(Vector2 worldPos, Vector2 screenPos)
        {
            Position = (ActivityManager.ScreenCenter - screenPos) / Zoom + worldPos;
            UpdateMatrix();
        }
          
        public void CameraDraw(Texture2D texture, Vector2 pos, float rot, Vector2 size, Color col)
        {
         
                SpriteBatch.Draw(texture, pos, null, col, rot, new Vector2(texture.Width / 2f, texture.Height / 2f), size, SpriteEffects.None, 0);
        }

        public void CameraDraw(Texture2D texture, Vector2 pos, float rot, float size, Color col)
        {            
                SpriteBatch.Draw(texture, pos, null, col, rot, new Vector2(texture.Width / 2, texture.Height / 2), size, SpriteEffects.None, 0);            
        }        

        public void CameraDraw(Sprite sprite, Vector2 pos, float rot, float size, Color col, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            if (!DrawLit)
            {   
                NormalMapEffect.CurrentTechnique = NormalTechniqueBasicSprite;
                SpriteBatch.Draw(sprite.Texture, pos, null, col, rot, new Vector2(sprite.Width / 2, sprite.Height / 2), size, spriteEffects, 0);
            }
            else
            {
                NormalMapEffect.CurrentTechnique = NormalTechniqueWithLights;
                DrawWithLighting(sprite, SpriteBatch, pos, null, col, rot, new Vector2(sprite.Width / 2, sprite.Height / 2), size, spriteEffects, 0, Position, Zoom, ActivityManager.ScreenCenter);
            }
        }

        public void CameraDraw(Spritesheet sheet, int spriteIndex, Vector2 pos, float rot, float size, Color col, SpriteEffects effect )
        {

            var sourceRect = sheet.SourceRect(spriteIndex);
            var position = new Vector2(sheet.SpriteWidth / 2, sheet.SpriteHeight / 2);

            if (!DrawLit)
            {
                NormalMapEffect.CurrentTechnique = NormalTechniqueBasicSprite;
                SpriteBatch.Draw(sheet.Sheet, pos, sourceRect, col, rot, position, size, effect, 0);
            }
            else
            {
                NormalMapEffect.CurrentTechnique = NormalTechniqueWithLights;
                DrawWithLighting(sheet.Sheet, SpriteBatch, pos, sourceRect, col, rot, position, size, effect, 0, Position, Zoom, ActivityManager.ScreenCenter);
            }
        }

        public Vector2 DrawText(SpriteFont font, string text, Vector2 position, Color color, float minScale = 0.25f, float maxScale = 2)
        {
            float clampedZoom = MathHelper.Clamp(Zoom, minScale, maxScale); //TODO: change
            Vector2 size = font.MeasureString(text) * clampedZoom;
            SpriteBatch.DrawString(font, text, position - size * 0.5f, color);
            return Vector2.Zero;
        }

        

        ///// <param name="batch">A batch that isn't presently between Begin() and End() calls.</param>
        ///// <param name="effects">Presently disregarded.</param>
        ///// <param name="positionOfScreen">World position of screen.</param>
        ///// <warning>Presently disregards effects arg and just applies the normal map effect.</warning>
        void DrawWithLighting(Sprite sprite, SpriteBatch batch, Vector2 position, Rectangle? source, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, Vector2 positionOfScreen, float zoom, Vector2 halfResolution)
        {            
            var graphicsDevice = batch.GraphicsDevice;

            // Pass sprite-specific shader parameters
            Camera.NormalEffectRotation.SetValue(rotation);
            Camera.NormalEffectNormal.SetValue(sprite.NormalMap ?? defaultNormals);
            Camera.NormalEffectRoughness.SetValue(sprite.RoughnessMap ?? defaultRoughness);
            Camera.NormalEffectEmission.SetValue(sprite.EmissionMap ?? defaultEmission);

            //Camera.NormalMapEffect.Parameters["MatrixTransform"].SetValue(Transform);

            batch.Draw(sprite.Texture, position, source, color, rotation, origin, scale, effects, layerDepth); //scale
            //batch.End();
        }
   
        /// <summary>Passes our lighting-related state to our shaders etc.</summary>
        public void PrepareForLitDraw()
        {
            if (!GraphicsSettings.UseLighting)
                return;

            if (IncomingLightsObjects == null || IncomingLightsObjects.Count == 0)
            {
                Camera.NormalEffectNumLights.SetValue(0);                
                return;
            }
            
            // Pass lights etc to the shader   
            // Shader expects lights' world positions minus the world position of the screen's center
            var lightPositions = IncomingLightsObjects.Select(l => l.Position - Position).ToArray();            
            var modifiedColors = IncomingLightsObjects.Select(l => l.Light.BaseColor * l.Light.Intensity).ToArray(); //TODO: get color from gameobject
            var lightHotspots = IncomingLightsObjects.Select(l => l.Light.Hotspot).ToArray();
            var lightAttenuation = IncomingLightsObjects.Select(l => l.Light.Attenuation).ToArray();

            NormalMapEffect.Parameters["NumLights"].SetValue(IncomingLightsObjects.Count());
            NormalMapEffect.Parameters["LightColors"].SetValue(modifiedColors.ToArray());
            NormalMapEffect.Parameters["LightPositions"].SetValue(lightPositions);
            NormalMapEffect.Parameters["LightHotspots"].SetValue(lightHotspots);
            NormalMapEffect.Parameters["LightAttenuation"].SetValue(lightAttenuation);
        }
    }
}

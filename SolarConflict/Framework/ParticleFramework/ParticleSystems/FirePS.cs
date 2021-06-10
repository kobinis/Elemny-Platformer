#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace SolarConflict
{
    class FirePS : ParticleSystemAdv
    {
        float MinRotateSpeed;
        float MaxRotateSpeed;

        float MinStartSize = 2;
        float MaxStartSize = 3;

        float MinEndSize = 14;
        float MaxEndSize = 25;

        public FirePS(ContentManager content, GraphicsDevice graphicsDevice)
            : base(content, graphicsDevice)
        {
        }


        //shader parameter set
        protected override void InitializeSettings()
        {
            TextureName = "Images/SmokeSUB_A";

            MaxParticles = 10000;

            Duration = TimeSpan.FromSeconds(1.65f);
            DurationRandomness = 0.4f;

            MinHorizontalVelocity = 0.2f;
            MaxHorizontalVelocity = 0.2f;

            MinVerticalVelocity = 0.2f;
            MaxVerticalVelocity = 0.2f;

            EndVelocity = 0.2f;

            MinColor = Color.White;
            MaxColor = Color.White;

            MinRotateSpeed = 0;
            MaxRotateSpeed = 1;

            EmitterVelocitySensitivity = 1.0f;

            Blending = BlendState.AlphaBlend;
        }

        protected override void LoadParticleEffect()
        {
            Effect effect = content.Load<Effect>("Shaders/firePE");
            particleEffect = effect.Clone();

            EffectParameterCollection parameters = particleEffect.Parameters;

            // Look up shortcuts for parameters that change every frame.
            effectViewParameter = parameters["View"];
            effectProjectionParameter = parameters["Projection"];
            effectViewportScaleParameter = parameters["ViewportScale"];
            effectTimeParameter = parameters["CurrentTime"];

            parameters["Duration"].SetValue((float)Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(DurationRandomness);
            parameters["EndVelocity"].SetValue(EndVelocity);
            parameters["MinColor"].SetValue(MinColor.ToVector4());
            parameters["MaxColor"].SetValue(MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(MinRotateSpeed, MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(MinStartSize, MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(MinEndSize, MaxEndSize));


            Texture2D texture = content.Load<Texture2D>(TextureName);
            parameters["Texture"].SetValue(texture);

        }
    }
}

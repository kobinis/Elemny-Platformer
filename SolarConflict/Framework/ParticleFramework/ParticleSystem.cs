using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections.Concurrent;
using XnaUtils;

namespace SolarConflict
{
    public abstract class ParticleSystem
    {
        protected ContentManager content;
        protected Effect particleEffect;

        protected DynamicVertexBuffer vertexBuffer;
        protected IndexBuffer indexBuffer;

        private ParticleVertex[] particles;
        //protected ParticleVertex[] particles;

        protected BlendState Blending;


        protected float EmitterVelocitySensitivity;
        protected int MaxParticles;
        protected int firstActiveParticle;
        protected int firstNewParticle;
        protected int firstFreeParticle;
        protected int firstRetiredParticle;

        protected string TextureName = null;

        protected EffectParameter effectViewParameter;
        protected EffectParameter effectProjectionParameter;
        protected EffectParameter effectViewportScaleParameter;
        protected EffectParameter effectTimeParameter;

        protected float MinHorizontalVelocity;
        protected float MaxHorizontalVelocity;

        protected float MinVerticalVelocity;
        protected float MaxVerticalVelocity;

        protected float EndVelocity;

        protected Color MinColor;
        protected Color MaxColor;

        protected TimeSpan Duration;
        protected float DurationRandomness = 0;

        protected float currentTime;
        protected int drawCounter;

        public GraphicsDevice graphicsDeviceA;

        protected ParticleSystem(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.content = content;
            this.graphicsDeviceA = graphicsDevice;
            Initialize();
            LoadContent();

        }

        protected virtual void Initialize()
        {
            InitializeSettings();
            // Allocate the particle array, and fill in the corner fields (which never change).
            particles = new ParticleVertex[MaxParticles * 4];

            for (int i = 0; i < MaxParticles; i++)
            {
                particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                particles[i * 4 + 1].Corner = new Vector2(1, -1);
                particles[i * 4 + 2].Corner = new Vector2(1, 1);
                particles[i * 4 + 3].Corner = new Vector2(-1, 1);
            }
        }

        protected abstract void InitializeSettings();

        protected virtual void LoadContent()
        {
            LoadParticleEffect();

            // Create a dynamic vertex buffer.
            vertexBuffer = new DynamicVertexBuffer(graphicsDeviceA, ParticleVertex.VertexDeclaration,
                                                   MaxParticles * 4, BufferUsage.WriteOnly);
            // Create and populate the index buffer.
            ushort[] indices = new ushort[MaxParticles * 6];

            for (int i = 0; i < MaxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            indexBuffer = new IndexBuffer(graphicsDeviceA, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);
        }

        protected abstract void LoadParticleEffect();

        public void update(float elapsed)
        {
            //if (gameTime == null)
            //    throw new ArgumentNullException("gameTime");//float isn't a nullable type so gameTime can't hold the value null

            currentTime += elapsed;//(float)gameTime; //.ElapsedGameTime.TotalSeconds //+=

            RetireActiveParticles();
            FreeRetiredParticles();

            // If we let our timer go on increasing for ever, it would eventually
            // run out of floating point precision, at which point the particles
            // would render incorrectly. An easy way to prevent this is to notice
            // that the time value doesn't matter when no particles are being drawn,
            // so we can reset it back to zero any time the active queue is empty.

            if (firstActiveParticle == firstFreeParticle)
                currentTime = 0;

            if (firstRetiredParticle == firstActiveParticle)
                drawCounter = 0;
        }

        protected virtual void RetireActiveParticles()
        {
            float particleDuration = (float)Duration.TotalSeconds;

            while (firstActiveParticle != firstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = currentTime - particles[firstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                particles[firstActiveParticle * 4].Time = drawCounter;

                // Move the particle from the active to the retired queue.
                firstActiveParticle++;

                if (firstActiveParticle >= MaxParticles)
                    firstActiveParticle = 0;
            }
        }

        protected virtual void FreeRetiredParticles()
        {
            while (firstRetiredParticle != firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                // We multiply the retired particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                int age = drawCounter - (int)particles[firstRetiredParticle * 4].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                firstRetiredParticle++;

                if (firstRetiredParticle >= MaxParticles)
                    firstRetiredParticle = 0;
            }
        }

        public virtual void draw()
        {
            GraphicsDevice device = graphicsDeviceA;

            // Restore the vertex buffer contents if the graphics device was lost.
            if (vertexBuffer.IsContentLost)
            {
                vertexBuffer.SetData(particles);
            }

            // If there are any particles waiting in the newly added queue,
            // we'd better upload them to the GPU ready for drawing.
            if (firstNewParticle != firstFreeParticle)
            {
                AddNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (firstActiveParticle != firstFreeParticle)
            {
                device.BlendState = Blending;
                device.DepthStencilState = DepthStencilState.DepthRead;
                device.RasterizerState = RasterizerState.CullNone;

                // Set an effect parameter describing the viewport size. This is
                // needed to convert particle sizes into screen space point sizes.
                effectViewportScaleParameter.SetValue(new Vector2(0.5f / device.Viewport.AspectRatio, -0.5f)); //

                // Set an effect parameter describing the current time. All the vertex
                // shader particle animation is keyed off this value.
                effectTimeParameter.SetValue(currentTime);

                // Set the particle vertex and index buffer.
                device.SetVertexBuffer(vertexBuffer);
                device.Indices = indexBuffer;

                // Activate the particle effect.
                foreach (EffectPass pass in particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (firstActiveParticle < firstFreeParticle)
                    {
                        // If the active particles are all in one consecutive range,
                        // we can draw them all in a single call.
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     firstActiveParticle * 4, (firstFreeParticle - firstActiveParticle) * 4,
                                                     firstActiveParticle * 6, (firstFreeParticle - firstActiveParticle) * 2);
                    }
                    else
                    {
                        // If the active particle range wraps past the end of the queue
                        // back to the start, we must split them over two draw calls.
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     firstActiveParticle * 4, (MaxParticles - firstActiveParticle) * 4,
                                                     firstActiveParticle * 6, (MaxParticles - firstActiveParticle) * 2);

                        if (firstFreeParticle > 0)
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         0, firstFreeParticle * 4,
                                                         0, firstFreeParticle * 2);
                        }
                    }
                }

                // Reset some of the renderstates that we changed,
                // so as not to mess up any other subsequent drawing.
                device.DepthStencilState = DepthStencilState.Default;
            }

            drawCounter++;
        }

        protected virtual void AddNewParticlesToVertexBuffer()
        {
            int stride = ParticleVertex.SizeInBytes;

            if (firstNewParticle < firstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                vertexBuffer.SetData(firstNewParticle * stride * 4, particles,
                                     firstNewParticle * 4,
                                     (firstFreeParticle - firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                vertexBuffer.SetData(firstNewParticle * stride * 4, particles,
                                     firstNewParticle * 4,
                                     (MaxParticles - firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (firstFreeParticle > 0)
                {
                    vertexBuffer.SetData(0, particles,
                                         0, firstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            firstNewParticle = firstFreeParticle;
        }

        public virtual void scheduleParticle(Vector3 position, Vector3 velocity, Vector3 color, float parameterA, float parameterB)
        {
        }

        public void setCamera(Matrix view)
        {
            effectViewParameter.SetValue(view);
        }

        public void setProjection(Matrix projection)
        {
            effectProjectionParameter.SetValue(projection);
        }

        protected virtual void addParticle(ParticleVertexAdv particle)
        {

        }

        public virtual void addParticle(Vector3 position, Vector3 velocity, float parameterA, float parameterB)
        {
            addParticle(position, velocity);
        }


        public virtual void addParticle(Vector3 position, Vector3 velocity, Vector3 color, float parameterA, float parameterB)
        {

        }

        public virtual void addParticle(Vector3 position, Vector2 size, float rotation, float parameter)
        {

        }
        /*
        public virtual void addParticle(Vector3 position, Vector3 color, float parameterA, float parameterB, float initialSize, float finalSize, float durationMulti)
        {

        }
        */
        public void addParticle(Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = firstFreeParticle + 1;

            if (nextFreeParticle >= MaxParticles)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == firstRetiredParticle)
                return;

            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= EmitterVelocitySensitivity;

            // Add in some FMath.Rand amount of horizontal velocity.
            float horizontalVelocity = MathHelper.Lerp(MinHorizontalVelocity,
                                                       MaxHorizontalVelocity,
                                                       (float)FMath.Rand.NextDouble());

            double horizontalAngle = FMath.Rand.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            // Add in some FMath.Rand amount of vertical velocity.
            velocity.Y += MathHelper.Lerp(MinVerticalVelocity,
                                          MaxVerticalVelocity,
                                          (float)FMath.Rand.NextDouble());

            // Choose four FMath.Rand control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color((byte)FMath.Rand.Next(255),
                                           (byte)FMath.Rand.Next(255),
                                           (byte)FMath.Rand.Next(255),
                                           (byte)FMath.Rand.Next(255));

            // Fill in the particle vertex structure.
            for (int i = 0; i < 4; i++)
            {
                particles[firstFreeParticle * 4 + i].Position = position;
                particles[firstFreeParticle * 4 + i].Velocity = velocity;
                particles[firstFreeParticle * 4 + i].Random = randomValues;
                particles[firstFreeParticle * 4 + i].Time = currentTime;
            }

            firstFreeParticle = nextFreeParticle;
        }



    }
}


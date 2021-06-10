using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PaintPlay.XnaUtils
{
    class ParticleProfile
    {
        public float dSize;
        public float scale;
        public float maxLifetime;
        public Texture2D texture;
        public Vector2 origin;
        public ColorUpdater colorUpdater;

        public ParticleProfile()
        {
            scale = 1;
            maxLifetime = 1;
            dSize = 0.0f;            
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
            origin = new Vector2(texture.Width/2, texture.Height/2);
        }

        public Particle MakeParticle(Vector2 position, float size)
        {
            Particle particle = new Particle();
            particle.Init(this, position, size);
            return particle;
        }

    }

    class Particle
    {
        ParticleProfile profile;

        float lifetime;
        //float rotation, rotationSpeed;
        Vector2 position;
     //   Vector2 speed, accleration;
        float size;
        Color color;
        

        //ParicleEmitter timeoutEmitter;

        public Particle()
        {
            
        }

        public void Init(ParticleProfile profile, Vector2 position, float size)
        {
            this.profile = profile;
            lifetime = 0;
            this.position = position;
            this.size = size;
            color = Color.CornflowerBlue;
        }

        public Particle SetColor(Color color)
        {
            this.color = color;
            return this;
        }

        


        public void Update()
        {
            float normalaizedLifetime = lifetime / profile.maxLifetime;
            size += profile.dSize;



            // normolizedLifetime = lifetime / maxLifetime;
            /*color = colorUpdater(normolizedLifetime);

            spped += accselaration;
            position += speed;
            lifeTime += dt;
            size += dsize;*/
            lifetime++;

        }

        public bool IsDead()
        {
            return lifetime > profile.maxLifetime;
        }

        public void Draw()
        {            
            MyGraphics.sb.Draw(profile.texture, position, null, color, 0f, profile.origin, size*profile.scale, SpriteEffects.None, 1);
        }
    }
}

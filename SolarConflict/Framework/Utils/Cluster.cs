using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.Utils {

    public struct Transform {
        public Vector2 Position;

        public float Rotation;

        public Transform(Vector2 position, float rotation = 0f) {
            Position = position;
            Rotation = rotation;
        }
    }

    /// <summary>Class for distributing a bunch of points in 2D space</summary>
    /// <remarks>TODO: move, rename, and maybe utilize in ParamEmitter (lot of overlap between this class and that)</remarks>
    abstract class Cluster {
        /// <param name="random">If null, Cluster should return a deterministic pattern</param>
        /// <returns>[(position, rotation)]</returns>
        public abstract IEnumerable<Transform> Transforms(int numberToGenerate, Random random = null);
    }

    class Circle : Cluster {
        public float Radius;
        
        public Circle(float radius = -1f) {
            Radius = radius;
        }

        public override IEnumerable<Transform> Transforms(int numberToGenerate, Random random = null) {
            // TODO: deterministic implementation if random == null
            Debug.Assert(Radius >= 0f, "Circle radius is negative or uninitialized");
            
            for (int i = 0; i < numberToGenerate; ++i) {
                var angle = MathHelper.TwoPi * random.NextFloat();
                var distance = Radius * (float)Math.Sqrt(random.NextFloat());
                yield return new Transform(new Vector2((float)Math.Cos(angle) * distance, (float)Math.Sin(angle) * distance), angle);
            }
        }
    }

    class Ring : Cluster {
        public float Radius;

        public float Rotation;

        public Ring(float radius = -1f, float rotation = 0f) {
            Radius = radius;
            Rotation = rotation;
        }

        public override IEnumerable<Transform> Transforms(int numberToGenerate, Random random = null) {
            Debug.Assert(Radius >= 0f, "Ring radius is negative or uninitialized");

            var angleIncrement = (2f * (float)Math.PI) / numberToGenerate;

            var angle = Rotation;
            for (int i = 0; i < numberToGenerate; ++i) {
                yield return new Transform(new Vector2((float)Math.Cos(angle) * Radius, (float)Math.Sin(angle) * Radius), angle);

                angle += angleIncrement;
            }
        }
    }
}

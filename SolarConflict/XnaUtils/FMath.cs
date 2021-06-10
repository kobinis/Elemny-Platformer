using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaUtils
{

    //public static 

    //TOM: add a function that uniformly selects a random point in a band (Gets, random, maybe angle, so only do the radius)

    public static class FMath //change all values to floats
    {
        public static Random Rand = new Random(); //Todo: static per thread

        public static float FindScaleFitToBox(int width, int height, int limitWidth, int limitHeight)
        {
            float scaleX = limitWidth / (float)width;
            float scaleY = limitHeight / (float)height;
            float scale = Math.Min(scaleX, scaleY);
            return scale;
        }

        public static float FitInsideBox(ref int width, ref int height, int limitWidth, int limitHeight)
        {
            float scale = Math.Min(FindScaleFitToBox(width, height, limitWidth, limitHeight), 1f);
            width = (int)(width * scale);
            height = (int)(height * scale);
            return scale;
        }

        public static float MoveToTarget(float value, float target, float speed)
        {
            if (value > target)
            {
                value = Math.Max(value - speed, target);
            }
            if (value < target)
            {
                value = Math.Min(value + speed, target);
            }
            return value;
        }

        public static Rectangle ResizeRectangle(Rectangle rectangle, float scale)
        {
            return new Rectangle((int)(rectangle.X * scale), (int)(rectangle.Y * scale), (int)(rectangle.Width * scale), (int)(rectangle.Height * scale));
        }

        public static Vector2 GetMidPoint(Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
        }

        public static void Shuffle<T>(IList<T> list, Random rng)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Rectangle GetRectangle(Vector2 position, Vector2 size)
        {
            return new Rectangle((int)(position.X - size.X * 0.5f), (int)(position.Y - size.Y * 0.5f), (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Find the Greatest Common Divisor
        /// </summary>
        /// <param name="a">Number a</param>
        /// <param name="b">Number b</param>
        /// <returns>The greatest common Divisor</returns>
        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long tmp = b;
                b = a % b;
                a = tmp;
            }
            return a;
        }

        /// <summary>
        /// Find the Least Common Multiple
        /// </summary>
        /// <param name="a">Number a</param>
        /// <param name="b">Number b</param>
        /// <returns>The least common multiple</returns>
        public static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }

        public static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

        public static int Zigzag(int a, int b)
        {
            return Math.Abs(Mod(a + b, 2 * b) - b);
        }


        public static float AngleDiff(float deg1, float deg2) //change names
        {
            return AngleAbs(deg2 - deg1) * DegSign(deg2 - deg1);
        }

        public static float AngleAbs(float deg)
        {
            return Math.Abs(RealMod(Math.Abs(deg) + MathHelper.Pi, MathHelper.TwoPi) - MathHelper.Pi);
        }

        public static float DegSign(float deg) //ToDo
        {
            if (float.IsNaN(deg))
                //  return 0;
                throw new Exception();
            //return 0;//MyMath.Rand.Next(2) * 2 - 1; //Remove ? //throw exception
            else
                return Math.Sign(deg) * Math.Sign(RealMod(Math.Abs(deg) + MathHelper.Pi, MathHelper.TwoPi) - MathHelper.Pi);
        }

        public static float Frac(float value)
        {
            return value - (float)Math.Truncate(value);
        }

        /// <summary>Given an angle (in radians), returns it in [0, 2pi]</summary>        
        public static float NonNegativeAngle(float radians)
        {
            var result = radians % MathHelper.TwoPi;
            if (result < 0)
                result += MathHelper.TwoPi;

            return result;
        }

        public static float RealMod(float x, float y)
        {
            float res = x / y;
            return (float)(res - Math.Truncate(res)) * y;
        }

        /// <summary>Given two angles (in radians), returns the smallest rotation which would turn the second into the first</summary>        
        public static float SmallestAngleDiff(float radians1, float radians2)
        {
            var result = (NonNegativeAngle(radians1) - NonNegativeAngle(radians2)) % MathHelper.TwoPi;

            if (result > MathHelper.Pi)
                return result - MathHelper.TwoPi;
            if (result < -MathHelper.Pi)
                return result + MathHelper.TwoPi;
            return result;
        }

        /// <summary>
        /// Transforms from a uniformly distributed [0,1] interval to [minRadius,maxRadius] that will reslut in a uiform distribtion on a ring
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxRadius"></param>
        /// <param name="minRadius"></param>
        /// <returns></returns>
        public static float TransformToRadius(float value, float maxRadius, float minRadius = 0)
        {
            float radius = (float)Math.Sqrt(value * (maxRadius * maxRadius - minRadius * minRadius) + minRadius * minRadius);
            return radius;
        }

        public static bool Bern(float p, Random random)
        {
            return random.NextDouble() < p;
        }

        //Xna

        public static Vector2 MoveTowards(Vector2 initPos, Vector2 target, float speed = 1f)
        {
            Vector2 diff = target - initPos;
            float length = diff.Length();
            if (length <= speed)
            {
                return target;
            }
            return initPos + (diff / length) * speed;
        }

        /// <summary>
        /// Converts from Polar to a Cartesian coordinates
        /// </summary>
        /// <param name="radius">radius</param>
        /// <param name="angle"> Angle in radians</param>
        /// <returns></returns>
        public static Vector2 ToCartesian(float radius, float angle)
        {
            return new Vector2(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle));
        }

        public static float GetRotation(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 RotateVector(Vector2 rotationVector, Vector2 vector2)
        {
            return new Vector2(rotationVector.X * vector2.X - rotationVector.Y * vector2.Y, rotationVector.X * vector2.Y + rotationVector.Y * vector2.X);
        }

        public static Vector2 RotateVector(Vector2 vector, float angle)
        {
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);

            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }


        /// <summary>
        /// Returens the intercection point of a rectangle centerd at (0,0) with a vector ending at point
        /// </summary>
        public static Vector2 RectangleIntersectionPoint(Vector2 point, Vector2 halfSize)
        {
            float t1 = halfSize.Y / (Math.Abs(point.Y) + 0.001f);
            float x = Math.Min(halfSize.X, Math.Abs(t1 * point.X)) * Math.Sign(point.X);
            float t2 = halfSize.X / (Math.Abs(point.X) + 0.001f);
            float y = Math.Min(Math.Abs(t2 * point.Y), halfSize.Y) * Math.Sign(point.Y);
            return new Vector2(x, y);
        }

        public static float FindScale(Vector2 originalSize, Vector2 targetSize)
        {
            if (originalSize.X / originalSize.Y > targetSize.X / targetSize.Y)
            {
                return targetSize.X / originalSize.X;
            }
            else
            {
                return targetSize.Y / originalSize.Y;
            }
        }

        public static float FindBigScale(Vector2 originalSize, Vector2 targetSize)
        {
            if (originalSize.X / originalSize.Y > targetSize.X / targetSize.Y)
            {
                return targetSize.Y / originalSize.Y;
            }
            else
            {
                return targetSize.X / originalSize.X;
            }
        }

        /// <summary>
        /// Fit the originalSize inside the targetSize preserving the aspect ratio
        /// </summary>
        /// <param name="originalSize"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public static Vector2 FitSize(Vector2 originalSize, Vector2 targetSize)
        {
            Vector2 fitedSize = new Vector2();

            if (originalSize.X / originalSize.Y > targetSize.X / targetSize.Y)
            {

                fitedSize.X = targetSize.X;
                fitedSize.Y = targetSize.X / originalSize.X * originalSize.Y;
            }
            else
            {
                fitedSize.Y = targetSize.Y;
                fitedSize.X = targetSize.Y / originalSize.Y * originalSize.X;
            }
            return fitedSize;
        }

        /// <summary>
        /// Cover the targetSize with originalSize preserving the aspect ratio
        /// </summary>
        /// <param name="originalSize"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public static Vector2 CoverSize(Vector2 originalSize, Vector2 targetSize) //TODO: check
        {
            throw new NotImplementedException();
        }

        public static Vector2 GetFormationPosition(int index)
        {
            int xIndex = (int)Math.Floor((-3 + Math.Sqrt(1 + 8 * index)) / 2) + 1;
            float ypos = (index - Math.Max(((xIndex - 1) * (xIndex - 1) + 3 * (xIndex - 1)) / 2f + 1, 0)) - xIndex / 2f;
            return new Vector2(-xIndex, ypos);
        }

        public static Vector2 GetFormationPosition(int index, float angle)
        {
            return RotateVector(GetFormationPosition(index), angle);
        }

        public static Rectangle CenterAndSizeToRect(Vector2 center, Vector2 size)
        {
            Vector2 halfSize = size * 0.5f;
            return new Rectangle((int)(center.X - halfSize.X + 0.5f), (int)(center.Y - halfSize.Y + 0.5f), (int)(size.X + 0.5f), (int)(size.Y + 0.5f));
        }

        public static Vector2 FlipX(Vector2 vector)
        {
            return new Vector2(-vector.X, vector.Y);
        }

        public static Vector2 FlipY(Vector2 vector)
        {
            return new Vector2(vector.X, -vector.Y);
        }

        public static float Rotation(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 MinMagVector(Vector2 vec1, Vector2 vec2)
        {
            float x = Math.Abs(vec1.X) < Math.Abs(vec2.X) ? vec1.X : vec2.X;
            float y = Math.Abs(vec1.Y) < Math.Abs(vec2.Y) ? vec1.Y : vec2.Y;
            return new Vector2(x, y);
        }

        //Path utils

        public static Vector2 LerpPath(List<Vector2> path, float time)
        {
            int index = Math.Min(Math.Max((int)time, 0), path.Count - 2);
            float amount = Math.Min(time - index, 1);
            return Vector2.Lerp(path[index], path[index + 1], amount);
        }

        public static Vector2 CubicPath(List<Vector2> path, float time)
        {
            Microsoft.Xna.Framework.Curve c = new Curve();
            // c.Keys.Add(new CurveKey()
            //    c.Evaluate()
            int index = Math.Min(Math.Max((int)time, 0), path.Count - 2);
            float amount = Math.Min(time - index, 1);
            return Vector2.Lerp(path[index], path[index + 1], amount);
        }


        public static void PathAdd(List<Vector2> path, Vector2 point, float mult = 1)
        {
            for (int i = 0; i < path.Count; i++)
            {
                path[i] = path[i] * mult + point;
            }
        }

        public static float Noise(float x, float y)
        {
            return (float)Math.Sin(x * y);
        }


        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static Vector2 PointInCircle(this Random random, float radius)
        {
            var angle = MathHelper.TwoPi * random.NextFloat();
            var distance = radius * (float)Math.Sqrt(random.NextFloat());
            return new Vector2((float)Math.Cos(angle) * distance, (float)Math.Sin(angle) * distance);
        }


        public static Vector2 PointOnElipse(this Random random, Vector2 size) //Change
        {
            return new Vector2((float)Math.Cos(random.NextDouble() * MathHelper.TwoPi) * size.X, (float)Math.Sin(random.NextDouble() * MathHelper.TwoPi) * size.Y);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Utils
{

    public static class SerializationUtils
    {
        public static SurrogateSelector MakeSurrogateSelector()
        {
            var ss = new SurrogateSelector();
            ss.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), new ColorSerializationSurrogate());
            ss.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), new Vector2SerializationSurrogate());
            ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
            ss.AddSurrogate(typeof(Point), new StreamingContext(StreamingContextStates.All), new PointSerializationSurrogate());
            ss.AddSurrogate(typeof(Rectangle), new StreamingContext(StreamingContextStates.All), new RectangleSerializationSurrogate());
            ss.AddSurrogate(typeof(Texture2D), new StreamingContext(StreamingContextStates.All), new Texture2DSerializationSurrogate());
            ss.AddSurrogate(typeof(EffectTechnique), new StreamingContext(StreamingContextStates.All), new EffectTechniqueSerializationSurrogate());            
            return ss;
        }
    }     

    // This class can manually serialize an Color object.
    sealed class ColorSerializationSurrogate : ISerializationSurrogate
    {

        // Serialize the Color object
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var col = (Color)obj;
            info.AddValue("r", col.R);
            info.AddValue("g", col.G);
            info.AddValue("b", col.B);
            info.AddValue("a", col.A);         
        }

        // Deserialize the Color object
        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var col = (Color)obj;
            col.R = info.GetByte("r");
            col.G = info.GetByte("g");
            col.B = info.GetByte("b");
            col.A = info.GetByte("a");
            return col;
        }
    }


    



    sealed class Vector2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var vec = (Vector2)obj;
            info.AddValue("x", vec.X);
            info.AddValue("y", vec.Y);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var vec = (Vector2)obj;
            vec.X = info.GetSingle("x");
            vec.Y = info.GetSingle("y");
            return vec;
        }
    }


    sealed class Vector3SerializationSurrogate : ISerializationSurrogate
    {

        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var vec = (Vector3)obj;
            info.AddValue("x", vec.X);
            info.AddValue("y", vec.Y);
            info.AddValue("z", vec.Z);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var vec = (Vector3)obj;
            vec.X = info.GetSingle("x");
            vec.Y = info.GetSingle("y");
            vec.Z = info.GetSingle("z");
            return vec;
        }
    }

    sealed class PointSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var sobj = (Point)obj;
            info.AddValue("x", sobj.X);
            info.AddValue("y", sobj.Y);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var sobj = (Point)obj;
            sobj.X = info.GetInt32("x");
            sobj.Y = info.GetInt32("y");
            return sobj;
        }
    }

    sealed class RectangleSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var sobj = (Rectangle)obj;
            info.AddValue("x", sobj.X);
            info.AddValue("y", sobj.Y);
            info.AddValue("w", sobj.Width);
            info.AddValue("h", sobj.Height);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var sobj = (Rectangle)obj;
            sobj.X = info.GetInt32("x");
            sobj.Y = info.GetInt32("y");
            sobj.Width = info.GetInt32("w");
            sobj.Height = info.GetInt32("h");
            return sobj;
        }
    }

    sealed class Texture2DSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var vec = (Texture2D)obj;
            info.AddValue("id", vec.Name);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return TextureBank.Inst.GetTexture(info.GetString("id"));
        }
    }

    sealed class EffectTechniqueSerializationSurrogate : ISerializationSurrogate
    {

        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            var vec = (EffectTechnique)obj;
            info.AddValue("id", vec.Name);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            return Camera.NormalMapEffect.Techniques[info.GetString("id")];
        }
       
    }
    
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaUtils
{
    public class Path2
    {
        private Curve _xCurve;
        private Curve _yCurve;

        public int Count { get { return _xCurve.Keys.Count; } }
        
        public Path2(IEnumerable<Vector2> path)
        {
            _xCurve = new Curve();
            _yCurve = new Curve();
            int counter = 0;
            foreach (var point in path)
            {
                _xCurve.Keys.Add(new CurveKey(counter, point.X));
                _yCurve.Keys.Add(new CurveKey(counter, point.Y));
                counter++;
            }
            _xCurve.ComputeTangents(CurveTangent.Smooth);
            _yCurve.ComputeTangents(CurveTangent.Smooth);
        }

        public Vector2 Evaluate(float time)
        {
            return new Vector2(_xCurve.Evaluate(time), _yCurve.Evaluate(time));
        }

    }
}

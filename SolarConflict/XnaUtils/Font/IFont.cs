using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.XnaUtils.FontEngine
{
    public interface IFont
    {
        void DrawString(Camera camera, string text);
    }
}

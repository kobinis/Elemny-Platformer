using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.XnaUtils
{
    interface ISaverLoader
    {
        void Save(string path, object o);
        object Load(string path);
    }
}

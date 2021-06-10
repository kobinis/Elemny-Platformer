using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Scenes.HudEngine
{
    public struct HudInfo
    { 
        public string Name { get; set; }
        public string TextureId { get; set; }
        public Keys ActivationKey { get; set; }
        public Keys AltActivationKey { get; set; }
        //public int Order { get; set; }

        public HudInfo(string name, string textureId, Keys activationKey, Action<Scene> action, Keys altActivationKey = Keys.None)
        {
            Name = name;
            TextureId = textureId;
            ActivationKey = activationKey;
            AltActivationKey = altActivationKey;
        }
    }

}

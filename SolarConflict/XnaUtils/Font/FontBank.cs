using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.XnaUtils
{
    public class FontBank
    {
        private static FontBank bank = null;

        public static FontBank Inst
        {
            get
            {
                if (bank == null)
                {
                    bank = new FontBank();
                }
                return bank;
            }
        }

        public static SpriteFont DefaultFont { get; set; }


    }
}

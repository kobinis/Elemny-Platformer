using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.CardGame
{
    public class Card
    {
        public int Value { get; set; }
        public string TextureID { get; set; }

        public Card(int value, string textureID)
        {
            Value = value;
            TextureID = textureID;
        }        
    }
}

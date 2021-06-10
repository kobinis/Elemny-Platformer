using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;

namespace SolarConflict
{
    [Serializable]
    public class CharacterBank
    {
        private Dictionary<int, Character> _bank;
        private Dictionary<string, int> _stringToInt;

        public CharacterBank()
        {
            _bank = new Dictionary<int, Character>();
            _stringToInt = new Dictionary<string, int>();
        }



        public bool AddCharacter(string stringID, int intID, Character character)
        {
            stringID = stringID.ToLower();
            if (!_bank.ContainsKey(intID) && (string.IsNullOrWhiteSpace(stringID) || !_stringToInt.ContainsKey(stringID)))
            {
                _bank.Add(intID, character);
                if(!string.IsNullOrWhiteSpace(stringID))
                {
                    _stringToInt.Add(stringID, intID);
                }
                return true;
            }
            return false;
        }

        //Finds a free index;
        private int FindFreeIndex()
        {
            int index = _bank.Count;
            while(_bank.ContainsKey(index))
            {
                index++;
            }
            return index;
        }

        public Character Get(int id)
        {            
            return _bank.Get(id); //Or try Get
        }

        public Character Get(string id)
        {
            int intID = _stringToInt.Get(id.ToLower());
            return Get(intID);
        }

        //public Character Make(string image, string name, ge )
        //{
        //    return new Character() //Temp
        //    {
        //        SpriteID = image,
        //        Name = StringUtils.CapFirst(image),
        //        Gender = Gender.Female,
        //        FactionType = Framework.FactionType.Neutral

        //    };
        //}


        /// <summary>
        /// Get's you "Generated" character from hash
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public Character GetCharacterFromHash(int hash)
        {
            return new Character() //Temp
            {
                SpriteID = "help",
                Name = " ",
                Gender = Gender.Male,
                FactionType = Framework.FactionType.Neutral
                
            };
        }
    }
}


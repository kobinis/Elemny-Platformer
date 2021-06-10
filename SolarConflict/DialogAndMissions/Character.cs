using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;
using SolarConflict.Framework;

namespace SolarConflict
{
    public enum Gender
    {
        Male,
        Female,
        AndroidMale,
        AndroidFemale,
        Android,
        Other,
        Unknown
    }

    /// <summary>
    /// Hold data for Character
    /// </summary>
    [Serializable]
    public class Character//TODO: replace with charechter bank hold charcter id num in bank
    {
        public string Name;
        public string SpriteID;
        public Gender Gender;
        public FactionType FactionType;
        public string GreedingsDialogID;
        public string BiographyID;

        public Character()
        {
        }

        public Character(string image, string name)
        {
            Name = name;
            SpriteID = image;
        }


        //public override AgentSystem GetWorkingCopy()
        //{
        //    return this;
        //}
    }
}


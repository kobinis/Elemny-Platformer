using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Generation.TemplateGenerationEngine.Templates
{
    class CharacterTemplate : GenerationTemplate
    {
        const string Index = "Index";
        const string Name = "Name";
        const string Image = "Image";
        const string Gender = "Gender";
        const string Faction = "Faction";
        const string GreedingsID = "GreedingsID";


        public CharacterTemplate()
        {
            
            _directoryName = "CharacterTemplate";
            AddParametereName(Index);
            AddParametereName(ID);
            AddParametereName(Name);
            AddParametereName(Image);
            AddParametereName(Gender);
            AddParametereName(Faction);
            AddParametereName(GreedingsID);

        }
        

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            try
            {
                int index = csvUtils.GetInt(Index);
                string id = csvUtils.GetString(ID, string.Empty);
                string name = csvUtils.GetString(Name);
                string image = csvUtils.GetString(Image);
                Gender gender = csvUtils.GetEnum<Gender>(Gender, SolarConflict.Gender.Male);
                FactionType faction = csvUtils.GetEnum<FactionType>(Faction, FactionType.Neutral);
                string greedingsId = csvUtils.GetString(GreedingsID);

                Character character = new Character()
                {
                    Name = name,
                    SpriteID = image,
                    Gender = gender,
                    FactionType = faction,
                    GreedingsDialogID = greedingsId
                };

                ContentBank.Inst.CharacterBank.AddCharacter(id, index, character);                
            }
            catch (Exception e)
            {
                ActivityManager.Inst.AddToast(e.ToString(), 120, Color.Red);
            }
                        
        }


    }
}




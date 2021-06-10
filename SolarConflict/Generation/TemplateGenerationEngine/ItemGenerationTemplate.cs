using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.XnaUtils.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration
{
    public abstract class ItemGenerationTemplate: GenerationTemplate
    {
        private readonly int DEFAULT_MAX_STOCK = 1;


        protected readonly string NAME = "Name*";
        protected readonly string DESCRIPTION = "Description";
        protected readonly string QUALITY = "Quality";
        protected readonly string SIZE = "Size*";
        protected readonly string TEXTURE = "Texture";
        protected readonly string EQUIPPED_TEXTURE = "Equipped Texture";
        protected readonly string PRICE = "Price";
        protected readonly string SELL_RATIO = "Sell Ratio";
        public const string COLOR = "Color*";

        protected readonly string CAPACITY = "Capacity";
        protected readonly string METER_COST = "Meter Cost*";
        protected readonly string METER_TYPE = "Meter Type*";
        protected readonly string ACTIVE_TIME = "Active Time*";


        protected readonly int DEFUALT_ACTIVE_TIME = 1;
        protected readonly Color DEFAULT_COLOR = Color.White;
        protected readonly SizeType DEFAULT_SIZE = SizeType.Small;


        public ItemGenerationTemplate()
        {           
        }
                  
        
        protected void AddGeneralParameters()
        {
            AddParametereName(ID);
            AddParametereName(NAME);
            AddParametereName(DESCRIPTION);
            AddParametereName(QUALITY);
            AddParametereName(SIZE);
            AddParametereName(TEXTURE);
            AddParametereName(EQUIPPED_TEXTURE);
            AddParametereName(PRICE);
            AddParametereName(SELL_RATIO);
            AddParametereName(COLOR);
        }

        protected virtual ItemProfile InitGeneralParameters(string ID, string name, string description, int level, SizeType size, string textureID, 
                                                    string equippedTextureID, float buyPrice, float sellRatio, Color? color, SlotType itemType)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = ID;
            }

            ItemProfile profile = ItemQuickStart.Profile(name, description, level, textureID, equippedTextureID);
            profile.Id = ID;// + "Item"; // YANIV: remove add this suffix only to items.
            profile.DescriptionText = description;
            profile.BuyPrice = buyPrice;
            profile.SellPrice = (int)(buyPrice * sellRatio);
            profile.ItemSize = size;
            profile.MaxStack = DEFAULT_MAX_STOCK;            
            profile.IsActivatable = false;            
            profile.SlotType = itemType;

            if (color.HasValue)
                profile.TextureColor = color.Value;

            return profile;
        }


        protected virtual Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID, 
                                        string equippedTextureID, float buyPrice, float sellRatio, Color? color)
        {
            return null;
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            Item item = MakeItem(csvUtils.GetString(ID), csvUtils.GetString(NAME), csvUtils.GetString(DESCRIPTION), csvUtils.GetInt(QUALITY), 
                                 csvUtils.GetEnum<SizeType>(SIZE, DEFAULT_SIZE), csvUtils.GetString(TEXTURE), csvUtils.GetString(EQUIPPED_TEXTURE), 
                                 csvUtils.GetInt(PRICE), csvUtils.GetFloat(SELL_RATIO), csvUtils.GetColor(COLOR));

            ContentBank.Inst.AddContent(item);
        }   

    }
}

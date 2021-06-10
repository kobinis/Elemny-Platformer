//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
//{
//    public class WarHeadTemplate : NonEquippedItemsTemplate
//    {
//        private readonly string EMITTER_ID = "Emitter ID";

//        private readonly int DEFAULT_MAX_STACK = 20;

//        public WarHeadTemplate()
//            : base()
//        {
//            _directoryName = "WarHeads";
//            AddParametereName(MAX_STACK);
//            AddParametereName(COOLDOWN);
//            AddParametereName(EMITTER_ID);
//        }

//        protected override Item MakeItem(string ID, string name, string description, Item.RarityType rairty, ItemSize size, string textureID, float buyPrice, float sellRatio, Color? color)
//        {
//            ItemProfile profile = InitGeneralParameters(ID, name, description, rairty, size, textureID, null, buyPrice, sellRatio, color, Item.Category.Warhead | Item.Category.Material);
//            profile.MaxStack = csvUtils.GetInt(MAX_STACK, DEFAULT_MAX_STACK);
//            profile.IsRetainedOnDeath = false;
//            profile.IsActivatable = true;
//            profile.IsWorkingOnlyInSlot = false;
//            profile.IsConsumed = true;

//            Item item = new Item(profile);
//            AgentEmitter emitter = new AgentEmitter();
//            emitter.CooldownTime = csvUtils.GetInt(COOLDOWN, DEFAULT_COOLDOWN);
//            emitter.EmitterID = csvUtils.GetString(EMITTER_ID);

//            item.MainSystem = emitter; 

//            return item;
//        }

//    }
//}

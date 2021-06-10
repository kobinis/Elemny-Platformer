using Microsoft.Xna.Framework;
using SolarConflict;
using SolarConflict.GameContent.Utils;
using System.Linq;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    //REFACTOR: Maybe unite Meter cost meter type to one column.

    public class WeaponTemplate : ItemGenerationTemplate
    {
        private readonly string SHOT_SPEED = "Shot Speed";
        private readonly string COOL_DOWN = "Cooldown";
        private readonly string INNER_COOLDOWN = "Inner Cooldown";
        private readonly string SOUND_ID = "Sound ID";
        private readonly string EFFECT_ID = "Effect ID";
        private readonly string KICKBACK = "Kickback";
        private readonly string TURRET = "CreateTurretVersion*";
        private readonly string SHOT_ID = "Shot Name";
        private readonly string EFFECT_COLOR = "Effect Color*";
        private readonly string LIFETIME = "Lifetime*";
        private readonly string CATEGORY = "Category*";
        private readonly string FLAVOUR = "Flavour Text*";
        //private readonly string TAGS = "Tags";
        private readonly string DEFAULT_SOUND_ID = "sound_shotgun";
        private readonly string DEFAULT_EFFECT_ID = "GunFlashFx";
        private readonly float DEFAULT_KICKBACK = 0;
        private readonly int DEFAULT_INNER_COOLDOWN = 0;
        //private readonly bool DEFAULT_TURRET = false;

        public WeaponTemplate()
        {
            _directoryName = "Weapons";
            AddGeneralParameters();            
            AddParametereName(TURRET);
            AddParametereName(COOL_DOWN);
            AddParametereName(INNER_COOLDOWN);
           // AddParametereName("Shot ID"); // emitterId
            AddParametereName(SHOT_ID); // emitterId
            AddParametereName(ACTIVE_TIME);
            AddParametereName(SHOT_SPEED);
            AddParametereName(METER_TYPE);
            AddParametereName(METER_COST);
            AddParametereName(SOUND_ID);
            AddParametereName(EFFECT_ID);
            AddParametereName(EFFECT_COLOR);
            AddParametereName(KICKBACK);
            AddParametereName(LIFETIME);
            AddParametereName(CATEGORY);
            AddParametereName(FLAVOUR);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            Item item = MakeWeapon(csvUtils.GetString(ID), csvUtils.GetString(NAME), csvUtils.GetInt(QUALITY),
                                 csvUtils.GetEnum<SizeType>(SIZE, DEFAULT_SIZE), csvUtils.GetString(TEXTURE), csvUtils.GetString(EQUIPPED_TEXTURE),
                                 csvUtils.GetInt(PRICE), csvUtils.GetFloat(SELL_RATIO), csvUtils.GetColor(COLOR), false);

            ContentBank.Inst.AddContent(item);

            //if (csvUtils.GetBool(TURRET, DEFAULT_TURRET))
            //{
            //    Item turretItem = MakeWeapon(csvUtils.GetString(ID) + "Turret", csvUtils.GetString(NAME) + " Turret", csvUtils.GetInt(QUALITY),
            //                    csvUtils.GetEnum<SizeType>(SIZE, DEFAULT_SIZE), csvUtils.GetString(TEXTURE), csvUtils.GetString(EQUIPPED_TEXTURE),
            //                    csvUtils.GetInt(PRICE), csvUtils.GetFloat(SELL_RATIO), csvUtils.GetColor(COLOR), true);

            //    ContentBank.Inst.AddContent(turretItem);
            //}
        }
     

        protected Item MakeWeapon(string ID, string name, int level, SizeType size, string textureID,
                                 string equippedTextureID, float buyPrice, float sellRatio, Color? color, bool isTurret)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = ID;
            }

            WeaponData weaponData = new WeaponData(name, level, textureID, equippedTextureID);
            weaponData.Description = csvUtils.GetString(DESCRIPTION);
            weaponData.FlavourText = csvUtils.GetString(FLAVOUR);
            weaponData.ItemData.Size = size;
            weaponData.ShotEmitterID = csvUtils.GetString(SHOT_ID);
            weaponData.Cooldown = csvUtils.GetInt(COOL_DOWN);
            weaponData.ShotSpeed = csvUtils.GetFloat(SHOT_SPEED);
            weaponData.ItemData.BuyPrice = (int)buyPrice;
            weaponData.ItemData.SellRatio = sellRatio;
            weaponData.ActiveTime = csvUtils.GetInt(ACTIVE_TIME, DEFUALT_ACTIVE_TIME);
            weaponData.MidCooldownTime = csvUtils.GetInt(INNER_COOLDOWN, DEFAULT_INNER_COOLDOWN);
            weaponData.EffectColor = csvUtils.GetColor(EFFECT_COLOR, DEFAULT_COLOR);
            //weaponData.IsTurreted = isTurret;
            weaponData.KickbackForce = csvUtils.GetFloat(KICKBACK, DEFAULT_KICKBACK);
            weaponData.ShotLifetime = csvUtils.GetInt(LIFETIME, 0);

            if (color.HasValue)
            {
                weaponData.ShotColor = color.Value;
            }

            if (csvUtils.HasValue(METER_COST) && csvUtils.GetFloat(METER_COST) > 0)
            {
                weaponData.EnergyCost = csvUtils.GetFloat(METER_COST);
            }
            weaponData.SoundEffectEmitterID = csvUtils.GetString(SOUND_ID, DEFAULT_SOUND_ID);
            weaponData.EffectEmitterID = csvUtils.GetString(EFFECT_ID, DEFAULT_EFFECT_ID);
            Item item = WeaponQuickStart.Make(weaponData);            
            item.Profile.Id = ID;
            item.Profile.Category = csvUtils.GetEnum<ItemCategory>(CATEGORY, ItemCategory.None);
            item.Profile.FlavourText = csvUtils.GetString(FLAVOUR);
            return item;
        }   
    }
}


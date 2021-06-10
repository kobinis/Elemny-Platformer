using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents.Systems.EmitterCallers;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Emitters;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Utils
{
    public struct WeaponData
    {
        public ItemData ItemData;
        public IEmitter ShotEmitter;
        public string ShotEmitterID
        {
            get
            {
                return ShotEmitter?.ID;
            }
            set
            {
                ShotEmitter = ContentBank.Inst.GetEmitter(value);
            }
        }
        public IEmitter ActivationEmitter;
        public string ActivationEmitterID
        {
            set
            {
                ActivationEmitter = ContentBank.Inst.GetEmitter(value);
            }
        }
        public IEmitter WarmupEmitter;
        public string WarmupEmitterID { set { WarmupEmitter = ContentBank.Inst.GetEmitter(value); } }
        public IEmitter EndOfWarmpupEmitter { get; set; }
        public string EndOfWarmpupEmitterID { set { EndOfWarmpupEmitter = ContentBank.Inst.GetEmitter(value); } }
        public string EffectEmitterID { get; set; }
        public float EffectSpeed { get; set; }       
        public float ShotSpeed { get; set; }
        public int ShotLifetime { get; set; }
        public string SoundEffectEmitterID { get; set; }
        public int Cooldown { get; set; }
        public float EnergyCost { get; set; }
        public float KickbackForce { get; set; }
        public int WarmupTime { get; set; }
        public Color? ShotColor { get; set; }
        public Color? EffectColor { get; set; }
        public string ItemCostID { get; set; }
        public ItemCategory AmmoType { get; set; }
        public ParamEmitter AmmoPramEmitter { get; set; }        
        //public bool IsTurreted { get; set; }
        public bool IsAutoExtendedDescription { get; set; }
        public int ActiveTime { get; set; }
        public int MidCooldownTime { get; set; }                        
        public bool IsFixedToCenter { get; set; }
        public string Description { get; set; }
        public string FlavourText { get; set; }
        public int Range { get; set; }

        public float CooldownSec
        {
            get { return Cooldown / 60f; }
            set { Cooldown = (int)Math.Round(value * 60f); }
        }


        public WeaponData(string name)
            : this(name, 0, null, null)
        {
        }

        public WeaponData(string name, int level, string textureId, string equippedTextureId = null)
        {
            ItemData = new ItemData(name, level, textureId, equippedTextureId);
            ItemData.BreaksClocking = true;
            ItemData.SlotType = SlotType.Weapon | SlotType.Turret;
            ItemData.Category = ItemCategory.EnergyConsumingWeapon | ItemCategory.Gun;
            ShotEmitter = null;
            EffectEmitterID = null;
            SoundEffectEmitterID = null;
            WarmupEmitter = null;
            ActivationEmitter = null;
            EndOfWarmpupEmitter = null;
            Cooldown = 1;
            EnergyCost = 0;
            ShotSpeed = 0;
            EffectEmitterID =  typeof(GunFlashFx).Name;
            SoundEffectEmitterID = "sound_shot5";
            KickbackForce = 0;
            ShotLifetime = 0; //Range
            //IsTurreted = false;
            IsAutoExtendedDescription = true;
            ActiveTime = 1;
            MidCooldownTime = 0;
            ItemCostID = null;
            AmmoType = ItemCategory.None;
            AmmoPramEmitter = null;
            EffectColor = null;
            ShotColor = null;
            IsFixedToCenter = false;
            EffectSpeed = 0;
            WarmupTime = 0;
            Description = null;
            FlavourText = null;
            Range = 0;            
        }

        public int NumberOfShotsPerActivation {  get { return (int)Math.Ceiling(ActiveTime / (float)Math.Max(MidCooldownTime, 1)); } }
        public float  ComputeShotDamage(float targetDpf)
        {
            return targetDpf * Cooldown / NumberOfShotsPerActivation;
        }

        public float ComputeEnergyActivationCost(float targetEpf)
        {
            return targetEpf * Cooldown;
        }

        //public int NumberOfShotsPerActivation => (int)Math.Ceiling(ActiveTime / (float)Math.Max(MidCooldownTime, 1));

        public float GetRange() //TOdo: change
        {
            if (Range != 0)
                return Range;
            EmitterStatUtils.EmitterData shotData = new EmitterStatUtils.EmitterData();
            EmitterStatUtils.GetEmitterData(ShotEmitter, shotData);
            float lifetime = Math.Max(ShotLifetime,10);
            if (lifetime == 0)
                lifetime = shotData.Lifetime;
            float shotSpeed = ShotSpeed;
            return ShotSpeed * lifetime; // + ShotSize
        }

        //TODO: move this function out
        public string ExtendedDescription() //TODO: add calculation for active time and midcooldown
        {

            string result = string.Empty;

            if (IsAutoExtendedDescription)
            {
                EmitterStatUtils.EmitterData shotData = new EmitterStatUtils.EmitterData();
                //float cool
                EmitterStatUtils.GetEmitterData(ShotEmitter, shotData);
                float damagePerShoot = shotData.Damage;
                float dps =  damagePerShoot * 60f * NumberOfShotsPerActivation / Cooldown;
                var activationsPerSecond = 60f / Cooldown;
                var eps = activationsPerSecond * EnergyCost;                
                float lifetime = ShotLifetime; //TODO: fix
                if (lifetime == 0)
                    lifetime = shotData.Lifetime;
                float shotSpeed = ShotSpeed;
               // int dpsForBar = (int)Math.Round(Math.Min(dps / 300f, 1) * 100);
                //damage per energy
                //float range 
                StringBuilder sb = new StringBuilder();
                if (AmmoType != ItemCategory.None)
                {
                    sb.AppendLine(Color.Yellow.ToTag("Ammo Type Weapon")); //TODO: add ammo type
                }
                //    sb.AppendLine("Projectile Damage: " + Color.Red.ToTag(damagePerShoot.ToString())); //  if(ShotSpeed > 0)
                //if (ShotSpeed != 0)
                //    sb.Append("\nProjectile Speed: " + Palette.Highlight.ToTag(ShotSpeed.ToString()));
                if (damagePerShoot != 0)
                    sb.Append("Damage: " + Color.Red.ToTag(Math.Round( damagePerShoot).ToString()));
                if (EnergyCost != 0)
                    sb.Append("\nEnergy Consumption: " + Color.Yellow.ToTag(EnergyCost.ToString()));// + "#Image{EnergyCost}");
                if (Cooldown != 0)
                    sb.Append("\nCooldown: " + Palette.Highlight.ToTag(Math.Round(Cooldown / 60f, 2).ToString()) + " Sec"); //TODO: display time                

                if(NumberOfShotsPerActivation > 1)
                    sb.Append("\nShots per burst: " + Color.Red.ToTag(NumberOfShotsPerActivation.ToString()));// + "#Image{EnergyCost}");
                if(dps != 0)
                    sb.Append("\nDPS: " + Color.Red.ToTag(dps.ToString("0"))); // if (AmmoType == ItemCategory.None && dps > 0)
                //if (eps != 0)
                //    sb.Append("\nEPS: " + Color.Yellow.ToTag(eps.ToString("0"))); // if (AmmoType == ItemCategory.None && dps > 0)

                float range = Range;
                if (Range == 0)
                    range = lifetime * shotSpeed;

                //sb.Append("Range: " + lifetime * shotSpeed);
                //  sb.AppendLine("Level: " + ScalingUtils.EffectColor(ItemData.Level).ToTag(ItemData.Level.ToString()));

                if (AmmoType == ItemCategory.None)// && range != 0)
                    sb.Append("\nRange:" + range);

                if (AmmoType != ItemCategory.None)
                {
                    sb.Append("\nAmmo Type: " + Palette.Highlight.ToTag(AmmoType.ToString())); //TODO: change to category to tag
                }

                if (ItemCostID != null)
                {
                    Item item = ContentBank.Inst.GetItem(ItemCostID, false); //All ammo must be generated before
                    sb.Append("\nItem Cost: " + item.Profile.Name + " #image{" + item.Profile.IconTextureID + "}");
                }                
                result = sb.ToString();
            }

            return result;
        }        
    }

    class WeaponQuickStart
    {
        public static Item Make(WeaponData weaponData, bool makeBeam = false)
        {
            Debug.Assert(weaponData.Cooldown > 0);
            weaponData.Cooldown = Math.Max(1, weaponData.Cooldown); //TODO: in debug mode  throw an exception
            ItemProfile profile = ItemQuickStart.Profile(weaponData.ItemData);
            //weaponData.ItemData.SlotType |= weaponData.IsTurreted ? SlotType.Turret : SlotType.Weapon; //Change
            
            profile.SlotType = weaponData.ItemData.SlotType;            
            profile.MaxStack = 1;
            profile.DescriptionText = weaponData.Description;
            profile.StatsText = weaponData.ExtendedDescription();
            profile.IsShownOnHUD = true;
            profile.AmmoType = weaponData.AmmoType;

            Item item = new Item(profile);
            AgentSystem agentSystem = null;
            if (!makeBeam)
            {
                EmitterCallerSystem system = new EmitterCallerSystem(null);
                system.WarmupTime = weaponData.WarmupTime;
                system.AmmoTypeNeeded = weaponData.AmmoType;
                system.ammoParamEmitter = weaponData.AmmoPramEmitter;
                system.Emitter = weaponData.ShotEmitter;
                system.CooldownTime = weaponData.Cooldown;
                system.velocity = Vector2.UnitX * weaponData.ShotSpeed;
                system.SecondaryEmitterID = weaponData.EffectEmitterID;
                system.MaxLifetime = weaponData.ShotLifetime;
                system.SecondarySize = 40;
                system.secondaryVelocityMult = 0.1f; //KOBI: change to be absolute
                system.ThirdEmitterID = weaponData.SoundEffectEmitterID;
                system.ActiveTime = weaponData.ActiveTime;
                system.MidCooldownTime = weaponData.MidCooldownTime;
                system.SecondaryColor = weaponData.EffectColor;
                system.SelfSpeedLimit = 10;
                system.ActivationEmitter = weaponData.ActivationEmitter;
                system.WarmupEmitter = weaponData.WarmupEmitter;
                system.EndOfWarmupEmitter = weaponData.WarmupEmitter;

                if (!string.IsNullOrEmpty(weaponData.ItemCostID))
                {
                    system.ActivationCheck.AddItemCost(weaponData.ItemCostID, 1);
                }

                if (weaponData.KickbackForce != 0)
                {
                    system.SelfImpactSpec = new CollisionSpec();
                    system.SelfImpactSpec.Force = -weaponData.KickbackForce;
                }

                if (weaponData.EnergyCost > 0)
                {
                    system.ActivationCheck.AddCost(MeterType.Energy, weaponData.EnergyCost);
                }
                agentSystem = system;
            }
            else
            {
                BeamSystem system = new BeamSystem();
                system.beamEmitter = weaponData.ShotEmitter;
                system.EffectEmitterID = weaponData.EffectEmitterID;
                agentSystem = system;
            }

            item.System = agentSystem;            

            if(weaponData.AmmoType != ItemCategory.None)
            {
                item.Profile.Category = ItemCategory.AmmoWeapon;
            }

            if (weaponData.Range > 0)
                item.Profile.MaximalRange = weaponData.Range;
            else
                item.Profile.MaximalRange = weaponData.GetRange();           
            return item;
        }
    }
}

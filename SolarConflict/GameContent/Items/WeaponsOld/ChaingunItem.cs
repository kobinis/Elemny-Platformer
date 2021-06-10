using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;

namespace SolarConflict.GameContent.Items
{
    //class ChaingunItem //Now: move to csv
    //{
    //    public static Item Make()
    //    {
    //        ItemProfile profile = ItemQuickStart.Profile("Fast machine gun", "Rapid fire gun.", 0, "GunItem", "turret1");
    //        profile.Category = Item.Category.Weapon;
    //        profile.ItemSize = Item.SizeType.Small;
    //        profile.IsWorkingOnlyInSlot = true;
    //        profile.Category = Item.Category.Weapon;
    //        profile.BuyPrice = 500;//1500;
    //        profile.SellPrice = 250;//750;

    //        Item item = new Item(profile);
    //        AgentEmitter gun = new AgentEmitter(ControlSignals.None, typeof(EmitterGun1).Name);
    //        gun.ActiveTime = 20;
    //        gun.CooldownTime = 30;
    //        gun.MidCooldownTime = 5;
    //        gun.velocity = Vector2.UnitX * 40;
    //        gun.ActivationCheck.AddCost(MeterType.Energy, 10);
    //        gun.refVelocityMult = 1;
    //        gun.SelfImpactSpec = new CollisionInfo();
    //        item.MainSystem = gun;

    //        profile.DescriptionText += ItemQuickStart.ExtendedDescription(gun.CooldownTime, "Shot1", 10);


    //        return item;
    //    }
    //}
}

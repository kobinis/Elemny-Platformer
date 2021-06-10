using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes.Activitys.Shop
{
    [Serializable]
    public class ShopData //Make it a agent syystem
    {
        public Inventory Inventory;
        public Sprite Portrait;
        public String ShopName;
        public Dictionary<string, float> ItemPriceMultiplier;

        //private void GenerateItemPriceMultipliers()
        //{
        //    foreach (var item in _items)
        //    {
        //        Random random = new Random(_seed + 5347 * item.GetHashCode());
        //        float time = (float)random.Next() * MathHelper.TwoPi + MetaWorld.Inst.WorldDate.DateInDays * 0.3f;
        //        float multiplier = 1 + (float)Math.Sin(time) * PriceRange;
        //        _itemPriceMultiplier[item] = multiplier;
        //    }
        //}
    }
}

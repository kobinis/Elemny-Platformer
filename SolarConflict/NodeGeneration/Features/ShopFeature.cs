using SolarConflict.Framework.World.Generation;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using System.Linq;
using System;

namespace SolarConflict.Session.World.Generation.Profiles
{
    /// <summary>
    /// A shop feature is a shop generator
    /// Its constructor takes parameters for the shop generation
    /// and its Generate() function generates the shop with items
    /// and registers it in the scene
    /// </summary>
    class ShopFeature : GenerationFeature
    {
        public int NumItems { get; private set; }               // number of items in the shop
        public ItemCategory ItemCategory { get; private set; }  // the allowed item categories

        public ShopFeature(FactionType? faction, int? level, int numItems, ItemCategory categories = ItemCategory.All) : base()
        {
            this.localFaction = faction;
            this.localLevel = level;
            this.NumItems = numItems;
            this.ItemCategory = categories;
        }

        // Generate a shop with items and register it in the scene
        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
          //  Vector2 position = generator.PlayerStartingPoint + Vector2.UnitX * 1000;
            Agent ship = GenerateShip("SmallShop1", scene.GameEngine, Position);
            if(Faction == FactionType.Neutral)
                ship.collideWithMask = GameObjectType.None;
            if (ship == null) return null;

            var factionItems = MetaWorld.Inst.GetFaction(Faction).GenerationData.ShipItems.ToList();

            //Add a shopping system to allow the ship to buy and sell items
            ShopSystem shop = new ShopSystem();
            GenerateShopItems(shop, ItemCategory.All, ItemCategory.None, Level, 6, factionItems, Rand);

            // Chance of stocking higher-tier items
            var bonusItems = ((int)(Rand.NextFloat() * 9)) - 7;
            if (bonusItems > 0)
                GenerateShopItems(shop, ItemCategory.All, ItemCategory.None, Level + 1, bonusItems, factionItems, Rand);

            ship.AddSystem(shop);
            ship.gameObjectType = GameObjectType.Agent;
            ship.Flags |= GameObjectFlags.UpdateOnlyOnScreen;
            return ship;
        }

        private Agent GenerateShip(string textureName, GameEngine engine, Vector2 position)
        {
            IEmitter emitter = ContentBank.Inst.GetEmitter(textureName);
            return emitter.Emit(engine, null, Faction, position, Vector2.Zero, MathHelper.PiOver2) as Agent;
        }

        public static void GenerateShopItems(ShopSystem shop , ItemCategory itemCategory, ItemCategory notInCategory, int level, int itemNumber, List<Item> items, System.Random random = null)
        {
            // Generates the first 'numItems' of level 'Level' of the faction's items
            int numGenerated = 0;
            items.Shuffle(random);
            foreach (var item in items) //TODO: add randomization
            {           
                if (item.Level == 0 || Math.Abs(item.Level - level) > random.Next(2)) continue;
                if ((item.Category & itemCategory) > 0 && (item.Category & notInCategory) ==0)
                {
                    shop.AddItem(item.ID);
                    numGenerated++;
                    if (numGenerated == itemNumber) break;
                }
            }
        }
    }
}

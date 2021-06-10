using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Generation.RecipeGeneration
{
    class RecipeGenerator
    {


        /// <summary>
        /// Calculate the amount of each material in the materials list that we use to create and item with the itemPrice.
        /// </summary>
        /// <param name="ItemPrice">the price goal of the item we want to create with the materials list.</param>
        /// <param name="matrials">materials used to create an item</param>
        /// <returns>list of amounts, ordered in the same order of the materials list.</returns>
        public static List<int> CalculateItemsAmounts(float ItemPrice, List<Item> matrials)
        {
            List<int> matrialAmounts = new List<int>();

            int numberOfItemsLeft = matrials.Count;

            foreach (var matrial in matrials)
            {
                int matrialAmount = (int)Math.Ceiling(ItemPrice / numberOfItemsLeft / matrial.Profile.BuyPrice);
                ItemPrice -= matrialAmount * matrial.Profile.BuyPrice;
                numberOfItemsLeft--;
                matrialAmounts.Add(matrialAmount);
            }

            return matrialAmounts;
        }

        /// <summary>
        /// For each item without recipe, we create recipe automatically with materials amount and costs that are based on the item cost.
        /// Call this method, after recipes from CSV was created.
        /// </summary>
        public static void CreateAutomaticRecipes()
        {
            List<Item> itemsWitoutRecipies = CreateItemsWitoutRecipiesList();

            List<Item> rawMaterials = ContentBank.Inst.GetAllItemsCopied(SlotType.None);

            foreach (var item in itemsWitoutRecipies)
            {
                var itemPrice = item.Profile.BuyPrice;

                List<Item> filtredRawMaterials = rawMaterials.Where(matrial => matrial.Profile.Level <= item.Profile.Level &&
                                                                               matrial.Profile.BuyPrice < itemPrice)
                                                           .OrderByDescending(matrial => matrial.Profile.BuyPrice).ToList<Item>();

                if (filtredRawMaterials.Count > 0)
                {
                    Random random = new Random(item.Profile.Id.GetHashCode()); // As long as we don't change the item ID, we will have the same list of materials for this item, each game.

                    List<Item> chosenMatrials = CreateChosenMatrialsList(filtredRawMaterials, random);

                    List<int> amounts = CalculateItemsAmounts(itemPrice, chosenMatrials);

                    CreateRecipe(item, chosenMatrials, amounts);
                }
            }
        }

        private static void CreateRecipe(Item item, List<Item> chosenMatrials, List<int> amounts)
        {
            Recipe recipe = new Recipe(item.ID);

            for (int i = 0; i < chosenMatrials.Count; i++)
            {
                recipe.AddCost(chosenMatrials[i].ID, amounts[i]);
            }

            ContentBank.Inst.AddContent(recipe);
        }

        private static List<Item> CreateChosenMatrialsList(List<Item> filtredRawMaterials, Random random)
        {
            int materialsAmount = Math.Min(random.Next(2, 5), filtredRawMaterials.Count);
            List<Item> chosenMatrials = new List<Item>();
            bool[] matrialWasUsed = new bool[filtredRawMaterials.Count];

            for (int i = 0; i < materialsAmount; i++)
            {
                int materialIndex = 0;

                do
                {
                    materialIndex = random.Next(filtredRawMaterials.Count);
                }
                while (matrialWasUsed[materialIndex]);

                matrialWasUsed[materialIndex] = true;
                chosenMatrials.Add(filtredRawMaterials[materialIndex]);
            }

            return chosenMatrials;
        }

        private static List<Item> CreateItemsWitoutRecipiesList()
        {
            List<Item> itemsWitoutRecipies = new List<Item>();
            List<Item> allItems = ContentBank.Inst.GetAllItemsCopied();

            foreach (var item in allItems)
            {
                if (!ContentBank.Inst.ContainsEmitter(item.ID + "Recipe") && item.Profile.SlotType != SlotType.None)
                {
                    itemsWitoutRecipies.Add(item);
                }
            }

            return itemsWitoutRecipies;
        }


    }
}

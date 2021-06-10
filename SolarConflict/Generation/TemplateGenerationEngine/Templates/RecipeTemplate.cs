using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using SolarConflict.Generation.RecipeGeneration;
using SolarConflict.Framework;
using System.Diagnostics;
using XnaUtils;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class RecipeTemplate : ItemGenerationTemplate
    {
        public RecipeTemplate()
        {
            _directoryName = "Recipes";
            AddParametereName(ID);
            AddParametereName("Crafting Station*");
            AddParametereName("Amount Produced");
        }

        // TODO: if user write amounts and the price is too small, write to the log.
        // TODO: if all the items more expansive from the item price, throw exception
        protected override void ParseAndAddEmitter(string[] parameters)
        {
            if(string.IsNullOrWhiteSpace( csvUtils.GetString(ID)))
            {
                return;
            }

            CraftingStationType craftingStation = 0;
            if (!string.IsNullOrWhiteSpace(csvUtils.GetString("Crafting Station*")))
            {
                string[] craftingStations = csvUtils.GetString("Crafting Station*").Split('|');
                foreach (var item in craftingStations)
                {
                    craftingStation |= ParserUtils.ParseEnum<CraftingStationType>(item);
                }
            }
            else
                craftingStation = CraftingStationType.Basic;

            int amountProduced = csvUtils.GetInt("Amount Produced", 1);

            Recipe recipe = new Recipe(csvUtils.GetString(ID), craftingStation, amountProduced);            
            float itemPrice = ContentBank.Inst.GetItem(csvUtils.GetString(ID), false).Profile.BuyPrice; // TODO: handle if item not exist

            List<Item> matrialsWithoutAmount = ParseMaterials(parameters, recipe, ref itemPrice);

            AddCost(recipe, itemPrice, matrialsWithoutAmount);

            ContentBank.Inst.AddContent(recipe);
        }

        private void AddCost(Recipe recipe, float itemPrice, List<Item> matrialsWithoutAmount)
        {
            if (itemPrice > 0)
            {
                var orderedMatrials = matrialsWithoutAmount.OrderByDescending(matrial => matrial.Profile.BuyPrice).ToList();
                var amounts = RecipeGenerator.CalculateItemsAmounts(itemPrice, orderedMatrials);

                for (int i = 0; i < orderedMatrials.Count; i++)
                {
                    recipe.AddCost(orderedMatrials[i].ID, amounts[i]);
                }
            }
        }

        private List<Item> ParseMaterials(string[] parameters, Recipe recipe, ref float itemPrice)
        {
            List<Item> neededMaterials = new List<Item>();
            var materialRatios = new Dictionary<string, float>();
            var usingRatios = 0; // tribool: -1 = false, 1 = true

            for (int i = 3; i < parameters.Length; i++) //TODO: change starting index to be done auto
            {
                if (parameters[i] != string.Empty)
                {
                    string[] materialData = parameters[i].Split(':');
                    int amountNeeded = 1;
                    bool isConsumed = true;

                    if (materialData.Length > 2)
                    {
                        isConsumed = ParserUtils.ParseBool(materialData[2], true);
                    }

                    if (materialData.Length > 1)
                    {
                        // Did we provide an explicit amount needed, or a ratio?
                        if (materialData[1].StartsWith(".")) {
                            // Ratio
                            Debug.Assert(usingRatios >= 0, "Crafting cost: can't combine material ratios with explicit material quantities");
                            usingRatios = 1;

                            materialRatios[materialData[0]] = ParserUtils.ParseFloat(materialData[1].Substring(1));
                        } else {
                            Debug.Assert(usingRatios <= 0, "Crafting cost: can't combine material ratios with explicit material quantities");
                            usingRatios = -1;

                            amountNeeded = ParserUtils.ParseInt(materialData[1], 1);
                            recipe.AddCost(materialData[0], amountNeeded, isConsumed);
                            float materialPrice = ContentBank.Inst.GetItem(materialData[0], false).Profile.BuyPrice; // TODO: handle if item not exist
                            itemPrice -= amountNeeded * materialPrice;
                        }
                    }
                    else
                    {
                        neededMaterials.Add(ContentBank.Inst.GetItem(materialData[0], false));
                    }
                }
            }

            if (usingRatios == 1) {

                throw new Exception("Using Tom's code exception !");  
                // Material costs were provided by ratio, calculate exact cost
                //var args = materialRatios.ToDictionary(p => ContentBank.Inst.GetItem(p.Key,false), p => p.Value);
                //var amounts = Items.Pricing.CraftingCost(args, itemPrice);
                //amounts.Do(p => recipe.AddCost(p.Key.ID, p.Value));
                //itemPrice = 0f; // this is actually remainingItemPrice; by zeroing it out, we communicate there's nothing left to be done
              
            }

            return neededMaterials;
        }
    }
}

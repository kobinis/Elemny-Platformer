using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SolarConflict.GameContent.ContentGeneration.Items {
    public static class Pricing {
        
        /// <summary>Given a dictionary of ingredients, their desired ratios, and a target total value, attempts to come up with a mix of said ingredients that satisfies
        /// said ratios and total value.</summary>
        /// <param name="materialCostsAndDesiredCostRatios">TEMP: {materialID->(materialValue, desiredCostRatio)}.
        /// TODO: look up value wherever we're supposed to look it up, or make the keys whatever type we use for materials, that provides a cost field or method.</param>
        //public static Dictionary<Item, int> CraftingCost(Dictionary<Item, float> materialsAndTargetRatios, float targetCost) {
        //    var result = CraftingCost(materialsAndTargetRatios.ToDictionary(p => p.Key.ID, p => Tuple.Create(p.Key.GetStackBuyPrice(), p.Value)), targetCost);
        //    return result.ToDictionary(p => materialsAndTargetRatios.Keys.First(k => k.ID == p.Key), p => p.Value);
        //}
        
        static Dictionary<string, int> CraftingCost(Dictionary<string, Tuple<float, float>> materialCostsAndDesiredCostRatios, float targetCost) {
            Debug.Assert(materialCostsAndDesiredCostRatios.Values.Sum(t => t.Item2) > 0f, "Crafting cost: Invalid material ratios or no materials given");
            Debug.Assert(materialCostsAndDesiredCostRatios.All(pair => pair.Value.Item1 > 0f), "Worthless trash error");

            // Lambda for normalizing what's left of the dict
            Action normalize = () => {
                var ratioSum = materialCostsAndDesiredCostRatios.Values.Sum(t => t.Item2);
                materialCostsAndDesiredCostRatios = materialCostsAndDesiredCostRatios.ToDictionary(p => p.Key, p => Tuple.Create(p.Value.Item1, p.Value.Item2 / ratioSum));
            };
            normalize();

            // Convert target cost ratios to target costs
            var targetCosts = materialCostsAndDesiredCostRatios.ToDictionary(p => p.Key, p => targetCost * p.Value.Item2);

            // Sort by material value (descending)
            var sorted = materialCostsAndDesiredCostRatios.Keys.OrderByDescending(k => materialCostsAndDesiredCostRatios[k].Item1).ToArray();            

            var result = new Dictionary<string, int>();

            foreach (var key in targetCosts.Keys.ToArray()) {
                var materialCost = materialCostsAndDesiredCostRatios[key].Item1;
                var targetCostForMaterial = targetCosts[key];

                // Add as many units as we can without overshooting the target
                var targetInUnits = targetCostForMaterial / materialCost;
                var asInt = (int)targetInUnits;

                // Round (if adding another unit would bring us closer to the target, do so)
                if (targetInUnits - asInt > 0.5f)
                    ++asInt;

                if (asInt > 0) {
                    result[key] = asInt;
                    targetCostForMaterial -= materialCost * asInt;
                }

                // Done with this material
                materialCostsAndDesiredCostRatios.Remove(key);

                // Have we failed to exactly match the target ratio? And are there any cheaper materials left?
                if (targetCostForMaterial != 0f && materialCostsAndDesiredCostRatios.Count > 0) {
                    // Yeah, distribute the remainder amongst said cheaper materials, according to their desired cost ratios
                    normalize();

                    materialCostsAndDesiredCostRatios.Keys.Do(k => targetCosts[k] += targetCostForMaterial * materialCostsAndDesiredCostRatios[k].Item2);
                }
            }

            Debug.Assert(result.Count > 0, "Unable to generate recipe (was the target cost less than half the value of the cheapest ingredient?");

            return result;
        }
    }
}

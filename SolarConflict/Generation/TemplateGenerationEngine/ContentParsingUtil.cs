using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Utils;
using System.Diagnostics;
using System.Globalization;
using XnaUtils;

namespace SolarConflict.Generation.TemplateGenerationEngine
{

    public class ContentParsingUtil
    {

        public static string GetStringBetween(string input)
        {
            return input.Split('(', ')')[1];
        }


        /*
# pickone{emitter:prop, emitter:prob, emitter...}
# all{emitter:prob[,emiter:prob[,...]]}
# loot{emitter:prob:amount:range, emitter:prob, emitter}
# all{#pickone{Mat1,Mat2}:0.5, #pickone{Mat2, Mat3, Mat4:2}}
# Param{paramname:value} - not implamnted 
*/

        public static IEmitter ParseEmitter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            input = input.Trim();
                //throw new ArgumentNullException("empty line instead of emmiter");
            if (!input.StartsWith("#"))
            {
                return ContentBank.Get(input);
            }
            var indexOfParanthisis = input.IndexOf("{");
            var type = input.Substring(1, indexOfParanthisis - 1);
            var subInput = input.Substring(indexOfParanthisis + 1,input.Length - indexOfParanthisis - 2);

            switch (type)
            {
                case "pickone":
                    return ParseGroupEmitter(GroupEmitter.EmitterType.RandomOne, subInput); 
                case "all":
                    return ParseGroupEmitter(GroupEmitter.EmitterType.All, subInput);
                case "loot":
                    return ParseLootEmitter(subInput);
                default:
                    throw new ArgumentNullException($"Unknown emiter type {type} in {input}");
                    //break;
            }
        }

        
        public static IEmitter ParseGroupEmitter(GroupEmitter.EmitterType type, string input)
        {
            var emitter = new GroupEmitter();
            emitter.EmitType = type;
            var children = StringUtils.SplitByTopLevelDelimiter(input, ',','{','}');
            foreach (var child in children)
            {
                int index = child.LastIndexOf('}');
                string emitterString;
                float prob = 1;
                if (index == -1)
                {
                    string[] splitChild = child.Split(':');
                    emitterString = splitChild[0];
                    if (splitChild.Length > 1)
                        prob = ParserUtils.ParseFloat(splitChild[1]);
                }
                else
                {
                    emitterString = child.Substring(0, index+1);
                    var paramString = child.Substring(index + 1);
                    string[] splitChild = paramString.Split(':');
                    if (splitChild.Length > 1)
                        prob = ParserUtils.ParseFloat(splitChild[1]);
                }
                IEmitter childEmitter = ParseEmitter(emitterString);
                emitter.AddEmitter(childEmitter, prob);
            }
            return emitter;
        }

        public static IEmitter ParseLootEmitter(string input)
        {
            var emitter = new LootEmitter();
            var children = StringUtils.SplitByTopLevelDelimiter(input, ',','{', '}');
            foreach (var child in children)
            {
                int index = child.LastIndexOf('}');
                string emitterString;
                string[] splitChild;
                float prob = 1;
                if (index == -1)
                {
                    splitChild = child.Split(':');
                    emitterString = splitChild[0];
                    if (splitChild.Length > 1)
                        prob = ParserUtils.ParseFloat(splitChild[1]);
                }
                else
                {
                    emitterString = child.Substring(0, index + 1);
                    var paramString = child.Substring(index + 1);
                    splitChild = paramString.Split(':');
                    
                }
                if (splitChild.Length > 1)
                    prob = ParserUtils.ParseFloat(splitChild[1]);
                int amountMin = 0;
                if (splitChild.Length > 2)
                    amountMin = ParserUtils.ParseInt(splitChild[2]);
                int amountRange = 0;
                if (splitChild.Length > 3)
                    amountRange = ParserUtils.ParseInt(splitChild[3]);
                IEmitter childEmitter = ParseEmitter(emitterString);
                emitter.AddEmitter(childEmitter, prob, amountMin, amountRange);
            }
            return emitter;
        }

        /// <remarks>Input is a comma-separated list of entries. Entries should have the form "weight probability mainExpression minAmount amountRange",
        /// where weight is a float preceded by the letter w, probability is a float, minAmount and amountRange are ints,
        /// and everything except mainExpression (see below) is optional. Weights should only occur if the input is part of a weighted choice expression (see below)
        /// 
        /// mainExpression can be:
        ///     * an emitter id
        ///     * another comma-separated list of entries, enclosed in square brackets
        ///     * a weighted choice of entries, enclosed in round brackets
        ///     
        /// Examples:
        ///     shield1 - just returns the shield1 emitter
        ///     shield1, 0.5 shield2 - emits shield1, also has an 50% chance of emitting shield2
        ///     shield1, 0.5 [shield2 2 1, shield3] - emits shield1, has a 50% chance of also emitting 2-3 instances of shield2 and one of shield3
        ///     shield1, 0.5 (w4 shield2 2, shield3) - emits shield1, has a 50% chance of emitting some additional loot, either two instances of shield2 or (1/5th of the time) one of shield3</remarks>
        public static IEmitter ParseOldEmitter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
            input = input.Trim();

            // Split comma-separated expressions
            var split = SplitInput(input);

            if (split.Count > 1)
            {
                // Multiple expressions found, combine them into one emitter               
                var group = new LootEmitter();
                foreach (var e in split)
                    group.AddEmitter(ParseExpression(e));

                return group;
                // TODO: could flatten hierarchy here, if any children are LootEmitters
            }

            // Just one expression found, parse it
            return ParseExpression(input);
        }

        static IEmitter ParseExpression(string input)
        {
            var splitBySpace = SplitInput(input, ' ');

            var probability = 1f;
            var minGenerated = 1;
            var rangeGenerated = 0;

            var index = 0;

            float parsedFloat;
            if (float.TryParse(splitBySpace[index], NumberStyles.Any, new CultureInfo("en-US"), out parsedFloat))
            {
                // First bit of the element is a float, should be our probability
                probability = parsedFloat;
                ++index;
            }

            var mainExpression = splitBySpace[index].Trim();
            index++;

            if (splitBySpace.Count > index)
            {
                // More space-separated stuff found after the main expression, should be the min amount to generate
                minGenerated = int.Parse(splitBySpace[index]);
                ++index;

                if (splitBySpace.Count > index)
                {
                    // Yet more stuff found, should be the generation amount range
                    rangeGenerated = int.Parse(splitBySpace[index++]);

                    Debug.Assert(splitBySpace.Count == index, "Too many spaces in expression");
                }
            }

            IEmitter result = null;

            if (mainExpression.StartsWith("["))
            {
                // Element is a group of subelements (comma-separated within square brackets)
                Debug.Assert(mainExpression.EndsWith("]"), "Bracket mismatch");

                result = ParseEmitter(mainExpression.Substring(1, mainExpression.Length - 2));
            }
            else if (mainExpression.StartsWith("("))
            {
                // Element is a weighted choice (comma-separated within round brackets)
                Debug.Assert(mainExpression.EndsWith(")"), "Bracket mismatch");

                var group = new GroupEmitter();
                foreach (var e in SplitInput(mainExpression.Substring(1, mainExpression.Length - 2)))
                {
                    var toParse = e.Trim();

                    group.EmitType = GroupEmitter.EmitterType.RandomOne;

                    var weight = 1f;

                    // Does the element begin with a weight expression (of the form "wf", where f is some float expression?)
                    var indexOfSpace = toParse.IndexOf(' ');
                    if (indexOfSpace > 0)
                    {
                        var weightExpression = toParse.Substring(0, indexOfSpace);
                        if (weightExpression[0] == 'w')
                        {
                            // Yeah. Parse the weight and ignore it when recurring 
                            weight = float.Parse(weightExpression.Substring(1), new CultureInfo("en-US"));

                            toParse = toParse.Substring(indexOfSpace + 1);
                        }
                    }

                    group.AddEmitter(ParseExpression(toParse), weight);
                }

                result = group;
            }
            else
                // Element should be an emitter name
                result = ContentBank.Inst.GetEmitter(mainExpression);

            if (probability != 1f || minGenerated != 1 || rangeGenerated != 0)
            {
                // Wrap the result in a LootEmitter (for probability or multiplicity)
                var wrapper = new LootEmitter();
                wrapper.AddEmitter(result, probability, minGenerated, rangeGenerated);

                return wrapper;
            }

            return result;
        }

        static List<string> SplitInput(string input, char separator = ',')
        {
            input = input.Trim();

            var result = new List<string>();

            var startIndex = 0;

            var roundBracketsOpen = 0;
            var squareBracketsOpen = 0;

            for (var i = 0; i < input.Length; ++i)
            {
                switch (input[i])
                {
                    // Open brackets
                    case '(':
                        ++roundBracketsOpen;
                        break;
                    case '[':
                        ++squareBracketsOpen;
                        break;
                    // Close brackets
                    case ')':
                        --roundBracketsOpen;
                        Debug.Assert(roundBracketsOpen >= 0, "Bracket mismatch");
                        break;
                    case ']':
                        --squareBracketsOpen;
                        Debug.Assert(squareBracketsOpen >= 0, "Bracket mismatch");
                        break;
                }

                // If no brackets open, split on comma
                if (input[i] == separator && squareBracketsOpen == 0 && roundBracketsOpen == 0)
                {
                    result.Add(input.Substring(startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }

            result.Add(input.Substring(startIndex));

            return result;
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace XnaUtils
{
    public static class StringUtils
    {

        public static bool IsPowerOfTwoOrZero(UInt64 x)
        {
            return (x & (x - 1)) == 0;
        }

        //public static string GetFullName(this Enum myEnum)
        //{
        //    return string.Format("{0}.{1}", myEnum.GetType().Name, myEnum.ToString());
        //}

        public static List<TextAsset> GetTextAssets(this Enum myEnum)
        {
            string[] names = myEnum.ToString().Split(',');
            List<TextAsset> asstes = new List<TextAsset>();
            for (int i = 0; i < names.Length; i++)
            {
                asstes.Add(TextBank.Inst.GetTextAsset(myEnum.GetType().Name + "." + names[i].Trim()));
            }
            return asstes;
        }

        public static string GetUserName(this Enum myEnum)
        {
            string[] names = myEnum.ToString().Split(',');
            StringBuilder sb = new StringBuilder(); // new StringBuilder(TextBank.Inst.GetTextAsset(names[0]).Text);
            for (int i = 0; i < names.Length; i++)
            {
                sb.Append(TextBank.Inst.GetTextAsset(myEnum.GetType().Name + "." + names[i].Trim()).Text + ", ");                
            }
            sb.Remove(sb.Length - 2, 2);            
            return sb.ToString();
        }

        public static string GetSpriteID(this Enum myEnum)
        {
            string[] names = myEnum.ToString().Split(',');
            return TextBank.Inst.GetTextAsset(myEnum.GetType().Name + "." + names[0].Trim()).ImageID;
        }

        public static string GetIconTag(this Enum myEnum)
        {
            string[] names = myEnum.ToString().Split(',');
            StringBuilder sb = new StringBuilder(); // new StringBuilder(TextBank.Inst.GetTextAsset(names[0]).Text);
            for (int i = 0; i < names.Length; i++)
            {
                sb.Append("#image{" + TextBank.Inst.GetTextAsset(myEnum.GetType().Name + "." + names[i].Trim()).ImageID + "}");
            }            
            return sb.ToString();
        }


        public static List<string> SplitByTopLevelDelimiter(string str, char delim, char open ='(', char close = ')')
        {
            List<string> res = new List<string>();
            int level = 0;
            int lastDelimiter = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == delim && level == 0)
                {
                    res.Add(str.Substring(lastDelimiter, i - lastDelimiter));
                    lastDelimiter = i + 1;
                }
                else if (str[i] == open) level++;
                else if (str[i] == close)
                {
                    level--;
                    if (level < 0)
                        throw new ArgumentException($"Unexpected delimiter in {str} position {i}");
                    
                }
            }
            res.Add(str.Substring(lastDelimiter));
            return res;
        }

        public static List<Tuple<string,float>> ParseStringFloatPair(string line)
        {
            List<Tuple<string, float>> tuples = new List<Tuple<string, float>>();
            var pairs = SplitAndTrim(line, ',');
            foreach (var item in pairs)
            {
                var idAndFloat = SplitAndTrim(item, ':');
                float value = 1;
                if(idAndFloat.Length > 1)
                {
                    value = ParserUtils.ParseFloat(idAndFloat[1], 1);
                }
                tuples.Add(new Tuple<string, float>(idAndFloat[0], value));
            }
            return tuples;
        }

        public static string[] SplitAndTrim(string line, char seperator = ',')
        {
            return Array.ConvertAll(line.Split(seperator), p => p.Trim());
        }

        public static string JoinNonNullsCustomSeparator(string separator, params string[] inputs)
        {
            return string.Join(separator, inputs.Where(s => s != null));
        }

        public static string JoinNonNulls(params string[] inputs)
        {
            return JoinNonNullsCustomSeparator("\n", inputs);
        }
        public static List<string> DictioneryToStringList(Dictionary<string, string> dictonery, char seperator = ',')
        {
            if (dictonery == null)
                return null;
            List<string> stringList = new List<string>(dictonery.Count); //TODO: needs to check thet key dosn't contain sperator
            foreach (var item in dictonery)
            {
                stringList.Add(item.Key + seperator.ToString() + item.Value);
            }
            return stringList;
        }

        public static Dictionary<string,string> ListToStringDictionery(IEnumerable<string> collection, char sperator = ',')
        {
            if (collection == null)
                return null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in collection)
            {
                if(!string.IsNullOrWhiteSpace(item))
                {
                    string[] keyItem = item.Split(new char[] { sperator }, 2);
                    dic.Add(keyItem[0], keyItem[1]);
                    if (keyItem.Count() != 2)
                        throw new Exception("ListToStringDictionery error, split was returned: " + keyItem.Count().ToString() + " Elements");
                }
            }
            return dic;
        }

        public static bool IsOneOf(string value, string[] oneOfValues)
        {
            for (int i = 0; i < oneOfValues.Length; i++)
            {
                if (value.Equals(oneOfValues[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static string AddSpacesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static string ReplaceUnderScore(this string value) //TODO:remove
        {
            return value.Replace("_", " ");
        }

        public static Sprite ToSprite(this string textureID)
        {
            return Sprite.Get(textureID);
        }

        public static string CapFirst(string text)
        {

            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}

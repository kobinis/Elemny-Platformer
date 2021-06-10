using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XnaUtils
{
    class StringHelper
    {

        public static string ParseString(string text)
        {
            if (!string.IsNullOrWhiteSpace(text)) //TODO: Replace /a1... /up ../q1 .. with key name 
            {                 
                return text.Replace(@"\n", "\n"); ;
            }
            return null;
        }

        public static string MultipleReplace(string text, Dictionary<string,string> replacements) //TODO: check
        {
            return Regex.Replace(text,
                                    "(" + String.Join("|", replacements.Keys.ToArray()) + ")",
                                    delegate (Match m) { return replacements[m.Value]; }
                                    );

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    static class ExtensionMethods
    {
        public static string ReplaceAt( this string changeStr, int index, char newChar )
        {
            try
            {
                char[] strArray = changeStr.ToCharArray();

                if (index < strArray.Length)
                {
                    strArray[index] = newChar;
                    return new string(strArray);
                }

                return "!".ToString();
            }
            catch
            {
                Console.WriteLine("ReplaceAt() Exception in ExtensionMethods.cs");
                return "".ToString();
            }
        }
    }
}

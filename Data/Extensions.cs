using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Data
{
    //This class is meant to hold all the extension methods we might require in our code -n.st
    public static class Extensions
    {
        /// <summary>
        /// Finds all indices of a substring in a given string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <param name="value">The substring to find.</param>
        /// <returns>Returns a list of all indices of the substring in the given string.</returns>
        public static IEnumerable<int> AllIndicesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("str cant be null or empty");
            }

            //iterator based return (to save the need for a List allocation)
            for (var index = 0;; index += 1)
            {
                index = str.IndexOf(value, index, StringComparison.Ordinal);
                if (index == -1)
                    break;
                yield return index;
            }
        }
        
        public static string GetSha1HashAsHexString(this string input)
        {
            var hashProvider = new SHA1CryptoServiceProvider();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashedBytes = hashProvider.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        }
    }
    
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Notepad
{
    public static class TextProp
    {
        public static int WordCount;
        public static int LettersCount;
        public static int Palindroms;
        public static string Vowels;

        static void Palindrome(string text)
        {
            string reverseText = Reverse(text);
            if (reverseText.Equals(text, StringComparison.CurrentCultureIgnoreCase))
                Palindroms++;
        }

        private static string Reverse(string text)
        {
            StringBuilder tempValue = new StringBuilder();
            for (int i = text.Length - 1; i >= 0; i--)
            {
                tempValue.Append(text[i]);
            }
            return tempValue.ToString();
        }

        public static void VowelCounter(string temp)
        {
            temp = temp.ToLower();
            Vowels = "";
            string pattern = @"[a e i o u]";
            Regex regex = new Regex(pattern);
            Dictionary<char, int> vowels = new Dictionary<char, int>();
            vowels.Add('a', 0); vowels.Add('e', 0); vowels.Add('i', 0); vowels.Add('o', 0); vowels.Add('u', 0);
            vowels['a'] = vowels['a']++;
            
            //CountEveryOccurence
            foreach (char letter in temp)
            {
                if (letter != ' ' && regex.IsMatch(letter.ToString()))
                {
                    vowels[letter] = vowels[letter] + 1;
                }
            }

            foreach (var value in vowels)
            {
                Vowels += value.Key + ": " + value.Value + " | ";
            }
        }
    }
}

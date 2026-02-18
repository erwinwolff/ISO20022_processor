using System;

namespace Wolff.FinanceTools.Base
{
    public class CalculationBase
    {
        /// <summary>
        /// The bban value must be in the format: [bank code][account number][iso 2 char country code]00
        /// </summary>
        /// <param name="bban"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GenerateCheckDigitsFromBban(string bban)
        {
            if (string.IsNullOrEmpty(bban))
                throw new ArgumentException("BBAN cannot be null or empty.", nameof(bban));

            string convertedBban = "";

            foreach (char ch in bban)
            {
                if (ch.IsAsciiLetter())
                {
                    convertedBban += ch - 55;
                }
                else
                {
                    convertedBban += ch;
                }
            }

            int firstMod97 = int.Parse(convertedBban.Substring(0, 9)) % 97;
            convertedBban = convertedBban.Replace(convertedBban.Substring(0, 9), string.Empty);

            int movingMod97 = firstMod97;
            while (convertedBban.Length > 7)
            {
                int mod97 = int.Parse(movingMod97.ToString() + convertedBban.Substring(0, 7)) % 97;
                convertedBban = convertedBban.Replace(convertedBban.Substring(0, 7), string.Empty);

                movingMod97 = mod97;
            }

            int resultMod97 = 98 - (int.Parse(movingMod97.ToString() + convertedBban) % 97);
            string checkDigits = resultMod97.ToString("D2");
            return checkDigits;
        }
    }

    internal static class Mod9710_Tools
    {
        public static bool IsAsciiLetter(this char c)
        {
            c |= ' ';
            return IsInRange(c, 'a', 'z');
        }

        private static bool IsInRange(char c, char min, char max)
        {
            return (uint)c - (uint)min <= (uint)max - (uint)min;
        }
    }
}
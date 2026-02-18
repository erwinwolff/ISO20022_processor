using System;

namespace Wolff.FinanceTools.UK
{
    public class UkAccntNumberToIban
    {
        public string Convert(string bic, string sortCode, string accountNumber)
        {
            if (string.IsNullOrEmpty(bic))
                throw new ArgumentException("BIC cannot be null or empty.", nameof(bic));

            if (string.IsNullOrEmpty(sortCode))
                throw new ArgumentException("SortCode cannot be null or empty.", nameof(sortCode));

            if (string.IsNullOrEmpty(accountNumber))
                throw new ArgumentException("AccountNumber cannot be null or empty.", nameof(accountNumber));

            bic = bic.Substring(0, 4).ToUpper();
            sortCode = sortCode.Replace("-", "").Replace(" ", "");
            accountNumber = accountNumber.Replace("-", "").Replace(" ", "");

            if (sortCode.Length != 6)
                throw new ArgumentException("SortCode must be 6 digits long.", nameof(sortCode));

            if (accountNumber.Length != 8)
                throw new ArgumentException("AccountNumber must be 8 digits long.", nameof(accountNumber));

            string iban = $"{bic}{sortCode}{accountNumber}GB00";
            string convertedIban = "";
            
            foreach (char ch in iban)
            {
                if (ch.IsAsciiLetter())
                {
                    convertedIban += ch - 55;
                }
                else
                {
                    convertedIban += ch;
                }
            }

            string remainder = convertedIban;
            int lastMod97 = 0;

            int firstMod97 = int.Parse(remainder.Substring(0, 9)) % 97;
            remainder = remainder.Replace(remainder.Substring(0, 9), string.Empty);

            lastMod97 = firstMod97;

            while (remainder.Length > 7)
            {
                int mod97 = int.Parse(lastMod97.ToString("D2") + remainder.Substring(0, 7)) % 97;
                remainder = remainder.Replace(remainder.Substring(0, 7), string.Empty);

                lastMod97 = mod97;
            }

            int resultMod97 = 98 - (int.Parse(lastMod97.ToString() + remainder) % 97);
            string checkDigits = resultMod97.ToString("D2");

            iban = $"GB{checkDigits}{bic}{sortCode}{accountNumber}";

            if (iban.Length != 22)
                throw new ArgumentException("The resulting IBAN must be 22 characters long.");

            return iban;
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
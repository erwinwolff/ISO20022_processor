using System;
using Wolff.FinanceTools.Base;

namespace Wolff.FinanceTools.UK
{
    public class UkAccntNumberToIban : CalculationBase, IUkAccntNumberToIban
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

            string bban = $"{bic}{sortCode}{accountNumber}GB00";

            string checkDigits = GenerateCheckDigitsFromBban(bban);

            string iban = $"GB{checkDigits}{bic}{sortCode}{accountNumber}";

            if (iban.Length != 22)
                throw new ArgumentException("The resulting IBAN must be 22 characters long.");

            return iban;
        }
    }
}
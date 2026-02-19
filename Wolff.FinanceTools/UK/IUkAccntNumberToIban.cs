namespace Wolff.FinanceTools.UK
{
    public interface IUkAccntNumberToIban
    {
        /// <summary>
        /// Convert UK account parameters to international IBAN format
        /// </summary>
        /// <param name="bic"></param>
        /// <param name="sortCode"></param>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        string Convert(string bic, string sortCode, string accountNumber);
    }
}
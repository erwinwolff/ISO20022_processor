using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wolff.FinanceTools.UK;

namespace ISO20022.Tests.Wolff
{
    [TestClass]
    public class CalcIbanTests
    {
        [TestMethod]
        public void CalculateIban_Test_Success()
        {
            // input:        60-16-13 31926819
            // output local: NWBK60161331926819
            // output iban:  GB29NWBK60161331926819
            UkAccntNumberToIban ukAccntNumberToIban = new UkAccntNumberToIban();

            var iban = ukAccntNumberToIban.Convert("NWBK", "601613", "31926819");

            Assert.AreEqual("GB29NWBK60161331926819", iban);
        }
    }
}
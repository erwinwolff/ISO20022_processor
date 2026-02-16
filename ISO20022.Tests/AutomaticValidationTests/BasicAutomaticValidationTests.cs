using ISO20022.Validator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ISO20022.Tests.AutomaticValidationTests
{
    [TestClass]
    public class BasicAutomaticValidationTests
    {
        [TestInitialize]
        public void InitClass()
        {
            // warm up the binary and load the XSDs into memory before running the tests
            XmlISOValidator xmlISOValidator = new XmlISOValidator();
        }

        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.001.001.12")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.002.001.14")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.007.001.12")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.008.001.11")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.009.001.08")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.010.001.08")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.011.001.08")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.012.001.08")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.013.001.11")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.014.001.11")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.017.001.04")]
        [DataRow("urn:iso:std:iso:20022:tech:xsd:pain.018.001.04")]
        [DataTestMethod]
        public void SpecificSchema_Test_Success(string urn)
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();
            
            var type = xmlISOValidator.SchemaToType(urn);
            var fullyCreatedType = Activator.CreateInstance(type);

            Assert.IsNotNull(fullyCreatedType);

            fullyCreatedType.InflateXmlPocoDefinition();
            XmlSerializer serializer = new XmlSerializer(type);

            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, fullyCreatedType);
                string xmlOutput = textWriter.ToString();
                Assert.IsNotEmpty(xmlOutput);
            }
        }

        [TestMethod]
        [Ignore("This test is just to print out the loaded schemas and their namespaces, not an actual unit test.")]
        public async Task AllSchemasIntoString_Success()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();
            string xsdList = string.Empty;

            foreach (XmlSchema schema in XmlISOValidator.Schemas.Schemas())
            {
                xsdList += $"{schema.TargetNamespace}\n";
            }
        }

        [TestMethod]
        public async Task GetSchemaUrns_Method_Success()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();
            var urns = xmlISOValidator.GetSchemaUrns();

            Assert.IsNotNull(urns);
            Assert.IsTrue(urns.Any());
        }

        [TestMethod]
        public async Task TestCamt_053_001_01_Xml_Success()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();

            var result = await xmlISOValidator.AutomaticValidationAsync(@"
<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:camt.053.001.13"">
<BkToCstmrStmt>
  <GrpHdr>
    <MsgId>235549650</MsgId>
    <CreDtTm>2023-10-05T14:43:51.979</CreDtTm>
    <MsgRcpt>
      <Nm>Test Client Ltd.</Nm>
      <Id>
        <OrgId>
          <Othr>
            <Id>test001</Id>
          </Othr>
        </OrgId>
      </Id>
    </MsgRcpt>
    <AddtlInf>AddTInf</AddtlInf>
  </GrpHdr>
  <Stmt>
    <Id>258158850</Id>
    <ElctrncSeqNb>1</ElctrncSeqNb>
    <LglSeqNb>1</LglSeqNb>
    <CreDtTm>2023-10-05T14:43:52.098</CreDtTm>
    <FrToDt>
      <FrDtTm>2023-09-30T20:00:00.000</FrDtTm>
      <ToDtTm>2023-10-01T19:59:59.000</ToDtTm>
    </FrToDt>
    <Acct>
      <Tp>
        <Prtry>IBDA_DDA</Prtry>
      </Tp>
      <Ccy>USD</Ccy>
      <Nm>Sample Name 123</Nm>
      <Svcr>
        <FinInstnId>
          <BICFI>GSCRUS30</BICFI>
          <Nm>Goldman Sachs Bank</Nm>
        </FinInstnId>
      </Svcr>
    </Acct>
    <Bal>
      <Tp>
        <CdOrPrtry>
          <Cd>OPBD</Cd>
        </CdOrPrtry>
      </Tp>
      <Amt Ccy=""USD"">843686.20</Amt>
      <CdtDbtInd>DBIT</CdtDbtInd>
      <Dt>
        <DtTm>2023-09-30T20:00:00.000</DtTm>
      </Dt>
    </Bal>
    <Bal>
      <Tp>
        <CdOrPrtry>
          <Cd>CLAV</Cd>
        </CdOrPrtry>
      </Tp>
      <Amt Ccy=""USD"">334432401.27</Amt>
      <CdtDbtInd>CRDT</CdtDbtInd>
      <Dt>
        <DtTm>2023-10-01T23:59:00.000Z</DtTm>
      </Dt>
    </Bal>
  </Stmt>
</BkToCstmrStmt>
</Document>
");

            Assert.IsTrue(result.Item1);
            Assert.AreEqual("urn:iso:std:iso:20022:tech:xsd:camt.053.001.13", result.Item2);
        }

        [TestMethod]
        public async Task TestCamt_053_001_01_Xml_Fail()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();

            var result = await xmlISOValidator.AutomaticValidationAsync(@"
<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:camt.053.001.10"">
<BkToCstmrStmt>
  <GrpHdr>
    <MsgId>235549650</MsgId>
    <AddtlInf>AddTInf</AddtlInf>
  </GrpHdr>
  <Stmt>
    <Idz>258158850</Idz>
    <ElctrncSeqNb>1</ElctrncSeqNb
    <LglSeqNb>1</LglSeqNb>
    <CreDtTm>2023-10-05T14:43:52.098</CreDtTm>
    <FrToDt>
      <FrDtTm>2023-09-30T20:00:00.000</FrDtTm>
      <ToDtTm>2023-10-01T19:59:59.000</ToDtTm>
    </FrToDt>
    <Acct>
      <Tp>
        <Prtry>IBDA_DDA</Prtry>
      </Tp>
      <Ccy>USD</Ccy>
      <Nm>Sample Name 123</Nm>
    </Acct>
    <Bal>
      <Tp>
        <CdOrPrtry>
          <Cd>OPBD</Cd>
        </CdOrPrtry>
      </Tp>
      <Amt Ccy=""USD"">843686.20</Amt>
      <CdtDbtInd>DBIT</CdtDbtInd>
      <Dt>
        <DtTm>2023-09-30T20:00:00.000</DtTm>
      </Dt>
    </Bal>
    <Bal>
      <Tp>
        <CdOrPrtry>
          <Cd>CLAV</Cd>
        </CdOrPrtry>
      </Tp>
      <Amt Ccy=""USD"">334432401.27</Amt>
      <CdtDbtInd>CRDT</CdtDbtInd>
      <Dt>
        <DtTm>2023-10-01T23:59:00.000Z</DtTm>
      </Dt>
    </Bal>
  </Stmt>
</BkToCstmrStmt>
</Document>
");

            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Invalid Xml", result.Item2);
        }

        [TestMethod]
        public async Task TestCamt_053_001_01_Xml_2_Fail()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();

            var result = await xmlISOValidator.AutomaticValidationAsync(@"
<Documentz xmlns=""urn:iso:std:iso:20022:tech:xsd:camt.053.001.13"">
<BkToCstmrStmt>
  <GrpHdr>
    <MsgIdz>235549650</MsgIdz>
    <AddtlInf>AddTInf</AddtlInf>
  </GrpHdr>
  <Stmt>
    <Idz>258158850</Idz>
    <ElctrncSeqNb>1</ElctrncSeqNb>
    <LglSeqNb>1</LglSeqNb>
    <CreDtTm>2023-10-05T14:43:52.098</CreDtTm>
    <FrToDt>
      <FrDtTm>2023-09-30T20:00:00.000</FrDtTm>
      <ToDtTm>2023-10-01T19:59:59.000</ToDtTm>
    </FrToDt>
    <Acct>
      <Tp>
        <Prtry>IBDA_DDA</Prtry>
      </Tp>
      <Ccy>USD</Ccy>
      <Nm>Sample Name 123</Nm>
    </Acct>
    <Bal>
      <Tp>
        <CdOrPrtry>
          <Cd>OPBD</Cd>
        </CdOrPrtry>
      </Tp>
      <Amt Ccy=""USD"">843686.20</Amt>
      <CdtDbtInd>DBIT</CdtDbtInd>
      <Dt>
        <DtTm>2023-09-30T20:00:00.000</DtTm>
      </Dt>
    </Bal>
    <Bal>
      <Tp>
        <CdOrPrtry>
        </CdOrPrtry>
      </Tp>
      <Amt Ccy=""USD"">334432401.27</Amt>
      <CdtDbtInd>CRDT</CdtDbtInd>
      <Dt>
        <DtTm>2023-10-01T23:59:00.000Z</DtTm>
      </Dt>
    </Bal>
  </Stmt>
</BkToCstmrStmt>
</Documentz>
");

            Assert.IsFalse(result.Item1);
            Assert.AreEqual("The 'urn:iso:std:iso:20022:tech:xsd:camt.053.001.13:Documentz' element is not declared.", result.Item2);
        }

        [TestMethod]
        public async Task TestCamt_053_001_01_Xml_3_Fail()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();

            var result = await xmlISOValidator.AutomaticValidationAsync(@"");

            Assert.IsFalse(result.Item1);
            Assert.AreEqual("Empty Xml", result.Item2);
        }

        [TestMethod]
        public async Task TestPain_001_001_12_Success()
        {
            string xmlFile = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.12"" 
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
xsi:schemaLocation=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.12 pain.001.001.12.xsd"">
  <CstmrCdtTrfInitn>
    <GrpHdr>
      <MsgId>Message-ID-4711</MsgId>
      <CreDtTm>2010-11-11T09:30:47.000Z</CreDtTm>
      <NbOfTxs>2</NbOfTxs>
      <InitgPty>
        <Nm>Initiator Name</Nm>
      </InitgPty>
    </GrpHdr>
    <PmtInf>
      <PmtInfId>Payment-Information-ID-4711</PmtInfId>
      <PmtMtd>TRF</PmtMtd>
      <BtchBookg>true</BtchBookg>
      <NbOfTxs>2</NbOfTxs>
      <CtrlSum>6655.86</CtrlSum>
      <PmtTpInf>
        <SvcLvl>
          <Cd>SEPA</Cd>
        </SvcLvl>
      </PmtTpInf>
      <ReqdExctnDt><Dt>2010-11-25</Dt></ReqdExctnDt>
      <Dbtr>
        <Nm>Debtor Name</Nm>
      </Dbtr>
      <DbtrAcct>
        <Id>
          <IBAN>DE87200500001234567890</IBAN>
        </Id>
      </DbtrAcct>
      <DbtrAgt>
        <FinInstnId>
          <Nm>Debitor Bank Name</Nm>
        </FinInstnId>
      </DbtrAgt>
      <ChrgBr>SLEV</ChrgBr>
      <CdtTrfTxInf>
        <PmtId>
          <EndToEndId>OriginatorID1234</EndToEndId>
        </PmtId>
        <Amt>
          <InstdAmt Ccy=""EUR"">6543.14</InstdAmt>
        </Amt>
        <CdtrAgt>
          <FinInstnId>
            <Nm>Creditor Bank Name</Nm>
          </FinInstnId>
        </CdtrAgt>
        <Cdtr>
          <Nm>Creditor Name</Nm>
        </Cdtr>
        <CdtrAcct>
          <Id>
            <IBAN>DE21500500009876543210</IBAN>
          </Id>
        </CdtrAcct>
        <RmtInf>
          <Ustrd>Unstructured Remittance Information</Ustrd>
        </RmtInf>
      </CdtTrfTxInf>
      <CdtTrfTxInf>
        <PmtId>
          <EndToEndId>OriginatorID1235</EndToEndId>
        </PmtId>
        <Amt>
          <InstdAmt Ccy=""EUR"">112.72</InstdAmt>
        </Amt>
        <CdtrAgt>
          <FinInstnId>
         <Nm>Creditor Agent Bank Name</Nm>
          </FinInstnId>
        </CdtrAgt>
        <Cdtr>
          <Nm>Other Creditor Name</Nm>
        </Cdtr>
        <CdtrAcct>
          <Id>
            <IBAN>DE21500500001234567897</IBAN>
          </Id>
        </CdtrAcct>
        <RmtInf>
          <Ustrd>Unstructured Remittance Information</Ustrd>
        </RmtInf>
      </CdtTrfTxInf>
    </PmtInf>
  </CstmrCdtTrfInitn>
</Document>";

            XmlISOValidator xmlISOValidator = new XmlISOValidator();
            var res = await xmlISOValidator.AutomaticValidationAsync(xmlFile);

            Console.WriteLine($"{res.Item2}");
            Assert.IsTrue(res.Item1);

            XmlSerializer serializerPain = new XmlSerializer(typeof(Pain_001_001_12.Document));

            var painDocument = (Pain_001_001_12.Document)serializerPain.Deserialize(new StringReader(xmlFile));

            Assert.IsNotNull(painDocument);
        }
    }
}
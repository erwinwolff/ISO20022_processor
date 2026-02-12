using ISO20022.Validator;

namespace ISO20022.Tests.AutomaticValidationTests
{
    [TestClass]
    public class BasicAutomaticValidationTests
    {
        [TestInitialize]
        public void InitClass()
        {
            XmlISOValidator xmlISOValidator = new XmlISOValidator();
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
    }
}
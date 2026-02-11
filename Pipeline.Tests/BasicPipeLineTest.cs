namespace Pipeline.Tests;

[TestClass]
public class BasicPipeLineTest
{
    [TestMethod]
    public void BootstrapPipeline()
    {
        TestPipeline testPipeline = new TestPipeline();

        testPipeline.ExecuteAsync();

        Assert.IsTrue(TestStep.WasExecuted);
    }
}
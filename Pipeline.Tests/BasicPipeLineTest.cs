using Pipeline.Tests.TestImp;

namespace Pipeline.Tests;

[TestClass]
public class BasicPipeLineTest
{
    [TestMethod]
    public async Task BootstrapPipeline_Success()
    {
        TestPipeline testPipeline = new TestPipeline();

        await testPipeline.ExecuteAsync();

        Assert.IsTrue(TestStep.WasExecuted);
    }

    [TestMethod]
    public async Task ExceptionPipeline_NoException_InProcess_Success()
    {
        ExceptionPipeline testPipeline = new ExceptionPipeline();

        await testPipeline.ExecuteAsync();

        Assert.IsFalse(ExceptionStep.WasExecuted);
    }
}
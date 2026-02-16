using Pipeline.Abstract.Interfaces;
using Pipeline.Implementation.Basic;
using Polly;

namespace Pipeline.Tests.TestImp
{
    public class TestPipeline : BasicAsyncPipeline, IPipeline<TestStep>
    {
        public TestPipeline()
        {
        }

        public TestPipeline(AsyncPolicy policy) : base(policy)
        {
        }
    }
}
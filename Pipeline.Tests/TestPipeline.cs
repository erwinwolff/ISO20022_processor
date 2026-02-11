using Pipeline.Abstract.Interfaces;
using Pipeline.Implementation.Basic;

namespace Pipeline.Tests
{
    public class TestPipeline : BasicAsyncPipeline, IPipeline<TestStep>
    {
    }
}
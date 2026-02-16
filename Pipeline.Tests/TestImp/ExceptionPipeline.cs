using Pipeline.Abstract.Interfaces;
using Pipeline.Implementation.Basic;
using Polly;

namespace Pipeline.Tests.TestImp
{
    public class ExceptionPipeline : BasicAsyncPipeline, IPipeline<ExceptionStep>
    {
        public ExceptionPipeline()
        {
        }

        public ExceptionPipeline(AsyncPolicy policy) : base(policy)
        {
        }
    }
}
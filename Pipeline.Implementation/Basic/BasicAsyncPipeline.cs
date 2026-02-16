using Pipeline.Abstract.Interfaces;
using Polly;

namespace Pipeline.Implementation.Basic
{
    public abstract class BasicAsyncPipeline : IPipeline
    {
        private AsyncPolicy LocalRetryPolicy { get; set; }

        public BasicAsyncPipeline()
        {
            LocalRetryPolicy = Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    ;
        }

        public BasicAsyncPipeline(AsyncPolicy policy)
        {
            if (policy == null)
            {
                LocalRetryPolicy = Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    ;
            }
            else
            {
                LocalRetryPolicy = policy;
            }
        }

        public async Task ExecuteAsync()
        {
            foreach (var step in GetSteps())
            {
                await LocalRetryPolicy.ExecuteAndCaptureAsync(async () =>
                {
                    await step.ExecuteAsync();
                });
            }
        }

        public List<IStep> GetSteps()
        {
            List<IStep> steps = new List<IStep>();

            var myPipelineGenericInterface = GetType()
                .GetInterfaces()
                .First(x => x.IsGenericType && x.IsInterface);

            var genericDefinedTypes = myPipelineGenericInterface
                .GetGenericArguments();

            if (!genericDefinedTypes.Any())
                return new List<IStep>();

            foreach (var genericDefinedType in genericDefinedTypes)
            {
                var step = Activator.CreateInstance(genericDefinedType) as IStep;
                if (step != null)
                {
                    steps.Add(step);
                }
            }

            return steps;
        }
    }
}
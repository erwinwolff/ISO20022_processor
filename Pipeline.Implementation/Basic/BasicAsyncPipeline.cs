using Pipeline.Abstract.Interfaces;
using Polly;

namespace Pipeline.Implementation.Basic
{
    public abstract class BasicAsyncPipeline : IPipeline
    {
        public BasicAsyncPipeline()
        {
        }

        public async void ExecuteAsync()
        {
            foreach (var step in GetSteps())
            {
                await Policy
                    .Handle<Exception>()
                    .RetryAsync(2)
                    .ExecuteAsync(async () =>
                {
                    await step.ExecuteAsync();
                });
            }
        }

        public List<IStep> GetSteps()
        {
            return new List<IStep>();
        }
    }
}
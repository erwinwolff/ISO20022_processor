using Pipeline.Abstract.Interfaces;

namespace Pipeline.Tests.TestImp
{
    public class TestStep : IStep
    {
        public static bool WasExecuted { get; set; }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken);

            WasExecuted = true;
        }
    }
}
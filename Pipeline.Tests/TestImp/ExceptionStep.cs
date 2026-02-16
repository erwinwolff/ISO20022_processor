using Pipeline.Abstract.Interfaces;

namespace Pipeline.Tests.TestImp
{
    public class ExceptionStep : IStep
    {
        public static bool WasExecuted { get; set; }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            throw new ArgumentException("This step will never work");

#pragma warning disable CS0162 // Unreachable code detected
            WasExecuted = true;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}

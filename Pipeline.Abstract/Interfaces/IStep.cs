using System.Threading;
using System.Threading.Tasks;

namespace Pipeline.Abstract.Interfaces
{
    public interface IStep
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
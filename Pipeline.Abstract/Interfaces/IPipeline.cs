using System.Threading.Tasks;

namespace Pipeline.Abstract.Interfaces
{
    public interface IPipeline
    {
        Task ExecuteAsync();
    }

    public interface IPipeline<T1>
        : IPipeline
        where T1 : class, IStep
        
    {
    }

    public interface IPipeline<T1, T2>
        : IPipeline
        where T1 : class, IStep
        where T2 : class, IStep
    {
    }

    public interface IPipeline<T1, T2, T3>
        : IPipeline
        where T1 : class, IStep
        where T2 : class, IStep
        where T3 : class, IStep
    {
    }

    public interface IPipeline<T1, T2, T3, T4>
        : IPipeline
        where T1 : class, IStep
        where T2 : class, IStep
        where T3 : class, IStep
        where T4 : class, IStep
    {
    }

    public interface IPipeline<T1, T2, T3, T4, T5>
        : IPipeline
        where T1 : class, IStep
        where T2 : class, IStep
        where T3 : class, IStep
        where T4 : class, IStep
        where T5 : class, IStep
    {
    }

    public interface IPipeline<T1, T2, T3, T4, T5, T6>
      : IPipeline
      where T1 : class, IStep
      where T2 : class, IStep
      where T3 : class, IStep
      where T4 : class, IStep
      where T5 : class, IStep
      where T6 : class, IStep
    {
    }
}
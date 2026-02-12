using System.Threading.Tasks;

namespace ISO20022.Interfaces
{
    public interface IXmlISOValidator
    {
        Task<(bool, string)> AutomaticValidationAsync(string xmlContent);
    }
}
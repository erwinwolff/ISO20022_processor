using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISO20022.Interfaces
{
    public interface IXmlISOValidator
    {
        /// <summary>
        /// Validate the provided XML content against the loaded ISO 20022 XSD schemas.
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        Task<(bool, string)> AutomaticValidationAsync(string xmlContent);

        /// <summary>
        /// Get all the schema URNs that are loaded in the validator. This can be used to display to users which schemas are available for validation or for debugging purposes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetSchemaUrns();
    }
}
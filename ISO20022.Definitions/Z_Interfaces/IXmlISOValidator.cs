using System;
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

        /// <summary>
        /// Reverse engineer the provided schema URN to determine the corresponding .NET type that represents the ISO 20022 message structure defined by that schema. This allows for dynamic deserialization of XML messages into strongly-typed objects based on their schema URN.
        /// </summary>
        /// <param name="urn"></param>
        /// <returns></returns>
        Type SchemaToType(string urn);
    }
}
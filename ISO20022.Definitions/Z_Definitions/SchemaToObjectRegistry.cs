using ISO20022.Definitions.Interfaces;
using System;
using System.Collections.Generic;

namespace ISO20022.Definitions.Definitions
{
    public class SchemaToObjectRegistry : ISchemaToObjectRegistry
    {
        public Dictionary<string, Type> SchemaToObjectMap => new Dictionary<string, Type>
        {
            { "urn:iso:std:iso:20022:tech:xsd:pain.001.001.12", typeof(Pain_001_001_12.Document) } 
        };
    }
}
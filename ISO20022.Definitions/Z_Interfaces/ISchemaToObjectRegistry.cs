using System;
using System.Collections.Generic;

namespace ISO20022.Definitions.Interfaces
{
    public interface ISchemaToObjectRegistry
    {
        Dictionary<string, Type> SchemaToObjectMap { get; }
    }
}
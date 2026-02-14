using ISO20022.Definitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace ISO20022.Definitions.Definitions
{
    public class SchemaToObjectRegistry : ISchemaToObjectRegistry
    {
        public SchemaToObjectRegistry()
        {
            var assembly = Assembly.Load("ISO20022.Definitions");
            var allTypes = assembly
                .GetTypes()
                .Where(x => x.GetCustomAttributes().Any(y => y.GetType() == typeof(XmlRootAttribute)))
                ;

            foreach (var type in allTypes)
            {
                var xmlrootAttribute = type.GetCustomAttribute<XmlRootAttribute>();
                SchemaToObjectMap.Add(xmlrootAttribute.Namespace, type);
            }
        }

        public Dictionary<string, Type> SchemaToObjectMap { get; set; } = new Dictionary<string, Type>();
    }
}
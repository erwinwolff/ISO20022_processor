using ISO20022.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ISO20022.Validator
{
    public class XmlISOValidator : IXmlISOValidator
    {
        static XmlISOValidator()
        {
            var assembly = Assembly.Load("ISO20022.Definitions");
            var resources = assembly
                .GetManifestResourceNames()
                .Where(x => x.EndsWith(".xsd"))
                .ToArray();

            List<string> xsdContents = new List<string>();
            foreach (var resource in resources)
            {
                using (var stream = assembly.GetManifestResourceStream(resource))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var xsdContent = reader.ReadToEnd();
                        xsdContents.Add(xsdContent);
                    }
                }
            }

            Xsds = xsdContents.ToArray();
            Schemas = new XmlSchemaSet();

            foreach (var xsd in Xsds)
            {
                using (var reader = XmlReader.Create(new StringReader(xsd)))
                {
                    Schemas.Add(reader.GetAttribute("targetNamespace"), reader);
                }
            }

            Schemas.Compile();
        }

        public static string[] Xsds;
        public static XmlSchemaSet Schemas;

        public async Task<(bool, string)> AutomaticValidationAsync(string xmlContent)
        {
            if (Schemas.Count == 0)
                return (false, "No XSD schemas loaded in the validator");

            if (string.IsNullOrWhiteSpace(xmlContent))
                return (false, "Empty Xml");

            XDocument xdoc;
            try
            {
                xdoc = XDocument.Parse(xmlContent);
            }
            catch
            {
                return (false, "Invalid Xml");
            }
            
            var namespaceAttribute = string.Empty;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            namespaceAttribute = xdoc
                .Root
                .Attributes()
                .FirstOrDefault(a => a.IsNamespaceDeclaration)
                .ToString();
            namespaceAttribute = namespaceAttribute
                .Replace("xmlns=\"", string.Empty)
                .Replace("\"", string.Empty)
                .Trim();

            foreach (XmlSchema schema in Schemas.Schemas())
                if (schema.TargetNamespace == namespaceAttribute)
                    settings.Schemas.Add(schema);

            if (settings.Schemas.Count == 0)
                return (false, "No matching XSD found for the provided ISO20022 message");

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent), settings))
            {
                XmlDocument asset = new XmlDocument();

                try
                {
                    asset.Load(reader);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }

                return (true, namespaceAttribute);
            }
        }

        public IEnumerable<string> GetSchemaUrns()
        {
            return Schemas.Schemas()
                .Cast<XmlSchema>()
                .Select(x => x.TargetNamespace)
                .OrderBy(x => x)
                .ToArray();
        }
    }
}
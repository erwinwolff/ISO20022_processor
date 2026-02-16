using ISO20022.Definitions.Definitions;
using ISO20022.Definitions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ISO20022.Tests
{
    [TestClass]
    public class XmlFromXsdTest
    {
        [TestMethod]
        public void CreatePain00100112_Success()
        {
            Pain_001_001_12.Document document = new Pain_001_001_12.Document();

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                XmlSerializer serializerPain = new XmlSerializer(typeof(Pain_001_001_12.Document));
                serializerPain.Serialize(sw, document);
                stream.Position = 0;
                xmlFile = Encoding.UTF8.GetString(stream.ToArray());
            }

#if NET8_0_OR_GREATER
            Assert.AreEqual(@"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<Document xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.12"" />", xmlFile);
#endif
#if NET48_OR_GREATER
            Assert.AreEqual(@"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<Document xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.12"" />", xmlFile);
#endif

        }


        [TestMethod]
        public void CreatePain00100112_And_Traverse_Success()
        {
            Pain_001_001_12.Document document = new Pain_001_001_12.Document();

            TraverseAndInitAllProperties(document);

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializerPain = new XmlSerializer(typeof(Pain_001_001_12.Document));
                serializerPain.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.IsNotEmpty(xmlFile);
        }


        [TestMethod]
        public void CreateCamt05300113_And_Traverse_Success()
        {
            Camt_053_001_13.Document document = new Camt_053_001_13.Document();

            TraverseAndInitAllProperties(document);

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializerCamt = new XmlSerializer(typeof(Camt_053_001_13.Document));
                serializerCamt.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.IsNotEmpty(xmlFile);
        }

        [TestMethod]
        public void CreateCatm00300114_And_Traverse_Success()
        {
            Catm_003_001_14.Document document = new Catm_003_001_14.Document();

            TraverseAndInitAllProperties(document);

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializerCatm = new XmlSerializer(typeof(Catm_003_001_14.Document));
                serializerCatm.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.IsNotEmpty(xmlFile);
        }

        [TestMethod]
        public void CreateTsmt01400105_And_Traverse_Success()
        {
            Tsmt_014_001_05.Document document = new Tsmt_014_001_05.Document();

            TraverseAndInitAllProperties(document);

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializerTsmt = new XmlSerializer(typeof(Tsmt_014_001_05.Document));
                serializerTsmt.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.IsNotEmpty(xmlFile);
        }

        [TestMethod]
        public void GetAllXsdTypes()
        {
            ISchemaToObjectRegistry schemaToObjectRegistry = new SchemaToObjectRegistry();
            Assert.IsTrue(schemaToObjectRegistry.SchemaToObjectMap.Any());
        }

        [TestMethod]
        [Ignore("Test takes too long for normal usage")]
        public async Task GetAllXsdTypes_2()
        {
            ISchemaToObjectRegistry schemaToObjectRegistry = new SchemaToObjectRegistry();
            Assert.IsTrue(schemaToObjectRegistry.SchemaToObjectMap.Any());

            Parallel.ForEach(schemaToObjectRegistry.SchemaToObjectMap, 
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (tpe, tkn) => 
            {
                XmlSerializer serializer = new XmlSerializer(tpe.Value);
                var doc = Activator.CreateInstance(tpe.Value);

                TraverseAndInitAllProperties(doc);

                string xmlFile = "";

                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, doc);
                    stream.Position = 0;
                    xmlFile = Encoding.UTF8.GetString(stream.ToArray());
                }
            });
        }

        public bool IsPropertyACollection(PropertyInfo property)
        {
            return (!typeof(string).Equals(property.PropertyType) &&
                typeof(IEnumerable).IsAssignableFrom(property.PropertyType));
        }

        public bool IsPropertyACollection(Type property)
        {
            return typeof(IEnumerable).IsAssignableFrom(property);
        }

        public void TraverseAndInitAllProperties(object obj)
        {
           if (obj == null)
                return;

            var type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(obj, false);
                    continue;
                }

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, property.Name);
                    continue;
                }

                if (property.PropertyType == typeof(decimal))
                {
                    property.SetValue(obj, 99M);
                    continue;
                }

                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(obj, 1);
                    continue;
                }

                if (IsPropertyACollection(property))
                {
                    var listPropertyType = property.PropertyType.GetElementType();

                    if (listPropertyType == null) continue;
                    if (IsPropertyACollection(property.PropertyType)) continue;

                    var collectionInstance = Array.CreateInstance(listPropertyType, 0);
                    property.SetValue(obj, collectionInstance);
                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    var enumValues = Enum.GetValues(property.PropertyType);
                    if (enumValues.Length > 0)
                    {
                        property.SetValue(obj, enumValues.GetValue(0));
                    }
                    continue;
                }

                var constructor = property.PropertyType.GetConstructor(Type.EmptyTypes);
                if (constructor != null &&
                    constructor.GetParameters().Length == 0 &&
                    property.PropertyType != typeof(object)) // xml validator crashes if we try to create an instance of object type, so we skip it
                {
                    property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    var value = property.GetValue(obj);

                    Console.WriteLine($"{property.Name}: {value}");

                    TraverseAndInitAllProperties(value);
                }
            }
        }
    }
}
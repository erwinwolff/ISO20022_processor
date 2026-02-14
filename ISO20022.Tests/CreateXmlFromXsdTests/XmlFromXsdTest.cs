using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ISO20022.Tests
{
    [TestClass]
    public class XmlFromXsdTest
    {
        XmlSerializer serializerPain = new XmlSerializer(typeof(Pain_001_001_12.Document));
        XmlSerializer serializerCamt = new XmlSerializer(typeof(Camt_053_001_13.Document));
        XmlSerializer serializerTsmt = new XmlSerializer(typeof(Tsmt_014_001_05.Document));

        [TestMethod]
        public void CreatePain00100112_Success()
        {
            Pain_001_001_12.Document document = new Pain_001_001_12.Document();

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                serializerPain.Serialize(sw, document);
                stream.Position = 0;
                xmlFile = System.Text.Encoding.UTF8.GetString(stream.ToArray());
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
                serializerPain.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = System.Text.Encoding.UTF8.GetString(stream.ToArray());
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
                serializerCamt.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = System.Text.Encoding.UTF8.GetString(stream.ToArray());
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
                serializerTsmt.Serialize(stream, document);
                stream.Position = 0;
                xmlFile = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }

            Assert.IsNotEmpty(xmlFile);
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
using ISO20022.Definitions.Definitions;
using ISO20022.Definitions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
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

            document.InflateXmlPocoDefinition();

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

            document.InflateXmlPocoDefinition();

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

            document.InflateXmlPocoDefinition();

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

            document.InflateXmlPocoDefinition();

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
        public void CreateTsmt01900105_And_Traverse_Success()
        {
            Tsmt_019_001_05.Document document = new Tsmt_019_001_05.Document();

            document.InflateXmlPocoDefinition();

            string xmlFile = "";

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializerTsmt = new XmlSerializer(typeof(Tsmt_019_001_05.Document));
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

                doc.InflateXmlPocoDefinition();

                string xmlFile = "";

                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, doc);
                    stream.Position = 0;
                    xmlFile = Encoding.UTF8.GetString(stream.ToArray());
                    Assert.IsNotEmpty(xmlFile);
                }
            });
        }
    }
}
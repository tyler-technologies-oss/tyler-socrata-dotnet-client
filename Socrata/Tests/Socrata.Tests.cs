using NUnit.Framework;
using NUnit.Compatibility;
using System;
using Socrata.DSMAPI;
using Socrata.SODA;
using Socrata.SODA.Schema;
using System.Collections.Generic;

namespace Socrata
{
    [TestFixture]
    public class Tests : TestBase
    {
        // Files are in the TestFunctions folder
        [DeploymentItemAttribute("Incidents.csv")]
        // Files are in the TestFunctions folder
        [DeploymentItemAttribute("Incidents_One_Row.csv")]

        [Test]
        public void CreateClient()
        {
            new SocrataClient(new Uri("https://opendata.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Assert.Pass();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void MissingCredentials()
        {
            new SocrataClient(new Uri("https://opendata.socrata.com"), null, null);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void NoSSLError()
        {
            new SocrataClient(new Uri("http://opendata.socrata.com"), "test", "test");
        }

        [Test]
        public void ValidateClientError()
        {
            SocrataClient testClient = new SocrataClient(new Uri("https://opendata.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Boolean res = testClient.ValidateConnection();
            Assert.IsTrue(res);
        }

        [Test]
        public void CreateColumn()
        {
            Column test = new Column("Name", SocrataDataType.TEXT);
            test.description = "Set Description";
        }

        [Test]
        public void CreateSodaSchema()
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder();
            schemaBuilder
                .AddColumn(new Column("Name", SocrataDataType.TEXT))
                .AddColumn(new Column("OtherColumn", SocrataDataType.NUMBER))
                .RemoveColumnByName("Name");
            
            Assert.AreEqual(1, schemaBuilder.GetColumns().Count);
            SODASchema schema = schemaBuilder.Build();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ExpectBadResourceError()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResource("1234-NOTAREALID");
        }

        [Test]
        public void GetResourcesForDomain()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            List<DomainResource> resources = socrataClient.GetResources();
            Console.WriteLine(resources.Count);
            Assert.IsTrue(resources.Count > 101);
        }
    }
}

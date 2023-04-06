using NUnit.Framework;
using System;
using Socrata.ActivityLog;
using Socrata.SODA.Schema;
using System.Collections.Generic;

namespace Socrata
{
    [TestFixture]
    public class Tests
    {
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
        [ExpectedException(typeof(System.Net.Http.HttpRequestException))]
        public void ExpectNotToFindAlias()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("does_not_exist");
        }

        [Test]
        public void ExpectToFindAlias()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("test_fixture");
            long rows = resource.Rows().Count();
            Assert.AreEqual(rows, 0);
        }

        [Test]
        public void GetResourcesForDomain()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            List<DomainResource> resources = socrataClient.GetResources();
            Console.WriteLine(resources.Count);
            Assert.IsTrue(resources.Count > 101);
        }

        [Test]
        public void GetLatestDomainActivityLog()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            List<ActivityLogModel> activities = socrataClient.GetLatestActivityLog();
            Assert.IsTrue(activities.Count > 100);
            ActivityLogFetcher alf = new ActivityLogFetcher(socrataClient.httpClient);
            List<ActivityLogModel> none = alf.FetchByUserEmail("fake@fake.com");
            Assert.IsTrue(none.Count == 0);
        }
    }
}

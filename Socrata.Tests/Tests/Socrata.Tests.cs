using NUnit.Framework;
using System;
using Socrata.ActivityLog;
using Socrata.SODA.Schema;
using System.Collections.Generic;
using System.Net.Http;


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
        public void ExpectNotToFindAlias()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("does_not_exist");
            Assert.Null(resource);
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

        [Test]
        public void CollocateDatasets()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            SODASchema schema = new SchemaBuilder()
                .AddColumn(new Column("id", SocrataDataType.TEXT))
                .Build();
            Resource a = socrataClient.CreateSodaResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            Resource b = socrataClient.CreateSodaResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            Resource c = socrataClient.CreateSodaResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            List<Resource> resourcesToCollocate = new List<Resource>{
                b,
                c
            };
            CollocationJob collocate = a.CollocateToResources(resourcesToCollocate);
            collocate.Run(status => Console.WriteLine(status));
            // Clean up datasets
            a.Delete();
            b.Delete();
            c.Delete();
        }

        [Test]
        public void ListSchedulesTest()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            socrataClient.GetSchedules();
        }

        [Test]
        [ExpectedException(typeof(HttpRequestException))]
        public void noScheduleResultsInError()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("test_fixture");
            resource.GetSchedule();
        }

        [Test]
        public void getScheduleTest()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("vc4g-8aqx");
            resource.GetSchedule();
        }

        [Test]
        public void DeleteAndCreateScheduleTest()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("edem-28vu");
            Schedule oldSchedule = resource.GetSchedule();
            resource.DeleteSchedule();
            resource.CreateSchedule(oldSchedule.Resource);
        }

        [Test]
        public void updateScheduleTest()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            Resource resource = socrataClient.GetResourceByAlias("p6n3-fvan");
            Schedule schedule = resource.GetSchedule();
            schedule.Resource.Action.Parameters.Path = new List<string> { $"Test Path {System.DateTime.Now}" };
            schedule.Resource.Paused = true;
            resource.UpdateSchedule(schedule.Resource);
        }

        [Test]
        public void ListAgentsTest()
        {
            SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
            socrataClient.GetAgents();
        }
    }
}

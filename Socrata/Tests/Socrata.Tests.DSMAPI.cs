using NUnit.Framework;
using System;
using Socrata.DSMAPI;

namespace Socrata
{
    [TestFixture]
    public class DSMAPITests
    {
        /*******************/
        /* DSMAPI IT TESTS */
        /*******************/
        SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));

        [Test]
        public void TestDSMAPICreate()
        {
            DsmapiResourceBuilder builder = socrataClient.CreateDsmapiResourceBuilder("ToDelete");
            Revision initialRevision = builder
                .SetDescription($"{System.DateTime.Now} <b>TEST</b>")
                .Build();
            Source source = initialRevision.CreateUploadSource("test-csv.csv");
            string filepath = @"Incidents.csv";
            string csv = System.IO.File.ReadAllText(filepath);
            source.AddBytesToSource(csv);
            source.AwaitCompletion(status => Console.WriteLine(status));
            initialRevision.Apply();
            initialRevision.AwaitCompletion(status => Console.WriteLine(status));
            Resource newResource = new Resource(initialRevision.Metadata.FourFour, socrataClient.httpClient);
            newResource.Delete();
            Assert.IsTrue(true);
        }

        [Test, Order(2)]
        public void TestDSMAPIRenameView()
        {
            string newname = $"Incident_{System.DateTime.Now}"; 
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            revision.CreateViewSource();
            revision.RenameResource(newname);
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
            Assert.AreEqual(newname, revision.Metadata.Metadata.Name);
        }

        [Test, Order(3)]
        public void TestDSMAPISetDescription()
        {
            string newdesc = $"{System.DateTime.Now} <b>TEST</b>"; 
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            revision.CreateViewSource();
            revision.SetDescription(newdesc);
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
            Assert.AreEqual(newdesc, revision.Metadata.Metadata.Description);
        }

        [Test, Order(4)]
        public void TestDSMAPIWorkflowWithView()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            Source source = revision.CreateViewSource();
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
        }

        [Test, Order(5)]
        public void TestDSMAPIReplaceWorkflowWithCSV()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            Source source = revision.CreateUploadSource("test-csv.csv");
            string filepath = @"Incidents.csv";
            string csv = System.IO.File.ReadAllText(filepath);
            source.AddBytesToSource(csv);
            source.AwaitCompletion(status => Console.WriteLine(status));
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
        }

        [Test, Order(6)]
        public void TestDSMAPIStreamingWorkflowWithCSV()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            Source source = revision.CreateStreamingSource("test-csv.csv");
            ByteSink sink = source.StreamingSource(ContentType.CSV);
            //Open the stream and read the file.
            sink.FileSink(@"Incidents.csv");
            source.AwaitCompletion(status => Console.WriteLine(status));
            Assert.IsTrue(source.HasErrorRows());
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
        }
        
        [Test, Order(7)]
        public void TestDSMAPIErrorRowsWorkflowWithCSV()
        {
            Resource resource = this.socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            Source source = revision.CreateStreamingSource("test-csv.csv");
            ByteSink sink = source.StreamingSource(ContentType.CSV);
            //Open the stream and read the file.
            sink.FileSink(@"Incidents.csv");
            source.AwaitCompletion(status => Console.WriteLine(status));
            if(source.HasErrorRows())
            {
                string errorFile = "ErrorRows.csv";
                int numberOfErrors = source.NumberOfErrors();
                Console.WriteLine($"{numberOfErrors} error(s) found");
                source.ExportErrorRows(errorFile);
            }
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
        }

        [Test, Order(8)]
        public void TestDSMAPIUpdateWorkflowWithCSV()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.UPDATE);
            Source source = revision.CreateUploadSource("test-csv.csv");
            string filepath = @"Incidents_One_Row.csv";
            string csv = System.IO.File.ReadAllText(filepath);
            source.AddBytesToSource(csv);
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
        }
        
        [Test, Order(9)]
        public void TestDSMAPIChangeOutputSchema()
        {
            DsmapiResourceBuilder builder = socrataClient.CreateDsmapiResourceBuilder("ToDelete");
            Revision initialRevision = builder
                .SetDescription($"{System.DateTime.Now} <b>TEST</b>")
                .Build();
            Source source = initialRevision.CreateUploadSource("test-csv.csv");
            string filepath = @"Incidents.csv";
            string csv = System.IO.File.ReadAllText(filepath);
            source.AddBytesToSource(csv);
            source.AwaitCompletion(status => Console.WriteLine(status));
            OutputSchema os = source.GetLatestOutputSchema();
            os.ChangeColumnName("agency", "agency2")
                .ChangeColumnDescription("area_district", "TEST")
                .ChangeTransform("area_beat", Transforms.NUMBER("area_beat"))
                .ChangeTransform("area_station", Transforms.CUSTOM("upper('TEST')"))
                .ChangeTransform("area_quadrant", Transforms.CUSTOM("replace(" + Transforms.TEXT("incident_id").Value + ", '0', '1')"))
                .Submit();
            source.AwaitCompletion(status => Console.WriteLine(status));
            initialRevision.Apply();
            initialRevision.AwaitCompletion(status => Console.WriteLine(status));
            Resource newResource = new Resource(initialRevision.Metadata.FourFour, socrataClient.httpClient);
            newResource.Delete();
            Assert.IsTrue(true);
        }
    }
}

using NUnit.Framework;
using NUnit.Compatibility;
using System;
using Socrata.DSMAPI;
using Socrata.SODA;
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
        public void MoveDataAssets()
        {
            Assert.Pass();
        }

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
        [ExpectedException(typeof(Exception))]
        public void ExpectBadResourceError()
        {
            Resource resource = socrataClient.GetResource("1234-NOTAREALID");
        }

        [Test]
        public void CreateColumn()
        {
            Column test = new Column("Name", SocrataDataType.TEXT);
            test.description = "Set Description";
        }

        [Test]
        public void CreateSchema()
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder();
            schemaBuilder
                .AddColumn(new Column("Name", SocrataDataType.TEXT))
                .AddColumn(new Column("OtherColumn", SocrataDataType.NUMBER))
                .RemoveColumnByName("Name");
            
            Assert.AreEqual(1, schemaBuilder.GetColumns().Count);
            Schema schema = schemaBuilder.Build();
        }

        // DATA TESTS
        SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
        [Test]
        public void CreateDataset()
        {
            Schema schema = new SchemaBuilder()
                .AddColumn(new Column("Text", SocrataDataType.TEXT))
                .AddColumn(new Column("Number", SocrataDataType.NUMBER))
                .AddColumn(new Column("Point", SocrataDataType.POINT))
                .AddColumn(new Column("Date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("Bool", SocrataDataType.BOOLEAN))
                .Build();
            Resource newDataset = socrataClient.CreateResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            // Clean it up
            newDataset.Delete();
        }

        [Test]
        public void GetSchema()
        {
            Resource resource = socrataClient.GetResource("hpjc-drhf");
            SchemaBuilder schema = resource.GetSchema();
            List<Column> columns = schema.GetColumns();
            // Assert.AreEqual(columns.Count, 7);
            // Assert.AreEqual(columns[0].columnName, "ID");
        }

        [Test]
        public void GetMetadata()
        {
            Resource resource = socrataClient.GetResource("hpjc-drhf");
            Assert.AreEqual("Soda2 Testing", resource.metadata.Name);
        }

        [Test]
        public void SetMetadata()
        {
            Resource resource = socrataClient.GetResource("hpjc-drhf");
            resource.metadata.Name = "New Name";
            Assert.AreEqual("New Name", resource.metadata.Name);
        }

        Dictionary<string, object> testRecord = new Dictionary<string, object> {
            { "id", 2 },
            { "text", "test string"},
            { "number", 100 },
            { "date", "2021-10-10"},
            { "datetime", "2021-09-02T12:33:12"},
            { "point", "POINT (33 22)"}
        };

        Dictionary<string, object> badRecord = new Dictionary<string, object> {
            { 
                "id", 10
            },
            { 
                "number", "not a number"
            }
        };

        List<Dictionary<string, object>> testRecords = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"id", 1000000 },
                {"text", "test"}
            },
            new Dictionary<string, object>
            {
                {"id", 10000001 },
                {"text", "test"}
            }
        };

        [Test]
        public void InsertThenUpdateThenDeleteRecord()
        {
            Rows resource = socrataClient.GetResource("hpjc-drhf").Rows();
            // We first have to insert it, then update it, then delete it
            Result resInsert = resource.InsertRecord(testRecord);
            Result resUpdate = resource.UpdateRecord(testRecord);
            Result resDelete = resource.DeleteRecord(testRecord);
            Assert.AreEqual(1, resInsert.Created);
            Assert.AreEqual(1, resUpdate.Updated);
            Assert.AreEqual(1, resDelete.Deleted);
        }

        [Test]
        public void InsertThenUpdateThenDeleteRecords()
        {
            Rows resource = socrataClient.GetResource("hpjc-drhf").Rows();
            Result resInsert = resource.InsertRecords(testRecords);
            Result resUpdate = resource.UpdateRecords(testRecords);
            Result resDelete = resource.DeleteRecords(testRecords);
            Assert.AreEqual(2, resInsert.Created);
            Assert.AreEqual(2, resUpdate.Updated);
            Assert.AreEqual(2, resDelete.Deleted);
        }

        public void BadKeyInsertFailure()
        {
            Rows resource = socrataClient.GetResource("tzmz-8bnb").Rows();
            Dictionary<string, object> badKeyRecord = new Dictionary<string, object>(testRecord);
            badKeyRecord.Add("NotAKey", "NotAValue");
            Result resInsert = resource.InsertRecord(badKeyRecord);
            Console.WriteLine(resInsert.Message);
            Assert.IsTrue(resInsert.IsError);
        }

        public void BadTypeInsertFailure()
        {
            Rows resource = socrataClient.GetResource("tzmz-8bnb").Rows();
            Dictionary<string, object> badKeyRecord = new Dictionary<string, object>(testRecord);
            badKeyRecord.Add("call_id", "ShouldBeANumber");
            Result resInsert = resource.InsertRecord(badKeyRecord);
            Console.WriteLine(resInsert.Message);
            Assert.IsTrue(resInsert.IsError);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void DiffSchema()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            resource.AssertSchemaMatch(new TestSchema());
        }

        public void CreateWorkingCopy()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            Assert.AreEqual("644i-z9jj", workingCopy.Metadata.Id);
        }

        public void DiscardWorkingCopy()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            bool isDeleted = workingCopy.Discard();
            Assert.IsTrue(isDeleted);
        }
        
        public void AddColumnToSchemaBuilder()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();
            schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT));

            Column newCol = schemaBuilder.GetColumns().Find(c => c.columnName == "new_column");
            Assert.IsNotNull(newCol);
        }

        public void RemoveColumnFromSchemaBuilder()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();
            schemaBuilder.RemoveColumnByName("name");
            
            Column column = schemaBuilder.GetColumns().Find(c => c.columnName == "name");
            Assert.IsNull(column);
        }

        public void UpdateColumnFromSchemaBuilder()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            Column col = schemaBuilder
                .FindColumnByName("name")
                .UpdateName("newName");
            
            Column oldColumn = schemaBuilder.FindColumnByName("name");
            Column updatedColumn = schemaBuilder.FindColumnByName("newName");
            Assert.IsNull(oldColumn);
            Assert.IsNotNull(updatedColumn);
        }

        public void BuildSchema()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            Schema schema = schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT))
                .Build();
            
            Column newCol = schema.Columns.Find(c => c.columnName == "new_column");
            Assert.IsInstanceOf<Schema>(schema);
            Assert.IsNotNull(newCol);
        }

        public void SetWorkingCopySchema()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            Schema schema = schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT))
                .Build();
            
            WorkingCopy updatedWorkingCopy = workingCopy.SetSchema(schema);
            ColumnMetadata newCol = updatedWorkingCopy.Metadata.Columns.Find(c => c.name == "new_column");
            Assert.IsNotNull(newCol);
            updatedWorkingCopy.Discard();
        }

        public void PublishWorkingCopy()
        {
            Schema schema = new SchemaBuilder()
                .AddColumn(new Column("Text", SocrataDataType.TEXT))
                .AddColumn(new Column("Number", SocrataDataType.NUMBER))
                .AddColumn(new Column("Point", SocrataDataType.POINT))
                .AddColumn(new Column("Date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("Bool", SocrataDataType.BOOLEAN))
                .Build();
            Resource newDataset = socrataClient.CreateResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            

            WorkingCopy workingCopy = newDataset.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            Schema updatedSchema = schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT))
                .Build();
            
            WorkingCopy updatedWorkingCopy = workingCopy.SetSchema(schema);
            // Resource updatedDataset = updatedWorkingCopy.Publish();

            // Clean it up
            newDataset.Delete();
        }

        // SocrataClient newSocrataClient = new SocrataClient(new Uri("https://ori-scgc.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
        
        [Test]
        public void TestNewColumnsChanges()
        {
            Schema schema = new SchemaBuilder()
                .AddColumn(new Column("Text", SocrataDataType.TEXT))
                .AddColumn(new Column("Number", SocrataDataType.NUMBER))
                .AddColumn(new Column("Point", SocrataDataType.POINT))
                .AddColumn(new Column("Date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("Bool", SocrataDataType.BOOLEAN))
                .Build();

            Resource dataset = this.socrataClient.CreateResourceBuilder("ToDeleteII")
                .SetSchema(schema)
                .Build();
            
            WorkingCopy wc = dataset.CreateWorkingCopy();

            SchemaBuilder sbwc = wc.GetSchema();

             // let's modify a column first
            Column numCol = sbwc
                .FindColumnByName("Number")
                .UpdateName("new_number");

            // then let's add and remove existing columns
            Schema modifiedSchema = sbwc.AddColumn(new Column("NewText", SocrataDataType.TEXT))
                .RemoveColumnByName("Text")
                .Build();

            wc.SetSchema(modifiedSchema);

             // Check that we have our updated field
            Assert.IsNotNull(wc.Metadata.Columns.Find(c => c.name == "new_number"));

            // Check that we have our new "New Text" column
            Assert.IsNotNull(wc.Metadata.Columns.Find(c => c.name == "NewText"));

            // Check that we don't have our removed "Text" column
            Assert.IsNull(wc.Metadata.Columns.Find(c => c.name == "Text"));

            // Let's publish our working copy
            ResourceMetadata ur = wc.Publish();

            // Lets check our actual dataset now after we publish the working copy
            // Check that we have our updated field
            Assert.IsNotNull(ur.Columns.Find(c => c.name == "new_number"));

            // Check that we have our new "New Text" column
            Assert.IsNotNull(ur.Columns.Find(c => c.name == "NewText"));

            // Check that we don't have our removed "Text" column
            Assert.IsNull(ur.Columns.Find(c => c.name == "Text"));
            
            //Clean it up
            dataset.Delete();
        }

        [Test]
        public void DSMAPITests()
        {
            TestDSMAPIRenameView();
            TestDSMAPISetDescription();
            TestDSMAPIWorkflowWithView();
            TestDSMAPIReplaceWorkflowWithCSV();
            TestDSMAPIStreamingWorkflowWithCSV();
            TestDSMAPIErrorRowsWorkflowWithCSV();
        }

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

        public void TestDSMAPISetDescription()
        {
            string newdesc = $"{System.DateTime.Now} <b>yup</b>"; 
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            revision.CreateViewSource();
            revision.SetDescription(newdesc);
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
            Assert.AreEqual(newdesc, revision.Metadata.Metadata.Description);
        }

        public void TestDSMAPIWorkflowWithView()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            Source source = revision.CreateViewSource();
            revision.Apply();
            revision.AwaitCompletion(status => Console.WriteLine(status));
        }

        
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

        public void TestDSMAPIChangeOutputSchema()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Revision revision = resource.OpenRevision(RevisionType.REPLACE);
            Source source = revision.CreateStreamingSource("test-csv.csv");
            ByteSink sink = source.StreamingSource(ContentType.CSV);
            // Open the stream and read the file.
            sink.FileSink(@"Incidents.csv");
            source.AwaitCompletion(status => Console.WriteLine(status));

            // revision.Apply();
            // revision.AwaitCompletion(status => Console.WriteLine(status));
            // Assert.Fail();
        }
        
        [Test]
        public void TestFetchRows()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Rows rows = resource.Rows();
            List<TestJson> result = rows.Fetch<TestJson>(1000, 0);
            Assert.AreEqual(result.Count, 1000);
        }

        [Test]
        public void TestFetchAllRows()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Rows rows = resource.Rows();
            List<TestJson> result = rows.FetchAll<TestJson>();
            Assert.IsTrue(result.Count > 1000);
        }

        [Test]
        public void TestRowCount()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Rows rows = resource.Rows();
            long result = rows.Count();
            Assert.AreEqual(result, 3154);
        }

        [Test]
        public void TestPagination()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            Rows rows = resource.Rows();
            long total = rows.Count();
            long limit = 1000;
            long offset = 0;
            while(offset < total)
            {
                rows.Fetch<Dictionary<string, object>>(limit, offset);
                offset += limit;
            }
            Assert.AreEqual(offset, 4000);
        }
    }
}

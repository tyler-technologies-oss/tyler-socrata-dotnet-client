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
    public class SodaTests
    {
        /*****************/
        /* SODA IT TESTS */
        /*****************/
        SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
        [Test]
        public void CreateDataset()
        {
            SODASchema schema = new SchemaBuilder()
                .AddColumn(new Column("text", SocrataDataType.TEXT))
                .AddColumn(new Column("number", SocrataDataType.NUMBER))
                .AddColumn(new Column("point", SocrataDataType.POINT))
                .AddColumn(new Column("date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("bool", SocrataDataType.BOOLEAN))
                .Build();
            Resource newDataset = socrataClient.CreateSodaResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            // Clean it up
            newDataset.Delete();
        }

        [Test]
        public void CreateDatasetWithRowIdentifier()
        {
            Column id = new Column("id", SocrataDataType.TEXT);
            SODASchema schema = new SchemaBuilder()
                .AddColumn(id)
                .AddColumn(new Column("text", SocrataDataType.TEXT))
                .AddColumn(new Column("number", SocrataDataType.NUMBER))
                .AddColumn(new Column("point", SocrataDataType.POINT))
                .AddColumn(new Column("date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("bool", SocrataDataType.BOOLEAN))
                .Build();
            Resource newDataset = socrataClient.CreateSodaResourceBuilder("ToDelete")
                .SetSchema(schema)
                .SetRowIdentifier(id)
                .Build();
            // Clean it up
            newDataset.Delete();
        }

        [Test]
        public void CreateDatasetWithResourceAlias()
        {
            string alias = "to_delete";
            SODASchema schema = new SchemaBuilder()
                .AddColumn(new Column("text", SocrataDataType.TEXT))
                .AddColumn(new Column("number", SocrataDataType.NUMBER))
                .AddColumn(new Column("point", SocrataDataType.POINT))
                .AddColumn(new Column("date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("bool", SocrataDataType.BOOLEAN))
                .Build();
            Resource newDataset = socrataClient.CreateSodaResourceBuilder("To Delete")
                .SetSchema(schema)
                .Build();
            newDataset.SetResourceIdAlias(alias);
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

        [Test]
        public void BadKeyInsertFailure()
        {
            Rows resource = socrataClient.GetResource("hpjc-drhf").Rows();
            Dictionary<string, object> badKeyRecord = new Dictionary<string, object>(testRecord);
            badKeyRecord.Add("NotAKey", "NotAValue");
            Result resInsert = resource.InsertRecord(badKeyRecord);
            Assert.IsTrue(resInsert.IsError);
        }

        [Test]
        public void BadTypeInsertFailure()
        {
            Rows resource = socrataClient.GetResource("hpjc-drhf").Rows();
            Result resInsert = resource.InsertRecord(badRecord);
            Console.WriteLine(resInsert.Message);
            Assert.IsTrue(resInsert.IsError);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void DiffSchema()
        {
            Resource resource = socrataClient.GetResource("tzmz-8bnb");
            // TODO: Assert match
            TestSchema ts = new TestSchema();
            throw new Exception();
        }

        public void CreateWorkingCopy()
        {
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
        }

        public void DiscardWorkingCopy()
        {
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            bool isDeleted = workingCopy.Discard();
            Assert.IsTrue(isDeleted);
        }
        
        public void AddColumnToSchemaBuilder()
        {
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();
            schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT));

            Column newCol = schemaBuilder.GetColumns().Find(c => c.columnName == "new_column");
            Assert.IsNotNull(newCol);
        }

        public void RemoveColumnFromSchemaBuilder()
        {
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();
            schemaBuilder.RemoveColumnByName("name");
            
            Column column = schemaBuilder.GetColumns().Find(c => c.columnName == "name");
            Assert.IsNull(column);
        }

        public void UpdateColumnFromSchemaBuilder()
        {
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
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
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            SODASchema schema = schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT))
                .Build();
            
            Column newCol = schema.Columns.Find(c => c.columnName == "new_column");
            Assert.IsInstanceOf<SODASchema>(schema);
            Assert.IsNotNull(newCol);
        }

        public void SetWorkingCopySchema()
        {
            Resource resource = socrataClient.GetResource("qvvq-qwm2");
            WorkingCopy workingCopy = resource.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            SODASchema schema = schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT))
                .Build();
            
            WorkingCopy updatedWorkingCopy = workingCopy.SetSchema(schema);
            ColumnMetadata newCol = updatedWorkingCopy.Metadata.Columns.Find(c => c.name == "new_column");
            Assert.IsNotNull(newCol);
            updatedWorkingCopy.Discard();
        }

        public void PublishWorkingCopy()
        {
            SODASchema schema = new SchemaBuilder()
                .AddColumn(new Column("Text", SocrataDataType.TEXT))
                .AddColumn(new Column("Number", SocrataDataType.NUMBER))
                .AddColumn(new Column("Point", SocrataDataType.POINT))
                .AddColumn(new Column("Date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("Bool", SocrataDataType.BOOLEAN))
                .Build();
            Resource newDataset = socrataClient.CreateSodaResourceBuilder("ToDelete")
                .SetSchema(schema)
                .Build();
            

            WorkingCopy workingCopy = newDataset.CreateWorkingCopy();
            SchemaBuilder schemaBuilder = workingCopy.GetSchema();

            SODASchema updatedSchema = schemaBuilder
                .AddColumn(new Column("new_column", SocrataDataType.TEXT))
                .Build();
            
            WorkingCopy updatedWorkingCopy = workingCopy.SetSchema(schema);
            // Resource updatedDataset = updatedWorkingCopy.Publish();

            // Clean it up
            newDataset.Delete();
        }

        public void TestNewColumnsChanges()
        {
            SODASchema schema = new SchemaBuilder()
                .AddColumn(new Column("Text", SocrataDataType.TEXT))
                .AddColumn(new Column("Number", SocrataDataType.NUMBER))
                .AddColumn(new Column("Point", SocrataDataType.POINT))
                .AddColumn(new Column("Date", SocrataDataType.DATETIME, "Data Column Description"))
                .AddColumn(new Column("Bool", SocrataDataType.BOOLEAN))
                .Build();

            Resource dataset = this.socrataClient.CreateSodaResourceBuilder("ToDeleteII")
                .SetSchema(schema)
                .Build();
            
            WorkingCopy wc = dataset.CreateWorkingCopy();

            SchemaBuilder sbwc = wc.GetSchema();

             // let's modify a column first
            Column numCol = sbwc
                .FindColumnByName("Number")
                .UpdateName("new_number");

            // then let's add and remove existing columns
            SODASchema modifiedSchema = sbwc.AddColumn(new Column("NewText", SocrataDataType.TEXT))
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
    }
}

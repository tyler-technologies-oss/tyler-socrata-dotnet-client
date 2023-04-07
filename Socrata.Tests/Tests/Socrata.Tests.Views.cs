using NUnit.Framework;
using NUnit.Compatibility;
using System;
using Socrata.DSMAPI;
using Socrata.SODA;
using Socrata.SODA.Schema;


namespace Socrata
{
    [TestFixture]
    public class ViewTests
    {
        /*****************/
        /* SODA IT TESTS */
        /*****************/
        SocrataClient socrataClient = new SocrataClient(new Uri("https://tyler.partner.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
        [Test]
        public void CreateView()
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

            string soql = @"
                SELECT text
            ";
            string viewName = "test";
            View newView = newDataset.CreateViewFromSoQL(viewName, soql);

            // Clean it up
            newView.Delete();
            newDataset.Delete();
        }

        [Test]
        public void CreateInternalView()
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

            string soql = @"
                SELECT text
            ";
            string viewName = "test";
            View newView = newDataset.CreateViewFromSoQL(viewName, soql, AudienceLevel.INTERNAL);

            // Clean it up
            newView.Delete();
            newDataset.Delete();
        }
        
        [Test]
        public void CreatePublicView()
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

            string soql = @"
                SELECT text
            ";
            string viewName = "test";
            View newView = newDataset.CreateViewFromSoQL(viewName, soql, AudienceLevel.PUBLIC);

            // Clean it up
            newView.Delete();
            newDataset.Delete();
        }

        [Test]
        public void ReadViewRows()
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

            string soql = @"
                SELECT text
            ";
            string viewName = "test";
            View newView = newDataset.CreateViewFromSoQL(viewName, soql);

            Rows rows = newView.Rows();
            Assert.AreEqual(rows.Count(), 0);
            // Clean it up
            newView.Delete();
            newDataset.Delete();
        }

        [Test]
        public void ChangeViewPermissions()
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

            string soql = @"
                SELECT text
            ";
            string viewName = "test";
            View newView = newDataset.CreateViewFromSoQL(viewName, soql);
            newView.SetAudience(AudienceLevel.INTERNAL);
            // Clean it up
            // newView.Delete();
            // newDataset.Delete();
        }
    }
}

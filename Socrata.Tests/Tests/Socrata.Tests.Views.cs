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
        SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
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

            string soql = @"""
                SELECT text
            """;

            // View newView = newDataset.CreateViewFromSoQL(soql);

            // Clean it up
            // newView.Delete();
            newDataset.Delete();
        }
    }
}

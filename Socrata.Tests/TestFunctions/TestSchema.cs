using Socrata.SODA.Schema;

namespace Socrata
{
    public class TestSchema
    {
        public Column name = new Column("name", SocrataDataType.TEXT);
        public Column incident_id = new Column("incident_id", SocrataDataType.TEXT);
        public Column date = new Column("date", SocrataDataType.DATETIME);
    }
}

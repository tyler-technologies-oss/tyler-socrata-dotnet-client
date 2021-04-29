using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Socrata
{
    public class SchemaBuilder
    {
        public List<Column> Columns = new List<Column> ();
        public SchemaBuilder()
        {

        }
        public SchemaBuilder AddColumn(Column c) 
        {
            // TODO: Columns cannot have the same name
            // TODO: Ensure column names are valid (i.e. no spaces or illegal characters)
            this.Columns.Add(c);
            return this;
        }

        public Column FindColumnByName(string name)
        {
            return this.Columns.Find(c => c.columnName == name);
        }

        public Column FindColumnById(string id)
        {
            return this.Columns.Find(c => c.columnId == id);
        }
        public SchemaBuilder RemoveColumnByName(string name)
        {
            this.Columns.RemoveAll(c => c.columnName == name);
            return this;
        }

        public List<Column> GetColumns() => this.Columns;

        public Schema Build() => new Schema(Columns);

        public SchemaBuilder BuildFromResourceMetadata(List<ColumnMetadata> columns)
        {
            columns.ForEach((c) => {
                Column col = new Column(c);
                this.Columns.Add(col);
            });
            return this;
        }

        public List<Dictionary<string, object>> ConstructSchemaJson()
        {
            List<Dictionary<string, object>> d = this.Columns.Select(c => c.ToColumnDictionary()).ToList();
            var jsonString = JsonConvert.SerializeObject(
                d, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            return d;
        }

    }
}

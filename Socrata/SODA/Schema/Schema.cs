using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Socrata.SODA.Schema
{
    public class SODASchema
    {
        public List<Column> Columns {get; set;}
        public SODASchema(List<Column> columns)
        {
            this.Columns = columns;
        }

        public List<Dictionary<string, object>> ConstructSchemaJson()
        {
            List<Dictionary<string, object>> d = this.Columns.Select(c => c.ToColumnDictionary()).ToList();
            var jsonString = JsonConvert.SerializeObject(
                d, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            System.Diagnostics.Debug.WriteLine(jsonString);
            return d;
        }
    }
}

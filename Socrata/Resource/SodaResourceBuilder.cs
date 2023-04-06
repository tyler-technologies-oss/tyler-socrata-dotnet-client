using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Socrata.HTTP;
using Socrata.SODA.Schema;

namespace Socrata
{
    public class SodaResourceBuilder
    {
        string name { get; set; }
        string description;
        SODASchema Schema;
        string PrimaryKey = null;
        SocrataHttpClient httpClient;
        public SodaResourceBuilder(string name, SocrataHttpClient httpClient)
        {
            this.name = name;
            this.httpClient = httpClient;
        }

        public Resource Build()
        {
            string newId = Create();
            System.Diagnostics.Debug.WriteLine(newId);
            return new Resource(newId, httpClient);
        }

        public Dictionary<string, object> ResourceJson()
        {
            Dictionary<string, object> d = new Dictionary<string, object>
            {
                { "name", this.name },
                { "description", this.description },
                { "columns", this.Schema.ConstructSchemaJson() },
                // TODO: support adding categories and tags
                { "category", null },
                { "tags", null }
            };
            if(!string.IsNullOrEmpty(this.PrimaryKey))
            {
                // Add the Row Identifier if it is set
                d.Add("metadata", new Dictionary<string, string>{ {"rowIdentifier", this.PrimaryKey } });
            }
            var jsonString = JsonConvert.SerializeObject(
                d, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            
            System.Diagnostics.Debug.WriteLine(jsonString);
            return d;
        }

        public SodaResourceBuilder SetSchema(SODASchema schema)
        {
            this.Schema = schema;
            return this;
        }

        /// <summary>
        /// Set the Row Identifier (basically the Primary Key) of the dataset
        /// </summary>
        public SodaResourceBuilder SetRowIdentifier(Column column)
        {
            this.PrimaryKey = column.columnName;
            return this;
        }

        private string Create()
        {
            var t = this.ResourceJson();
            var newDataset = httpClient.PostJson<NewDataset>("/api/views.json", t);
            var publishedDataset = httpClient.PostJson<NewDataset>("/api/views/" + newDataset.Id + "/publication.json", new Dictionary<string, object>{});
            return newDataset.Id;
        }
        
    }
}

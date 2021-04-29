using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Socrata.HTTP;

namespace Socrata
{
    public class ResourceBuilder
    {
        string name;
        string description;
        Schema schema;
        SocrataHttpClient httpClient;
        public ResourceBuilder(string name, SocrataHttpClient httpClient)
        {
            this.name = name;
            this.httpClient = httpClient;
        }

        public ResourceBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public ResourceBuilder SetSchema(Schema schema)
        {
            // TODO: Validate the schema
            this.schema = schema;
            return this;
        }

        public ResourceBuilder SetDescription(string description)
        {
            this.description = description;
            return this;
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
                { "columns", this.schema.ConstructSchemaJson() },
                // TODO: support adding categories and tags
                { "category", null },
                { "tags", null }
            };
            var jsonString = JsonConvert.SerializeObject(
                d, Formatting.Indented,
                new JsonConverter[] {new StringEnumConverter()});
            
            System.Diagnostics.Debug.WriteLine(jsonString);
            return d;
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

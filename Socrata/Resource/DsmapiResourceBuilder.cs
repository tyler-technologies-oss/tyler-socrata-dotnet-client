using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Socrata.HTTP;
using Socrata.DSMAPI;

namespace Socrata
{
    public class DsmapiResourceBuilder
    {
        string name { get; set; }
        string description;
        SocrataHttpClient httpClient;
        public DsmapiResourceBuilder(string name, SocrataHttpClient httpClient)
        {
            this.name = name;
            this.httpClient = httpClient;
        }
        public Revision Build()
        {
            return CreateNewRevision();
        }

        public DsmapiResourceBuilder SetDescription(string description)
        {
            this.description = description;
            return this;
        }

        private Revision CreateNewRevision()
        {
            Dictionary<string, object> newAssetJson = new Dictionary<string, object>();

            Dictionary<string, string> revisionType = new Dictionary<string, string>();
            revisionType.Add("type", "update");

            
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("name", this.name);

            newAssetJson.Add("action", revisionType);
            newAssetJson.Add("metadata", metadata);

            RevisionResponse revisionResponse = 
                httpClient.PostJson<RevisionResponse>("/api/publishing/v1/revision", newAssetJson);
            return new Revision(httpClient, revisionResponse);;
        }
    }
}

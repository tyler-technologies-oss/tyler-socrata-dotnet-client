using System;
using System.Collections.Generic;
using Socrata.Abstractions;
using Socrata.HTTP;
using Socrata.DSMAPI;
using Socrata.SODA;
using Socrata.SODA.Schema;

namespace Socrata
{
    public class View : IView 
    {
        Resource parent;
        string name;
        string soql;
        AudienceLevel audience;
        string Id;
        bool Deleted = false;
        SocrataHttpClient httpClient;

        public ResourceMetadata metadata;
        public SODASchema schema;

        public View(Resource parent, SocrataHttpClient httpClient, string name, string soql, AudienceLevel audienceLevel)
        {
            if(this.Deleted) throw new Exception("Dataset has been deleted");
            this.parent = parent;
            this.httpClient = httpClient;
            this.name = name;
            this.soql = soql;
            this.audience = audienceLevel;
        }

        public View(Resource parent, SocrataHttpClient httpClient, string name, string soql)
        {
            new View(parent, httpClient, name, soql, AudienceLevel.PRIVATE);
        }

        /// <summary>
        /// Delete the resource. This will delete the asset on Socrata,
        /// so please be careful calling this.
        /// </summary>
        public Result Delete()
        {
            this.Deleted = true;
            return httpClient.Delete<Result>("/api/views/" + Id + ".json");
        }

        /// <summary>
        /// Create the View
        /// </summary>
        public View Create()
        {
            // Create the Revision
            string create_endpoint = "/api/publishing/v1/revision";
            Dictionary<string, object> body = new Dictionary<string, object> {
                {"action", new Dictionary<string, string>{ {"type", "replace"} }},
                {"creation_source", "dotnet-sdk"},
                {"metadata", new Dictionary<string, string>{
                    {"name", name},
                    {"originalViewId", parent.Id},
                    {"queryString", soql}
                }}
            };
            RevisionResponse revision = httpClient.PostJson<RevisionResponse>(create_endpoint, body);
            // Update with the permissions
            Dictionary<string, Dictionary<string, string>> permissions = new Dictionary<string, Dictionary<string, string>> {
                {"permissions", new Dictionary<string, string>{
                    {"scope", audience.Value}
                }}
            };
            string update_endpoint = revision.Links.Update;
            httpClient.PutJson<RevisionResponse>(update_endpoint, permissions);
            // Apply the Revision to create
            httpClient.PutEmpty<RevisionResponse>(revision.Links.Apply);
            RevisionResponse req = httpClient.GetJson<RevisionResponse>(revision.Links.Show);
            string status = req.Resource.TaskSets[0].Status;
            while (status != "successful" && status != "failure")
            {
                System.Threading.Thread.Sleep(1000);
                req = httpClient.GetJson<RevisionResponse>(revision.Links.Show);
                try
                {
                    status = req.Resource.TaskSets[0].Status;
                    System.Threading.Thread.Sleep(4000);
                }
                catch
                {
                    Console.WriteLine("WARN: Asset requires approval and cannot be published");
                    status = "failure";
                }
            }
            this.Id = revision.Resource.FourFour;
            this.metadata = httpClient.GetJson<ResourceMetadata>("/api/views/"+this.Id+".json");
            this.schema = new SchemaBuilder().BuildFromResourceMetadata(this.metadata.Columns).Build();
            return this;
        }
        
        public void SetAudience(AudienceLevel audienceLevel)
        {
            Dictionary<string, string> permissions = new Dictionary<string, string>{
                    {"scope", audienceLevel.Value}
            };
            httpClient.PutJson<Result>("/api/views/" + Id + "/permissions", permissions);
        }

        public Rows Rows()
        {
            return new Rows(httpClient, Id);
        }
    }
}

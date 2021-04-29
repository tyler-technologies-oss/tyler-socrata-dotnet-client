using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Socrata.Abstractions;
using System.Threading;
using System.Reflection;
using NUnit.Framework;
using Socrata.HTTP;
using Socrata.DSMAPI;
using Socrata.SODA;

namespace Socrata
{
    
    public class Resource : IResource 
    {
        string Id;
        bool Deleted = false;
        SocrataHttpClient httpClient;

        public ResourceMetadata metadata;
        public Schema schema;
        Regex idRegex = new Regex(@"^[a-z0-9]{4}-[a-z0-9]{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Resource(string id, SocrataHttpClient httpClient)
        {
            if(!ValidId(id))
                throw new Exception("Invalid id: " + id);
            if(this.Deleted) throw new Exception("Dataset has been deleted");

            this.Id = id;
            this.httpClient = httpClient;
            // TODO: Probably need to wrap this in a TRY CATCH
            this.metadata = httpClient.GetJson<ResourceMetadata>("/api/views/" + this.Id + ".json");
            this.schema = new SchemaBuilder().BuildFromResourceMetadata(this.metadata.Columns).Build();
        }

        /// <summary>
        /// Delete the resource. This will delete the asset on Socrata,
        /// so please be careful calling this.
        /// </summary>
        public Result Delete()
        {
            this.Deleted = true;
            return httpClient.Delete<Result>("/api/views/" + this.Id + ".json");
        }

        /// <summary>
        /// Test whether the given dataset ID is a valid id
        /// </summary>
        private Boolean ValidId(string id) => !String.IsNullOrEmpty(id) && idRegex.IsMatch(id);

        // <summary>
        // Opens a Revision for the dataset
        // </summary>
        public Revision OpenRevision(RevisionType type)
        {
            Dictionary<string, string> revisionType = new Dictionary<string, string>();
            revisionType.Add("type", type.Value);
            Dictionary<string, object> revisionAction = new Dictionary<string, object>();
            revisionAction.Add("action", revisionType);
            RevisionResponse revisionResponse = 
                httpClient.PostJson<RevisionResponse>("/api/publishing/v1/revision/" + this.Id, revisionAction);
            return new Revision(httpClient, revisionResponse);;
        }

        // This function is synchronous, so it won't return until the dataset has finished creating a working copy.
        // To Do: Create async version of this?
        public WorkingCopy CreateWorkingCopy()
        {
            ResourceMetadata workingCopyResponse = 
                httpClient.PostJson<ResourceMetadata>("/api/views/" + this.Id + "/publication.json?method=copy", new Dictionary<string, object>());
            // check the response back to see if it is done
            
            if (String.IsNullOrEmpty(workingCopyResponse.Status)) {
                return new WorkingCopy(httpClient, workingCopyResponse);
            } 
            System.Console.WriteLine(workingCopyResponse.Status);
            Thread.Sleep(3000);
            return CreateWorkingCopy();
        }

        public void AssertSchemaMatch<T>(T schema) where T : class
        {
            var newSchema = new SchemaBuilder();

            var t = schema.GetType().GetFields().ToList();
            foreach (FieldInfo p in t)
            {
                Column c = (Column)p.GetValue(schema);
                newSchema.AddColumn(c);
            }
            List<string> currSchemaDict = this.schema.ConstructSchemaJson().Select(c => (string)c.GetValueOrDefault("name")).ToList<string>();
            List<string> newSchemaDict = newSchema.ConstructSchemaJson().Select(c => (string)c.GetValueOrDefault("name")).ToList<string>();
            currSchemaDict.Sort();
            newSchemaDict.Sort();
            Assert.AreEqual(currSchemaDict, newSchemaDict);
        }

        public SchemaBuilder GetSchema()
        {
            return new SchemaBuilder();
        }

        public Rows Rows()
        {
            return new Rows(httpClient, Id);
        }
    }
}

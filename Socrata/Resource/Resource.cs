using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Socrata.Abstractions;
using System.Threading;
using Socrata.HTTP;
using Socrata.DSMAPI;
using Socrata.SODA;
using Socrata.SODA.Schema;

namespace Socrata
{
    
    public class Resource : IResource 
    {
        public string Id { get; internal set; }
        bool Deleted = false;
        SocrataHttpClient httpClient;

        public ResourceMetadata metadata;
        public SODASchema schema;
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
        /// Delete the resource. This will delete the asset on Socrata,
        /// so please be careful calling this.
        /// </summary>
        public Result SetResourceIdAlias(string alias)
        {
            return httpClient.PutJson<Result>("/api/views/" + this.Id + ".json", new Dictionary<string, string>{{"resourceName", alias}});
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

        // <summary>
        // Opens a Revision for the dataset using the config
        // </summary>
        public Revision OpenRevisionUsingConfig(string config)
        {
            Dictionary<string, string> revisionType = new Dictionary<string, string>();
            revisionType.Add("config", config);
            RevisionResponse revisionResponse = 
                httpClient.PostJson<RevisionResponse>("/api/publishing/v1/revision/" + this.Id, revisionType);
            return new Revision(httpClient, revisionResponse);
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

        public SchemaBuilder GetSchema()
        {
            return new SchemaBuilder();
        }

        public Rows Rows()
        {
            return new Rows(httpClient, Id);
        }

        /// <summary>
        /// Create a Collocation Job to run.
        /// Collocations can take a minute, so the run task must be awaited
        /// </summary>
        public CollocationJob CollocateToResources(List<Resource> resources)
        { 
            return new CollocationJob(this, resources, httpClient);
        }

        /// <summary>
        /// Given a SoQL query, create a view.
        /// NOTE: `SELECT *` is not permitted in this interface
        /// </summary>
        public View CreateViewFromSoQL(string name, string soql, AudienceLevel audienceLevel)
        { 
            View view = new View(this, httpClient, name, soql, audienceLevel);
            return view.Create();
        }
        public View CreateViewFromSoQL(string name, string soql)
        {
            return CreateViewFromSoQL(name, soql, AudienceLevel.PRIVATE);
        }

        /// <summary>
        /// Set the resource audience, one of AudienceLevel:
        /// PRIVATE, INTERNAL, PUBLIC
        /// </summary>
        public void SetAudience(AudienceLevel audienceLevel)
        {
            Dictionary<string, string> permissions = new Dictionary<string, string>{
                    {"scope", audienceLevel.Value}
            };
            httpClient.PutJson<Result>("/api/views/" + Id + "/permissions", permissions);
        }

        /// <summary>
        /// Get the schedule attached to the resource
        /// or an HttpError if no schedule is present.
        /// </summary>
        public Schedule GetSchedule() 
        {
            Schedule datasetSchedule = httpClient.GetJson<Schedule>("/api/publishing/v1/schedule/" + Id);
            return datasetSchedule;
        }

        public Schedule CreateSchedule(ScheduleResource newSchedule)
        {
            Dictionary<string, object> newScheduleJson = new Dictionary<string, object> {
                {"paused", newSchedule.Paused},
                {"cadence", newSchedule.Cadence},
                {"action", newSchedule.Action}
            };
            Schedule newDatasetSchedule = httpClient.PostJson<Schedule>(
                "/api/publishing/v1/schedule/" + Id,
                newScheduleJson
            );
            return newDatasetSchedule;
        }

        public Schedule UpdateSchedule(ScheduleResource newSchedule)
        {
            Dictionary<string, object> newScheduleJson = new Dictionary<string, object> {
                {"paused", newSchedule.Paused},
                {"cadence", newSchedule.Cadence},
                {"action", newSchedule.Action}
            };
            Schedule newDatasetSchedule = httpClient.PutJson<Schedule>(
                "/api/publishing/v1/schedule/" + Id,
                newScheduleJson
            );
            return newDatasetSchedule;
        }

        public void DeleteSchedule()
        {
            httpClient.Delete<Schedule>("/api/publishing/v1/schedule/" + Id);
        }
    }
}

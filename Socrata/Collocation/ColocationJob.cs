using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Socrata.Abstractions;
using System.Threading;
using Socrata.HTTP;
using Socrata.Collocation;

namespace Socrata
{
    public class CollocationJob
    {
        SocrataHttpClient socrataClient;
        Resource primary;

        List<Resource> secondaries;
        public CollocationJob(Resource primary, List<Resource> secondaries, SocrataHttpClient socrataClient)
        {
            this.socrataClient = socrataClient;
            this.primary = primary;
            this.secondaries = secondaries;
        }

        private JobStatus CreateColocation()
        {
            string create_endpoint = "/api/collocate";
            List<List<string>> datasetMappings = new List<List<string>>();
            this.secondaries.ForEach(resource => {
                List<string> ret = new List<string>();
                ret.Add(this.primary.Id);
                ret.Add(resource.Id);
                datasetMappings.Add(ret);
            });
            Dictionary<string, List<List<string>>> body = new Dictionary<string, List<List<string>>> {
                {"collocations", datasetMappings}
            };
            JobStatus create = socrataClient.PostJson<JobStatus>(create_endpoint, body);
            return create;
        }

        private JobStatus GetJobStatus(string jobId)
        {
            string job_endpoint = "/api/collocate/" + jobId;
            JobStatus status = socrataClient.GetJson<JobStatus>(job_endpoint);
            return status;
        }

        public void Run(Action<string> lambda)
        {
            JobStatus created = CreateColocation();
            string status = created.Status;
            while(status != "completed" && status != "failed")
            {
                status = GetJobStatus(created.JobId).Status;
                lambda(status);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}

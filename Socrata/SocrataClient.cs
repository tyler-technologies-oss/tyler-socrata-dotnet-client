﻿using System;
using Socrata.Abstractions;
using System.Collections.Generic;

namespace Socrata
{
    using Socrata.HTTP;
    using Socrata.ActivityLog;
    using System.Net.Http;

    public class SocrataClient : ISocrataClient
    {
        public SocrataHttpClient httpClient { get; internal set; }

        /// <summary>
        /// The Base Socrata Client.
        /// </summary>
        public SocrataClient(Uri host, string apikey, string apitoken)
        {
            if (host.Scheme != "https")
                throw new Exception("Https protocol required");

            if (String.IsNullOrEmpty(apikey) || String.IsNullOrEmpty(apitoken))
                throw new Exception("API key and secret required");

            httpClient = new SocrataHttpClient(host, apikey, apitoken);
        }

        /// <summary>
        /// Get Resource by Dataset ID.
        /// </summary>
        public Resource GetResource(string id) => new Resource(id, httpClient);

        /// <summary>
        /// Get a Resource by its alias, returns `null` if resource does not exist
        /// </summary>
        public Resource GetResourceByAlias(string alias)
        {
            try 
            {
                DomainResource result = httpClient.GetJson<DomainResource>("/api/views/" + alias);
                return new Resource(result.Id, httpClient);
            } 
            catch (HttpRequestException)
            {
                Console.WriteLine("Resource " + alias + " was not found on " + httpClient.host.ToString());
                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            } 

        }

        /// <summary>
        /// Test to make sure the connection is valid.
        /// </summary>
        public bool ValidateConnection() => httpClient.Get("/api/users/current.json").IsSuccessStatusCode;

        /// <summary>
        /// Create a new resource on the domain via SODA.
        /// </summary>
        public SodaResourceBuilder CreateSodaResourceBuilder(string name) => new SodaResourceBuilder(name, httpClient);

        /// <summary>
        /// Create a new resource on the domain via DSMAPI.
        /// </summary>
        public DsmapiResourceBuilder CreateDsmapiResourceBuilder(string name) => new DsmapiResourceBuilder(name, httpClient);


        /// <summary>
        /// List all resources on the domain.
        /// </summary>
        public List<DomainResource> GetResources()
        {
            DomainResources dr = new DomainResources(httpClient);
            List<DomainResult> res = dr.Fetch();
            List<DomainResource> resources = new List<DomainResource>();
            res.ForEach((resource) => resources.Add(resource.Resource));
            return resources;
        }

        public List<ActivityLogModel> GetLatestActivityLog(long offset = 0, long limit = 1000)
        {
            ActivityLogFetcher al = new ActivityLogFetcher(this.httpClient);
            return al.FetchLatest(offset, limit);
        }

        public List<Schedule> GetSchedules()
        {
            return httpClient.GetJson<List<Schedule>>("/api/publishing/v1/schedule");
        }

        public List<Agent> GetAgents()
        {
            return httpClient.GetJson<List<Agent>>("/api/publishing/v1/connection_agent");
        }

    }
}

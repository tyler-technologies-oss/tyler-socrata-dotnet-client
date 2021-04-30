using System;
using Socrata.Abstractions;
using System.Collections.Generic;

namespace Socrata
{
    using Socrata.HTTP;
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
    }
}

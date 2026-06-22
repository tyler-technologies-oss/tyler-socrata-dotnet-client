using Socrata.HTTP;
using System.Collections.Generic;
using System;

namespace Socrata
{
    public class DomainResources
    {
        SocrataHttpClient httpClient;
        string DefaultDiscoveryAPI = "http://api.us.socrata.com/api/catalog/v1";
        public DomainResources(SocrataHttpClient httpClient) 
        {
            this.httpClient = httpClient;
        }

        public List<DomainResult> Fetch(String discoveryAPIOverride)
        {
            Uri uri = new Uri(discoveryAPIOverride + "?domains=" + this.httpClient.host.Host + "&limit=10000");
            DomainResults results = httpClient.GetJson<DomainResults>(uri);
            return results.Results;
        }
        
        public List<DomainResult> Fetch()
        {
            Uri uri = new Uri(DefaultDiscoveryAPI + "?domains=" + this.httpClient.host.Host + "&limit=10000");
            DomainResults results = httpClient.GetJson<DomainResults>(uri);
            return results.Results;
        }
    }
}

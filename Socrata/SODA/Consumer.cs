using System.Collections.Generic;
using System.Linq;

namespace Socrata.SODA
{
    using Socrata.HTTP;
    public class Consumer
    {
        SocrataHttpClient httpClient;
        string Id;
        long MAX = 1000000000;
        public Consumer(SocrataHttpClient httpClient, string Id)
        {
            this.httpClient = httpClient;
            this.Id = Id;
        }
        public List<T> Fetch<T>(long limit, long offset)
        {
            return httpClient.GetJson<List<T>>("/api/v3/views/" + this.Id + "/query.json?query=SELECT * LIMIT " + limit.ToString() + " OFFSET " + offset.ToString());
        }

        public List<T> FetchAll<T>()
        {
            return Fetch<T>(MAX, 0);
        }

        public List<T> FetchQuery<T>(string queryString)
        {
            return httpClient.GetJson<List<T>>("/api/v3/views/" + this.Id + "/query.json?query=" + queryString);
        }

        public long Count()
        {
            List<RowCount> Count = FetchQuery<RowCount>("select count(*) as Count");
            return Count.First().Count;
        }
    }
}

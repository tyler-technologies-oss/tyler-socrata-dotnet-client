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
            return httpClient.GetJson<List<T>>("/resource/" + this.Id + ".json?$limit=" + limit.ToString() + "&$offset=" + offset.ToString());
        }

        public List<T> FetchAll<T>()
        {
            return Fetch<T>(MAX, 0);
        }

        public List<T> FetchQuery<T>(string queryString)
        {
            return httpClient.GetJson<List<T>>("/resource/" + this.Id + ".json?$query=" + queryString);
        }

        public long Count()
        {
            List<RowCount> Count = FetchQuery<RowCount>("select count(*) as Count");
            return Count.First().Count;
        }
    }
}

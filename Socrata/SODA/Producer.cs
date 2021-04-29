using System.Collections.Generic;
using System.Linq;

namespace Socrata.SODA
{
    using HTTP;
    public class Producer
    {
        SocrataHttpClient httpClient;
        string Id;
        public Producer(SocrataHttpClient httpClient, string Id)
        {
            this.httpClient = httpClient;
            this.Id = Id;
        }
        
        private List<IDictionary<string, object>> SingletonList(IDictionary<string, object> item)
        {
            return new List<IDictionary<string, object>>
            {
                item
            };
        }

        public Result UpdateRecord(IDictionary<string, object> record) => httpClient.PostJson<Result>("/resource/" + this.Id + ".json", SingletonList(record));

        public Result InsertRecord(IDictionary<string, object> record) => UpdateRecord(record);

        public Result DeleteRecord(IDictionary<string, object> record)
        {
            record.Add(":deleted", true);
            return httpClient.PostJson<Result>("/resource/" + this.Id + ".json", SingletonList(record));
        }

        public Result UpdateRecords(IEnumerable<IDictionary<string, object>> records) => httpClient.PostJson<Result>("/resource/" + this.Id + ".json", records);
        

        public Result InsertRecords(IEnumerable<IDictionary<string, object>> records) => httpClient.PostJson<Result>("/resource/" + this.Id + ".json", records);
        
        // <summary>
        // Delete a set of records from the dataset
        // </summary>
        public Result DeleteRecords(IEnumerable<IDictionary<string, object>> records)
        {
            records.ToList().ForEach((record) => record.Add(":deleted", true));
            return httpClient.PostJson<Result>("/resource/" + this.Id + ".json", records);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Socrata.SODA
{
    using Socrata.HTTP;
    public class Rows
    {
        Consumer consumer;
        Producer producer;
        public Rows(SocrataHttpClient httpClient, string Id)
        {
            this.consumer = new Consumer(httpClient, Id);
            this.producer = new Producer(httpClient, Id);
        }
        private List<IDictionary<string, object>> SingletonList(IDictionary<string, object> item)
        {
            return new List<IDictionary<string, object>>
            {
                item
            };
        }

        public Result UpdateRecord(IDictionary<string, object> record) => this.producer.UpdateRecord(record);

        public Result InsertRecord(IDictionary<string, object> record) => UpdateRecord(record);

        public Result DeleteRecord(IDictionary<string, object> record) => this.producer.DeleteRecord(record);

        public Result UpdateRecords(IEnumerable<IDictionary<string, object>> records) => this.producer.UpdateRecords(records);
        

        public Result InsertRecords(IEnumerable<IDictionary<string, object>> records) => this.producer.UpdateRecords(records);
        
        // <summary>
        // Delete a set of records from the dataset
        // </summary>
        public Result DeleteRecords(IEnumerable<IDictionary<string, object>> records) => this.producer.DeleteRecords(records);

        public List<T> Fetch<T>(long limit, long offset) => this.consumer.Fetch<T>(limit, offset);
        public List<T> FetchAll<T>() => this.consumer.FetchAll<T>();
        public long Count() => this.consumer.Count();

    }
}

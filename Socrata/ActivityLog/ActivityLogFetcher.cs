using Socrata.HTTP;
using System.Collections.Generic;

namespace Socrata.ActivityLog
{
    public class ActivityLogFetcher
    {
        SocrataHttpClient httpClient;

        public ActivityLogFetcher(SocrataHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public List<ActivityLogModel> FetchLatest(long offset = 0, long limit = 500)
        {
            List<ActivityLogModel> latest = this.httpClient.GetJson<List<ActivityLogModel>>($"/api/activity_log.json?$order=created_at desc&$limit={limit}&$offset={offset}");
            return latest;
        }

        public List<ActivityLogModel> FetchByUserEmail(string useremail, long offset = 0, long limit = 0)
        {
            string query = $"/api/activity_log.json?$order=created_at desc&$limit={limit}&$offset={offset}&$where=acting_user_name = '{useremail}'";
            List<ActivityLogModel> latest = this.httpClient.GetJson<List<ActivityLogModel>>(query);
            return latest;
        }
    }
}

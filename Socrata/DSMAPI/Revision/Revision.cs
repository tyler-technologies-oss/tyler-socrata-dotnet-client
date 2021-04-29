using System.Collections.Generic;
using System;
using Socrata.HTTP;

namespace Socrata.DSMAPI
{
    // <summary>
    // A Revision class from which sources can be added
    // </summary>
    public class Revision
    {
        public SocrataHttpClient HttpClient;
        public RevisionResource Metadata;
        public RevisionLinks Links;
        public Source Source;
        public Revision(SocrataHttpClient httpClient, RevisionResponse response)
        {
            this.HttpClient = httpClient;
            this.Metadata = response.Resource;
            this.Links = response.Links;
        }
        private SourceResponse CreateSource(Dictionary<string, string> source)
        {
            System.Console.WriteLine(string.Join(Environment.NewLine, source));
            Dictionary<string, object> sourceType = new Dictionary<string, object>();
            sourceType.Add("source_type", source);
            System.Console.WriteLine(string.Join(Environment.NewLine, sourceType));
            SourceResponse result = HttpClient.PostJson<SourceResponse>(Links.CreateSource, sourceType);
            return result;
        }

        public Source CreateViewSource()
        {   
            Dictionary<string, string> viewSource = new Dictionary<string, string>();
            viewSource.Add("type", SourceType.VIEW.Value);
            viewSource.Add("fourfour", "none");
            SourceResponse result = CreateSource(viewSource);
            this.Source = new Source(this.HttpClient, result.Resource, result.Links);
            return this.Source;
        }

        public Source CreateUploadSource(string filename)
        {   
            Dictionary<string, string> uploadSource = new Dictionary<string, string>();
            uploadSource.Add("type", SourceType.UPLOAD.Value);
            uploadSource.Add("filename", filename);
            SourceResponse result = CreateSource(uploadSource);
            this.Source = new Source(this.HttpClient, result.Resource, result.Links);
            return this.Source;
        }

        public Source CreateStreamingSource(string sourcename)
        {   
            Dictionary<string, string> uploadSource = new Dictionary<string, string>();
            uploadSource.Add("type", SourceType.UPLOAD.Value);
            uploadSource.Add("filename", sourcename);
            SourceResponse result = CreateSource(uploadSource);
            this.Source = new Source(this.HttpClient, result.Resource, result.Links);
            return this.Source;
        }

        public Source CreateURLSource(string filename, string url)
        {   
            Dictionary<string, string> viewSource = new Dictionary<string, string>();
            viewSource.Add("type", SourceType.URL.Value);
            viewSource.Add("filename", filename);
            viewSource.Add("url", url);
            SourceResponse result = CreateSource(viewSource);
            this.Source = new Source(this.HttpClient, result.Resource, result.Links);
            return this.Source;
        }

        public RevisionResponse Update(Dictionary<string, object> obj)
        {
            RevisionResponse result = HttpClient.PatchJson<RevisionResponse>(Links.Update, obj);
            return result;
        }

        public void RenameResource(string newName)
        {
            Dictionary<string, string> newNameJson = new Dictionary<string, string>();
            newNameJson.Add("name", newName);
            Dictionary<string, object> newMetadata = new Dictionary<string, object>();
            newMetadata.Add("metadata", newNameJson);
            RevisionResponse resp = HttpClient.PutJson<RevisionResponse>(this.Links.Update, newMetadata);
            this.Metadata = resp.Resource;
            return;
        }

        public void SetDescription(string newDescription)
        {
            Dictionary<string, string> newNameJson = new Dictionary<string, string>();
            newNameJson.Add("description", newDescription);
            Dictionary<string, object> newMetadata = new Dictionary<string, object>();
            newMetadata.Add("metadata", newNameJson);
            RevisionResponse resp = HttpClient.PutJson<RevisionResponse>(this.Links.Update, newMetadata);
            this.Metadata = resp.Resource;
            return;
        }

        public Revision Apply()
        {
            RevisionResponse result = HttpClient.PutEmpty<RevisionResponse>(Links.Apply);
            return this;
        }

        /// <summary>
        /// Await the completion of the update, optionally output the status.
        /// </summary>
        /// <param name="lambda">A lambda function for outputting status if desired.</param>
        public void AwaitCompletion(Action<string> lambda)
        {
            string status = "";
            while(status != "successful" && status != "failure")
            {
                RevisionResponse req = HttpClient.GetJson<RevisionResponse>(Links.Show);
                status = req.Resource.TaskSets[0].Status;
                lambda(status);
                System.Threading.Thread.Sleep(1000);
            }
        }

        public RevisionResponse Discard()
        {
            RevisionResponse result = HttpClient.Delete<RevisionResponse>(Links.Discard);
            return result;
        }
    }
}

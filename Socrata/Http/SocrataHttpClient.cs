using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;


namespace Socrata.HTTP
{
    public class SocrataHttpClient
    {
        HttpClient httpClient;
        HttpRequestMessage httpRequestMessage;
        public Uri host { get; private set; }

        public SocrataHttpClient(Uri host, string apikey, string apitoken)
        {
            this.host = host;
            httpClient = new HttpClient();            
            // Authentication
            string authKVP = String.Format("{0}:{1}", apikey, apitoken);
            byte[] authBytes = Encoding.UTF8.GetBytes(authKVP);
            httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));
        }
            
        private HttpResponseMessage _execute(HttpRequestMessage httpRequestMessage, string ContentType = "application/json", bool dangerously = false)
        {
            httpClient.DefaultRequestHeaders.Remove("Accept");
            httpClient.DefaultRequestHeaders.Add("Accept", ContentType);
            var resp = httpClient.SendAsync(httpRequestMessage).Result;
            // Console.WriteLine(resp.Content.ReadAsStringAsync().Result);
            if(!dangerously) {
                if(!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine(resp.Content.ReadAsStringAsync().Result);
                    resp.EnsureSuccessStatusCode();
                }
            }
            return resp;
        }

        public HttpResponseMessage Get(string endpoint, string ContentType = "application/json")
        {
            var url = new Uri(host, endpoint);
            Console.WriteLine("Get: " + url);
            httpRequestMessage = new HttpRequestMessage { RequestUri = url, Method = new HttpMethod("GET") };
            return this._execute(httpRequestMessage, ContentType);
        }

        public HttpResponseMessage Get(Uri uri, string ContentType = "application/json")
        {
            Console.WriteLine("Get: " + uri);
            httpRequestMessage = new HttpRequestMessage { RequestUri = uri, Method = new HttpMethod("GET") };
            return this._execute(httpRequestMessage, ContentType);
        }

        public HttpResponseMessage Put(string endpoint, HttpContent content)
        {
            var url = new Uri(host, endpoint);
            Console.WriteLine("Put: " + url);
            httpRequestMessage = new HttpRequestMessage { RequestUri = url, Method = new HttpMethod("PUT") };
            httpRequestMessage.Content = content;
            return this._execute(httpRequestMessage);           
        }

        public HttpResponseMessage Post(string endpoint, HttpContent content, bool dangerously = false)
        {
            Uri url = new Uri(host, endpoint);
            Console.WriteLine("Post: " + url);
            httpRequestMessage = new HttpRequestMessage { RequestUri = url, Method = new HttpMethod("POST") };
            httpRequestMessage.Content = content;
            return this._execute(httpRequestMessage, dangerously: dangerously);
        }

        public HttpResponseMessage Patch(string endpoint, HttpContent content)
        {
            Uri url = new Uri(host, endpoint);
            Console.WriteLine("Patch: " + url);
            httpRequestMessage = new HttpRequestMessage { RequestUri = url, Method = new HttpMethod("PATCH") };
            httpRequestMessage.Content = content;
            return this._execute(httpRequestMessage);
        }

        public T Delete<T>(string endpoint) where T : class
        {
            var url = new Uri(host, endpoint);
            Console.WriteLine("Delete: ", url);
            httpRequestMessage = new HttpRequestMessage { RequestUri = url, Method = new HttpMethod("DELETE") };
            var resp = this._execute(httpRequestMessage);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }

        /// <summary>
        /// Execute GET request and parse response as T.
        /// </summary>
        public T GetJson<T>(string endpoint) where T : class
        {
            var resp = Get(endpoint);
            var s = resp.Content.ReadAsStringAsync().Result;
            try {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                return result;
            } catch (Exception e) {
                Console.WriteLine(s);
                return null;
            }

        }

        public T GetJson<T>(Uri uri) where T : class
        {
            var resp = Get(uri);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }

        /// <summary>
        /// Execute PUT request with an object and parse response as T.
        /// </summary>
        public T PutJson<T>(string endpoint, object obj) where T : class
        {
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                "application/json"
            );
            var resp = this.Put(endpoint, content);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }
        /// <summary>
        /// Execute an empty PUT request and parse response as T.
        /// </summary>
        public T PutEmpty<T>(string endpoint) where T : class
        {
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>()),
                Encoding.UTF8,
                "application/json"
            );
            var resp = this.Put(endpoint, content);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }


        public T PostJson<T>(string endpoint, object obj, bool dangerously = false) where T : class
        {
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(obj), 
                Encoding.UTF8, 
                "application/json"
            );
            var resp = this.Post(endpoint, content, dangerously);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }

        public T PostBytes<T>(string endpoint, byte[] bytes) where T : class
        {
            var content = new ByteArrayContent(bytes);
            var resp = this.Post(endpoint, content);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }

        public T PostEmpty<T>(string endpoint) where T : class
        {
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>()), 
                Encoding.UTF8, 
                "application/json"
            );
            var resp = this.Post(endpoint, content);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }

        public T PatchJson<T>(string endpoint, object obj) where T : class
        {
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(obj), 
                Encoding.UTF8, 
                "application/json"
            );
            var resp = this.Patch(endpoint, content);
            T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp.Content.ReadAsStringAsync().Result);
            return result;
        }
    }
}

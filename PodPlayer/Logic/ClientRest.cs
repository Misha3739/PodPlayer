using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoList.HtppClient;

namespace PodPlayer.Logic
{
    internal partial class RestClient
    {
        private HttpClient client;

        public int Timeout { get; set; }

        public HttpClientHandler GetHttpClientHandler()
        {
            return new NativeMessageHandler(true);

        }

        public Response<T> Execute<T>(string url, ISerializer _serializer)
        {
            HttpClientHandler handler;
            HttpResponseMessage response;
            HttpRequestMessage request;
            Response<T> result;

            try
            {
                handler = GetHttpClientHandler();

                client = new HttpClient(handler);
                result = new Response<T>();

                request = new HttpRequestMessage(new HttpMethod("GET"), new Uri(url));

                response = client.SendAsync(request).Result;
                result.StatusCode = response.StatusCode;
                result.StatusDescription = response.ReasonPhrase;

                try
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    result.Data = _serializer.Deserialize<T>(content);
                }
                catch (FormatException)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                result = new Response<T>();
                result.ErrorMessage = GetExceptionMessage(ex);
                result.StatusCode = (HttpStatusCode)(-1);
                result.StatusDescription = result.ErrorMessage;
            }
            return result;
        }


        private string GetExceptionMessage(Exception ex)
        {
            StringBuilder sb;

            sb = new StringBuilder();
            if (ex is AggregateException)
            {
                AggregateException aex;

                aex = ex as AggregateException;
                foreach (Exception iex in aex.InnerExceptions)
                {

                    if (iex.InnerException != null)
                    {
                        sb.AppendLine(GetExceptionMessage(iex.InnerException));
                    }
                    else
                    {
                        sb.AppendLine(iex.Message);
                    }
                }
            }
            else if (ex is Exception)
            {
                sb.AppendLine(ex.Message);
            }

            return sb.ToString().Trim();
        }
    }

}

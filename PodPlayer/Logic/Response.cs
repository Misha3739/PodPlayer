using System;
using System.Net;

namespace PodPlayer.Logic
{

    internal class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

}

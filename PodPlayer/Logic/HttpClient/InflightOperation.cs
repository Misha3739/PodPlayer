using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace ToDoList.HtppClient
{
    class InflightOperation
    {
        public HttpRequestMessage Request { get; set; }
        public TaskCompletionSource<HttpResponseMessage> FutureResponse { get; set; }
        public ProgressDelegate Progress { get; set; }
        public ByteArrayListStream ResponseBody { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public bool IsCompleted { get; set; }
    }
}

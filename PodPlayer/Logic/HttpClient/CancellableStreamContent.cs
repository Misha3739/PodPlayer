using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ToDoList.HtppClient
{
    sealed class CancellableStreamContent : ProgressStreamContent
    {
        Action onDispose;

        public CancellableStreamContent(Stream source, Action onDispose) : base(source, CancellationToken.None)
        {
            this.onDispose = onDispose;
        }

        protected override void Dispose(bool disposing)
        {
            var disp = Interlocked.Exchange(ref onDispose, null);
            if (disp != null) disp();

            // EVIL HAX: We have to let at least one ReadAsync of the underlying
            // stream fail with OperationCancelledException before we can dispose
            // the base, or else the exception coming out of the ReadAsync will
            // be an ObjectDisposedException from an internal MemoryStream. This isn't
            // the Ideal way to fix this, but #yolo.
            Task.Run(() => base.Dispose(disposing));
        }
    }
}

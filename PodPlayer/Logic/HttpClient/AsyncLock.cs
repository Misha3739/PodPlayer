using System;
using System.Threading.Tasks;
using System.Threading;

namespace ToDoList.HtppClient
{

    public sealed partial class AsyncLock
    {
        readonly SemaphoreSlim m_semaphore;
        readonly Task<IDisposable> m_releaser;

        public static AsyncLock CreateLocked(out IDisposable releaser)
        {
            var asyncLock = new AsyncLock(true);
            releaser = asyncLock.m_releaser.Result;
            return asyncLock;
        }

        AsyncLock(bool isLocked)
        {
            m_semaphore = new SemaphoreSlim(isLocked ? 0 : 1, 1);
            m_releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {
            var wait = m_semaphore.WaitAsync();
            return wait.IsCompleted ?
                m_releaser :
                wait.ContinueWith((_, state) => (IDisposable)state,
                    m_releaser.Result, CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }
    }
}

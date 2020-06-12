using System;
using System.Threading;

namespace InMemoryQueue
{
    internal class LockKey : IDisposable
    {

        private readonly object padlock;

        public LockKey(object locker)
        {
            this.padlock = locker;
        }

        public void Dispose()
        {
            Monitor.Exit(this.padlock);
        }

        public static LockKey GetLock(object lockObject, int defaultTimeoutInMilliseconds)
        {
            if (Monitor.TryEnter(lockObject, defaultTimeoutInMilliseconds))
            {
                return new LockKey(lockObject);
            }
            else
            {
                throw new TimeoutException("Failed to acquire the lock on the object...");
            }
        }
    }
}

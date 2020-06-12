using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace InMemoryQueue
{
    public class ThreadSafeInMemoryQueue<T>
    {
        private long Thresold;

        private long currentCapacity;

        private int defaultTimeoutInMilliseconds = 4000;
    
        private readonly object lockObject = new object();

        private ConcurrentQueue<List<T>> cq = new ConcurrentQueue<List<T>>();

        public ThreadSafeInMemoryQueue()
        {

        }

        public ThreadSafeInMemoryQueue(long capacity)
        {
            using (var lockKey = LockKey.GetLock(this.lockObject, defaultTimeoutInMilliseconds))
            {
                this.Thresold = capacity;
            }
        }
        public void EnqueData(List<T> data)
        {
            using (var lockKey= LockKey.GetLock(this.lockObject,defaultTimeoutInMilliseconds))
            {

                if(currentCapacity+data.Count>Thresold)
                {
                    throw new Exception(QueueErrros.ExceededCapacity);
                }
                currentCapacity = currentCapacity + data.Count;
                cq.Enqueue(data);
            }
        }

        public List<T> DequeData()
        {
            using (var lockKey = LockKey.GetLock(this.lockObject, defaultTimeoutInMilliseconds))
            {
                if(currentCapacity==0)
                {
                    return null;
                }

                while (cq.TryDequeue(out List<T> data))
                {
                    currentCapacity = currentCapacity - data.Count;
                    return data;
                }
            }

            throw new Exception(QueueErrros.FailedToRetrieveData);

        }

        public long GetFreeCapacity()
        {
            using (var lockKey = LockKey.GetLock(this.lockObject, defaultTimeoutInMilliseconds))
            {
                return Thresold - currentCapacity;
            }
        }

    }
}

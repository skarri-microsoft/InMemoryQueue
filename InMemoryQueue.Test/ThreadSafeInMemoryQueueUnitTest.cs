using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMemoryQueue.Test
{
    [TestClass]
    public class ThreadSafeInMemoryQueueUnitTest
    {
        [TestMethod]
        public void TestFreeCapacity()
        {
            var totalCapacity = 10;
            ThreadSafeInMemoryQueue<int> q = new ThreadSafeInMemoryQueue<int>(totalCapacity);

            for (int i = 0; i < 10; i++)
            {
                q.EnqueData(new List<int> { i });
            }

            Assert.IsTrue(q.GetFreeCapacity() == 0, "Expected zero capacity");

            // An action to consume the ConcurrentQueue.
            Action action = () =>
            {
                List<int> data = q.DequeData();

                if (data != null && data.Count > 0)
                {
                    foreach (var i in data)
                    {
                        Console.WriteLine("Item value is: " + i);
                    }
                }
                else
                {
                    Console.WriteLine("No data found on this thread");
                }
            };

            // Start 4 concurrent consuming actions.
            Parallel.Invoke(
                action, action, action, action, action, action, action, action, action, action, action, action, action, action, action, action, action);

            Assert.IsTrue(q.GetFreeCapacity() == totalCapacity, "Expected total capacity");
        }

        [TestMethod]
        public void TestErrorWhenNoCapacity()
        {
            var totalCapacity = 5;
            ThreadSafeInMemoryQueue<int> q = new ThreadSafeInMemoryQueue<int>(totalCapacity);

            Assert.ThrowsException<Exception>(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    q.EnqueData(new List<int> { i });
                }
            }, QueueErrros.ExceededCapacity);
        }

        [TestMethod]
        public void TestEnqueuWithEnoughCapacity()
        {
            var totalCapacity = 5;
            ThreadSafeInMemoryQueue<int> q = new ThreadSafeInMemoryQueue<int>(totalCapacity);
            for (int i = 0; i < totalCapacity; i++)
            {
                q.EnqueData(new List<int> { i });
            }

            List<int> elements = new List<int>();

            while(q.GetFreeCapacity()<totalCapacity)
            {
                elements.AddRange(q.DequeData());
            }

            Assert.IsTrue(elements.Count == totalCapacity, "Expected total elements count same as total capacity");

        }

    }
}

using InMemoryQueue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMemoryQueueSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadSafeInMemoryQueue<int> q = new ThreadSafeInMemoryQueue<int>(10);

            for (int i = 0; i < 10; i++)
            {
                q.EnqueData(new List<int> { i});
            }

            
            // An action to consume the ConcurrentQueue.
            Action action = () =>
            {
                List<int> data = q.DequeData();

                if (data!=null && data.Count > 0)
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
            Console.WriteLine("Current capacity " + q.GetFreeCapacity());
            Console.ReadLine(); 
        }
    }
}

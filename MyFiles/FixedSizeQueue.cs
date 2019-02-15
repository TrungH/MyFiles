using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFiles
{
    class FixedSizeQueue<T>
    {
        readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public int Size { get; private set; }

        public FixedSizeQueue(int size)
        {
            Size = size;
        }

        public void Enqueue(T obj)
        {
            if (queue.Contains(obj)) return;
            queue.Enqueue(obj);

            while (queue.Count > Size)
            {
                T outObj;
                queue.TryDequeue(out outObj);
            }
        }
        public T[] toArray()
        {
            return queue.ToArray();
        }
    }
}

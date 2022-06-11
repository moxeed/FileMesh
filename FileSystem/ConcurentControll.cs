using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace FileSystem
{
    internal class Queue<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(0);

        public void Enqueue(T data)
        {
            _queue.Enqueue(data);
            _semaphoreSlim.Release();
        }

        public async Task<T> Dequeue() {
            await _semaphoreSlim.WaitAsync();
            _queue.TryDequeue(out T result);

            return result;
        }
    }
}

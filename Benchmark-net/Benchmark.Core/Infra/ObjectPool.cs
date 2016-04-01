using System.Collections.Concurrent;

namespace Benchmark.Core
{
    public class ObjectPool<T> where T : class
    {
        private readonly int _capacity;
        private readonly ConcurrentQueue<T> _pool;

        public int Capacity
        {
            get { return _capacity; }
        }

        public int Length
        {
            get { return _pool.Count; }
        }

        public ObjectPool() : this(int.MaxValue) { }

        public ObjectPool(int capacity)
        {
            _capacity = capacity;
            _pool = new ConcurrentQueue<T>();
        }

        public bool TryTake(out T obj)
        {
            obj = null;

            if (_pool.TryDequeue(out obj) && obj != null)
                return true;

            return false;
        }

        public bool Reclaim(ref T obj)
        {
            if (obj == null || _pool.Count >= _capacity)
                return false;

            _pool.Enqueue(obj);
            obj = null;
            return true;
        }

        public void Clear()
        {
            T obj;
            while (_pool.TryDequeue(out obj))
            {
                obj = null;
            }
        }
    }
}

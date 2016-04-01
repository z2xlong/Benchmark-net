using System;
using System.Collections.Concurrent;

namespace Benchmark.Core.Model
{
    public class WeakObjectPool<T> where T : class
    {
        private readonly int _capacity;
        private readonly ConcurrentQueue<WeakReference> _pool;

        public int Capacity
        {
            get { return _capacity; }
        }

        public int Length
        {
            get { return _pool.Count; }
        }

        public WeakObjectPool() : this(int.MaxValue) { }

        public WeakObjectPool(int capacity)
        {
            _capacity = capacity;
            _pool = new ConcurrentQueue<WeakReference>();
        }

        public bool TryTake(out T obj)
        {
            obj = null;
            WeakReference weak;

            if (!_pool.TryDequeue(out weak) || !weak.IsAlive)
                return false;

            obj = weak.Target as T;
            return true;
        }

        public bool Reclaim(ref T obj)
        {
            if (obj == null || _pool.Count >= _capacity)
                return false;

            _pool.Enqueue(new WeakReference(obj));
            return true;
        }

        public void Clear()
        {
            WeakReference weak;
            while (_pool.TryDequeue(out weak))
            {
                weak.Target = null;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace Benchmark.Core
{
    public static class ByteArrayPool
    {
        const int SIZELIMIT = int.MaxValue - 56;

        static readonly int _minSize = 256;
        static readonly int _maxSize, _idxOffset;
        static readonly ObjectPool<byte[]>[] _pools;
        static readonly long _poolSizeThreshold = 8589934592;

        static readonly long[] _gets, _recycles, _hits;
        static long _recycledBytes = 0, _reusedBytes = 0, _missedBytes = 0;

        static ByteArrayPool()
        {
            _maxSize = IntPtr.Size == 8 ? SIZELIMIT : SIZELIMIT / 2;
            _idxOffset = Ln(_minSize);

            _pools = new ObjectPool<byte[]>[Ln(_maxSize) - _idxOffset + 1];
            for (int i = 0; i < _pools.Length; i++)
                _pools[i] = new ObjectPool<byte[]>();

            _gets = new long[_pools.Length];
            _recycles = new long[_pools.Length];
            _hits = new long[_pools.Length];
        }

        public static int CheckMaxSize()
        {
            return _maxSize;
        }

        public static long GetRecycledBytes()
        {
            return _recycledBytes;
        }

        public static long GetMissedBytes()
        {
            return _missedBytes;
        }

        public static long GetReusedBytes()
        {
            return _reusedBytes;
        }

        public static void Reset()
        {
            try { }
            finally
            {
                foreach (var pool in _pools)
                    pool.Clear();

                Interlocked.Exchange(ref _recycledBytes, 0);
                Interlocked.Exchange(ref _reusedBytes, 0);
                Interlocked.Exchange(ref _missedBytes, 0);

                Array.Clear(_gets, 0, _gets.Length);
                Array.Clear(_recycles, 0, _recycles.Length);
                Array.Clear(_hits, 0, _hits.Length);
            }
        }

        public static IEnumerable<ByteArrayPoolMetric> GetMetrics()
        {
            for (int i = 0; i < _pools.Length; i++)
            {
                yield return new ByteArrayPoolMetric(GetBytesLength(i), _pools[i].Length, _gets[i], _recycles[i], _hits[i]);
            }
        }

        public static byte[] Get(int size)
        {
            if (size <= 0)
                return new byte[0];
            if (_poolSizeThreshold <= 0)
                return new byte[size];

            byte[] bytes;
            int index = GetPoolIndex(size);
            var bytePool = _pools[index];

            try { }
            finally
            {
                _gets[index] += 1;

                if (bytePool.TryTake(out bytes) && bytes != null)
                {
                    _hits[index] += 1;
                    Interlocked.Add(ref _reusedBytes, bytes.Length);
                }
                else
                {
                    bytes = new byte[GetBytesLength(index)];
                    Interlocked.Add(ref _missedBytes, bytes.Length);
                }
            }
            return bytes;
        }

        public static bool Recycle(ref byte[] bytes)
        {
            if (bytes == null || _poolSizeThreshold <= 0)
                return false;

            int poolIndex = GetPoolIndex(bytes.Length);
            if (bytes.Length != GetBytesLength(poolIndex))
                return false;

            try { }
            finally
            {
                if (_recycledBytes - _reusedBytes < _poolSizeThreshold)
                {
                    var bytePool = _pools[poolIndex];
                    int length = bytes.Length;
                    Array.Clear(bytes, 0, length);

                    if (bytePool.Reclaim(ref bytes))
                    {
                        _recycles[poolIndex] += 1;
                        Interlocked.Add(ref _recycledBytes, length);
                    }
                }
            }
            return true;
        }

        static int GetPoolIndex(int size)
        {
            if (size <= _minSize)
                return 0;
            if (size >= _maxSize)
                return _pools.Length - 1;

            return Ln(size) - _idxOffset;
        }

        static int Ln(int size)
        {
            return (int)Math.Ceiling(Math.Log(size, 2));
        }

        static int GetBytesLength(int poolIdx)
        {
            if (poolIdx <= 0)
                return _minSize;
            if (poolIdx >= _pools.Length - 1)
                return _maxSize;

            return 1 << (poolIdx + _idxOffset);
        }

    }

    public class ByteArrayPoolMetric
    {
        public int Size { get; private set; }

        public int PoolLength { get; private set; }

        public long Gets { get; private set; }

        public long Recycles { get; private set; }

        public long Hits { get; private set; }

        public ByteArrayPoolMetric(int size, int len, long gets, long recycles, long hits)
        {
            Size = size;
            PoolLength = len;
            Gets = gets;
            Recycles = recycles;
            Hits = hits;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBench;
using Benchmark.Core;
using Benchmark.Core.Enumerator;

namespace Benchmark
{
    public class Enumerator
    {
        private const int Len = 1000;
        private List<int> _list;
        private FlatternTree _tree;
        private Counter _counter;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            _list = new List<int>(Len);
            _tree = new FlatternTree(Len);

            for (int i = 0; i < Len; i++)
            {
                _tree.Set(i, i);
                _list.Add(i);
            }
            _counter = context.GetCounter("EnumeratorCounter");
        }

        [PerfBenchmark(Description = "Testint FlatternTree foreach.", NumberOfIterations = 3, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [CounterMeasurement("EnumeratorCounter")]
        [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void LoopTree()
        {
            for (int i = 0; i < 1000; i++)
            {
                int t = 0;
                foreach (int item in _tree)
                {
                    t += item;
                }
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBench;
using NBench.Util;

namespace Benchmark
{
    public class Protobuf
    {
        private Counter _counter;

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            _counter = context.GetCounter("ProtobufCounter");
        }

        [PerfBenchmark(Description = "Test to ensure that a minimal throughput test can be rapidly executed.",
        NumberOfIterations = 3, RunMode = RunMode.Throughput,
        RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterThroughputAssertion("ProtobufCounter", MustBe.GreaterThan, 10000000.0d)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.ThirtyTwoKb)]
        [GcTotalAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.ExactlyEqualTo, 0.0d)]
        public void SerializeWrapper()
        {

        }

        public void Deserialize()
        {

        }
    }
}

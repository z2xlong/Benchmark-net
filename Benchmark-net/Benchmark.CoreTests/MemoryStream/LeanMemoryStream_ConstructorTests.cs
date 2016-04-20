using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Benchmark.Core;

namespace Benchmark.CoreTests.MemoryStream
{
    public class MemoryStream_ConstructorTests
    {
        [Theory]
        [InlineData(10, -1, int.MaxValue)]
        [InlineData(10, 6, -1)]
        public static void MemoryStream_Ctor_NegativeIndeces(int arraySize, int index, int count)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LeanMemoryStream(new byte[arraySize], index, count));
        }

        [Theory]
        [InlineData(1, 2, 1)]
        [InlineData(7, 8, 2)]
        public static void MemoryStream_Ctor_OutOfRangeIndeces(int arraySize, int index, int count)
        {
            Assert.Throws<ArgumentException>(() => new LeanMemoryStream(new byte[arraySize], index, count));
        }

        [Fact]
        public static void MemoryStream_Ctor_NullArray()
        {
            Assert.Throws<ArgumentNullException>(() => new LeanMemoryStream(null, 5, 2));
        }

        [Fact]
        public static void MemoryStream_Ctor_InvalidCapacities()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LeanMemoryStream(int.MinValue));
            Assert.Throws<ArgumentOutOfRangeException>(() => new LeanMemoryStream(-1));
            Assert.Throws<OutOfMemoryException>(() => new LeanMemoryStream(int.MaxValue));
        }
    }
}

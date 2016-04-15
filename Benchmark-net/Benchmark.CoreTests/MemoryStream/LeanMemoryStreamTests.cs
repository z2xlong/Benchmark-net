using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benchmark.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Tests
{
    [TestClass()]
    public class LeanMemoryStreamTests
    {
        const int DefaultBlockSize = 16384;
        const int DefaultLargeBufferMultiple = 1 << 20;
        const int DefaultMaximumBufferSize = 8 * (1 << 20);
        const int MinSize = 256;

        readonly Random _random = new Random();


        #region Ctor Test
        [TestMethod()]
        public void LeanMemoryStreamTest()
        {
            var stream = new LeanMemoryStream();
            Assert.AreEqual(MinSize, stream.Length);
            Assert.AreEqual(MinSize, stream.Capacity);

            Assert.IsTrue(stream.CanWrite);
            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanRecycledOutside);
        }

        [TestMethod()]
        public void LeanMemoryStreamTest1()
        {
            //TODO Assert.throws with param out of boundaries
            var stream = new LeanMemoryStream(256);
            Assert.AreEqual(MinSize, stream.Length);
            Assert.AreEqual(MinSize, stream.Capacity);

            Assert.IsTrue(stream.CanWrite);
            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanRecycledOutside);
        }

        [TestMethod()]
        public void LeanMemoryStreamTest2()
        {
            byte[] buffer = ByteArrayPool.Get(1024);
            var stream = new LeanMemoryStream(buffer);

            Assert.IsFalse(stream.CanRecycledOutside);
        }

        #endregion

        [TestMethod()]
        public void FlushTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetBufferTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TryGetBufferTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReadByteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SeekTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetLengthTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WriteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WriteByteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WriteToTest()
        {
            Assert.Fail();
        }

        #region test helpers

        private byte[] GetRandomBuffer(int length)
        {
            var buffer = new byte[length];
            for (var i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)_random.Next(byte.MinValue, byte.MaxValue + 1);
            }
            return buffer;

        }

        #endregion
    }
}
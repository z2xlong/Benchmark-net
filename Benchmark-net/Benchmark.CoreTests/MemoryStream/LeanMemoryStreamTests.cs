using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Benchmark.Core;

namespace Benchmark.CoreTests.MemoryStream
{
    public class LeanMemoryStreamTests
    {
        [Fact]
        public static void LeanMemoryStream_Write_BeyondCapacity()
        {
            using (LeanMemoryStream LeanMemoryStream = new LeanMemoryStream())
            {
                long origLength = LeanMemoryStream.Length;
                byte[] bytes = new byte[10];
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = (byte)i;
                int spanPastEnd = 5;
                LeanMemoryStream.Seek(spanPastEnd, SeekOrigin.End);
                Assert.Equal(LeanMemoryStream.Length + spanPastEnd, LeanMemoryStream.Position);

                // Test Write
                LeanMemoryStream.Write(bytes, 0, bytes.Length);
                long pos = LeanMemoryStream.Position;
                Assert.Equal(pos, origLength + spanPastEnd + bytes.Length);
                Assert.Equal(LeanMemoryStream.Length, origLength + spanPastEnd + bytes.Length);

                // Verify bytes were correct.
                LeanMemoryStream.Position = origLength;
                byte[] newData = new byte[bytes.Length + spanPastEnd];
                int n = LeanMemoryStream.Read(newData, 0, newData.Length);
                Assert.Equal(n, newData.Length);
                for (int i = 0; i < spanPastEnd; i++)
                    Assert.Equal(0, newData[i]);
                for (int i = 0; i < bytes.Length; i++)
                    Assert.Equal(bytes[i], newData[i + spanPastEnd]);
            }
        }

        [Fact]
        public static void LeanMemoryStream_WriteByte_BeyondCapacity()
        {
            using (LeanMemoryStream LeanMemoryStream = new LeanMemoryStream())
            {
                long origLength = LeanMemoryStream.Length;
                byte[] bytes = new byte[10];
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = (byte)i;
                int spanPastEnd = 5;
                LeanMemoryStream.Seek(spanPastEnd, SeekOrigin.End);
                Assert.Equal(LeanMemoryStream.Length + spanPastEnd, LeanMemoryStream.Position);

                // Test WriteByte
                origLength = LeanMemoryStream.Length;
                LeanMemoryStream.Position = LeanMemoryStream.Length + spanPastEnd;
                LeanMemoryStream.WriteByte(0x42);
                long expected = origLength + spanPastEnd + 1;
                Assert.Equal(expected, LeanMemoryStream.Position);
                Assert.Equal(expected, LeanMemoryStream.Length);
            }
        }

        [Fact]
        public static void LeanMemoryStream_GetPositionTest_Negative()
        {
            int iArrLen = 100;
            byte[] bArr = new byte[iArrLen];

            using (LeanMemoryStream ms = new LeanMemoryStream(bArr))
            {
                long iCurrentPos = ms.Position;
                for (int i = -1; i > -6; i--)
                {
                    Assert.Throws<ArgumentOutOfRangeException>(() => ms.Position = i);
                    Assert.Equal(ms.Position, iCurrentPos);
                }
            }
        }

        [Fact]
        public static void LeanMemoryStream_LengthTest()
        {
            using (LeanMemoryStream ms2 = new LeanMemoryStream())
            {
                // [] Get the Length when position is at length
                ms2.SetLength(50);
                ms2.Position = 50;
                StreamWriter sw2 = new StreamWriter(ms2);
                for (char c = 'a'; c < 'f'; c++)
                    sw2.Write(c);
                sw2.Flush();
                Assert.Equal(55, ms2.Length);

                // Somewhere in the middle (set the length to be shorter.)
                ms2.SetLength(30);
                Assert.Equal(30, ms2.Length);
                Assert.Equal(30, ms2.Position);

                // Increase the length
                ms2.SetLength(100);
                Assert.Equal(100, ms2.Length);
                Assert.Equal(30, ms2.Position);
            }
        }

        [Fact]
        public static void LeanMemoryStream_LengthTest_Negative()
        {
            using (LeanMemoryStream ms2 = new LeanMemoryStream())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => ms2.SetLength(Int64.MaxValue));
                Assert.Throws<ArgumentOutOfRangeException>(() => ms2.SetLength(-2));
            }
        }

        [Fact]
        public static void LeanMemoryStream_ReadTest_Negative()
        {
            LeanMemoryStream ms2 = new LeanMemoryStream();

            Assert.Throws<ArgumentNullException>(() => ms2.Read(null, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => ms2.Read(new byte[] { 1 }, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => ms2.Read(new byte[] { 1 }, 0, -1));
            Assert.Throws<ArgumentException>(() => ms2.Read(new byte[] { 1 }, 2, 0));
            Assert.Throws<ArgumentException>(() => ms2.Read(new byte[] { 1 }, 0, 2));

            ms2.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ms2.Read(new byte[] { 1 }, 0, 1));
        }

        [Fact]
        public static void LeanMemoryStream_WriteToTests()
        {
            using (LeanMemoryStream ms2 = new LeanMemoryStream())
            {
                byte[] bytArrRet;
                byte[] bytArr = new byte[] { byte.MinValue, byte.MaxValue, 1, 2, 3, 4, 5, 6, 128, 250 };

                // [] Write to FileStream, check the filestream
                ms2.Write(bytArr, 0, bytArr.Length);

                using (LeanMemoryStream readonlyStream = new LeanMemoryStream())
                {
                    ms2.WriteTo(readonlyStream);
                    readonlyStream.Flush();
                    readonlyStream.Position = 0;
                    bytArrRet = new byte[(int)readonlyStream.Length];
                    readonlyStream.Read(bytArrRet, 0, (int)readonlyStream.Length);
                    for (int i = 0; i < bytArr.Length; i++)
                    {
                        Assert.Equal(bytArr[i], bytArrRet[i]);
                    }
                }
            }

            // [] Write to LeanMemoryStream, check the LeanMemoryStream
            using (LeanMemoryStream ms2 = new LeanMemoryStream())
            using (LeanMemoryStream ms3 = new LeanMemoryStream())
            {
                byte[] bytArrRet;
                byte[] bytArr = new byte[] { byte.MinValue, byte.MaxValue, 1, 2, 3, 4, 5, 6, 128, 250 };

                ms2.Write(bytArr, 0, bytArr.Length);
                ms2.WriteTo(ms3);
                ms3.Position = 0;
                bytArrRet = new byte[(int)ms3.Length];
                ms3.Read(bytArrRet, 0, (int)ms3.Length);
                for (int i = 0; i < bytArr.Length; i++)
                {
                    Assert.Equal(bytArr[i], bytArrRet[i]);
                }
            }
        }

        [Fact]
        public static void LeanMemoryStream_WriteToTests_Negative()
        {
            using (LeanMemoryStream ms2 = new LeanMemoryStream())
            {
                Assert.Throws<ArgumentNullException>(() => ms2.WriteTo(null));

                ms2.Write(new byte[] { 1 }, 0, 1);
                LeanMemoryStream readonlyStream = new LeanMemoryStream(new byte[1028], false);
                Assert.Throws<NotSupportedException>(() => ms2.WriteTo(readonlyStream));

                readonlyStream.Dispose();

                // [] Pass in a closed stream
                Assert.Throws<ObjectDisposedException>(() => ms2.WriteTo(readonlyStream));
            }
        }
    }
}

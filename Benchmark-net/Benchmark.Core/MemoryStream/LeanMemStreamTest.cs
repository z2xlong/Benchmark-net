using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Benchmark.Core
{
    public class LeanMemStreamTest
    {
        public static void TestPerf()
        {
            byte[] bytes = new byte[1020000];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)i;

            for (int i = 0; i < 10000; i++)
            {
                MsTest(bytes);
            }
        }

        static void MsTest(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(256))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        static void LeanMsTest(byte[] bytes)
        {
            using (LeanMemoryStream stream = new LeanMemoryStream(256))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}

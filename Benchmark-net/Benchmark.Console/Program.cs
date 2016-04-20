using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benchmark.Core.Enumerator;
using Benchmark.Core;

namespace Benchmark.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            LeanMemStreamTest.TestPerf();
            System.Console.Read();
        }
    }
}

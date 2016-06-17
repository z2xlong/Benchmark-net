using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benchmark.Core.Enumerator;
using Benchmark.Core;
using Benchmark.Core.Model;
using System.Collections;
using Benchmark.Core.Sort;

namespace Benchmark.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //ListRemoveAllTest.TestPerf();
            HotelSortTest.TestPerf();

            System.Console.Read();
        }
    }
}

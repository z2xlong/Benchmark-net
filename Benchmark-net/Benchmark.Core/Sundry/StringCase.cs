using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public sealed class StringCase
    {
        public static void TestPerf()
        {
            var t = "f";
            CodeTimer.Time("Up", 100000, () =>
            {
                Compare(t);
            });
        }

        /// <summary>
        ///  Time Elapsed:   9ms
        ///  CPU Cycles:     29,547,030
        ///  Gen 0:          1
        ///  Gen 1:          0
        ///  Gen 2:          0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static bool Up(string str)
        {
            return str.ToUpper() == "F";
        }

        static bool Low(string str)
        {
            return str.ToLower() == "f";
        }


        // Time Elapsed:   11ms
        // CPU Cycles:     33,848,707
        // Gen 0:          0
        // Gen 1:          0
        // Gen 2:          0
        static bool Compare(string str)
        {
            return string.Compare(str, "F", true) == 0;
        }
    }
}

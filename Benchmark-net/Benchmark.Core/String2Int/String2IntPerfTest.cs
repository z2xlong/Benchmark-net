using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public static class String2IntTest
    {
        public static void TestPerf()
        {
            string ids = "++++, aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00 * 100,,,, ";

            CodeTimer.Time("TestStrEnum", 10000, () =>
            {
                TestStrEnum(ids);
            });
        }

        public static void TestStrEnum(string s)
        {
            StringIntListEnumerator strEnum = new StringIntListEnumerator(s);
            foreach (int i in strEnum)
            {
                //Console.WriteLine(i.ToString());
                //int j = i;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public static class SplitorPerfTest
    {
        public static void TestPerf()
        {
            string ids = "++++, aaa avvv ,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,2    22,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,   56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905   ,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,56456.778,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q   ,56456.778   ,1111,aaa,-222b,+00*100,++++,aaaavvv,1111,aaa,222,b,56 8905,3434,q,   56456.778,1111,aaa,-222b,+00*100,,,, ";


            StringListEnumerator strEnum = new StringListEnumerator(ids, ',');

            CodeTimer.Time("TestStrEnum", 10000, () =>
            {
                TestStrEnum(strEnum);
                //TestBCL(ids);
            });
        }

        public static void TestStrEnum(StringListEnumerator strEnum)
        {
            foreach (string str in strEnum)
            {
                //Console.WriteLine(str);
            }
        }

        public static void TestBCL(string s)
        {
            var ss = s.Trim(new char[] { ',', '，', ' ' }).Split(new char[] { ',', '，' });
        }
    }
}

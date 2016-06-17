using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public class InterTest
    {
        public static void TestPerf()
        {
            CodeTimer.Time("TestStrEnum", 1000, () =>
            {
                TestWithoutIntern(1000);
            });

        }

        static void TestWithIntern(int loop)
        {
            for (int j = 0; j < 100; j++)
                for (int i = 0; i < loop; i++)
                {
                    StringUtility.Intern(i.ToString());
                }
        }

        static void TestWithoutIntern(int loop)
        {
            for (int j = 0; j < 100; j++)
                for (int i = 0; i < loop; i++)
                {
                    var s = i.ToString();
                }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public class ListRemoveAllTest
    {
        public static void TestPerf()
        {
            int len = 10000;
            List<int> list = new List<int>(len);
            for (int i = 0; i < 10000; i++)
                list.Add(i);

            CodeTimer.Time("ListRemoveAllTest", 1000, () =>
            {
                ReCreate(list);
            });
        }

        static List<int> RemoveAll(List<int> list)
        {
            list.RemoveAll(delegate (int i)
            {
                return i % 2 == 0;
            });
            return list;
        }

        static List<int> ReCreate(List<int> list)
        {
            List<int> l = new List<int>();
            foreach (int i in list)
                if (i % 2 != 0)
                    l.Add(i);
            return l;
        }
    }
}

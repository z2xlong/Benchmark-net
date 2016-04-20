using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Enumerator
{
    public class EnumeratorTest
    {
        public static void TestPerf()
        {
            List<int> enumerator = InitList(1000);
            //FlatternTree enumerator = InitTree(100);

            Stopwatch timePerParse = Stopwatch.StartNew();
            for (int i = 0; i < 1; i++)
            {
                int t = 0;
                foreach (int item in enumerator)
                //for (int j = 0; j < enumerator.Count; j++)
                {
                    t += item;
                    //int t = enumerator[j];
                }
                Console.WriteLine(t);
            }
            timePerParse.Stop();

            Console.WriteLine(timePerParse.ElapsedTicks);
            // Console.Read();
        }

        static FlatternTree InitTree(int len)
        {
            var tree = new FlatternTree(len);
            for (int i = 0; i < len; i++)
            {
                // tree.Set(i, new Dummy(i));
                tree.Set(i, i);
            }
            return tree;
        }

        static List<int> InitList(int len)
        {
            var list = new List<int>(len);
            for (int i = 0; i < len; i++)
            {
                // list.Add(new Dummy(i));
                list.Add(i);
            }
            return list;
        }

    }
}

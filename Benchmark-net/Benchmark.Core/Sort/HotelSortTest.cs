using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Sort
{
    public class HotelSortTest
    {
        public static void TestPerf()
        {
            Random r = new Random(100);

            List<LevelOneOrder> _levels = new List<LevelOneOrder>(2)
            {
                LevelOneOrder.IntType,
                LevelOneOrder.IntType,
                LevelOneOrder.DoubleType,
                LevelOneOrder.DoubleType
            };

            List<HotelScore> _list = new List<HotelScore>(20000);
            for (int i = 0; i < 20000; i++)
            {
                _list.Add(new HotelScore()
                {
                    IntType = new List<int>(2) { i, i },
                    DoubleType = new List<double>(2) { i, i },
                });
            }

            CodeTimer.Time("Sort", 1, () =>
            {
                var comparer = new HotelScoreComparer(_levels);
                _list.QuickSort(comparer);
            });
        }
    }
}

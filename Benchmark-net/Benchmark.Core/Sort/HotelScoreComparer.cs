using System.Collections.Generic;

namespace Benchmark.Core.Sort
{
    public class HotelScoreComparer : IComparer<HotelScore>
    {
        readonly IEnumerator<LevelOneOrder> _levelOneOrders;

        public HotelScoreComparer(IEnumerable<LevelOneOrder> levelOneIndex)
        {
            _levelOneOrders = levelOneIndex.GetEnumerator();
        }

        public int Compare(HotelScore x, HotelScore y)
        {
            int compareResult = 0;
            int doubleIndex = 0;
            int intIndex = 0;
            int decimalIndex = 0;

            _levelOneOrders.Reset();
            while (_levelOneOrders.MoveNext())
            {
                switch (_levelOneOrders.Current)
                {
                    case LevelOneOrder.DoubleType:
                        compareResult = x.DoubleType[doubleIndex].CompareTo(y.DoubleType[doubleIndex]);
                        doubleIndex++;
                        break;
                    case LevelOneOrder.DecimalType:
                        compareResult = x.DecimalType[decimalIndex].CompareTo(y.DecimalType[decimalIndex]);
                        decimalIndex++;
                        break;
                    case LevelOneOrder.IntType:
                        compareResult = x.IntType[intIndex].CompareTo(y.IntType[intIndex]);
                        intIndex++;
                        break;
                }

                if (compareResult != 0)
                    return compareResult;
            }

            return compareResult;
        }
    }
}

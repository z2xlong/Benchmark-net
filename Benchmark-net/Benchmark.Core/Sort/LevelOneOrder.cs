using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Sort
{
    public enum LevelOneOrder
    {
        DoubleType,
        DecimalType,
        IntType
    }

    public class HotelScore
    {
        public List<double> DoubleType;
        public List<decimal> DecimalType;
        public List<int> IntType;
    }
}

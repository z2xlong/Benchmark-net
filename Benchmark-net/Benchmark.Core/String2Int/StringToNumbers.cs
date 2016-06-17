using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.String2Int
{
    public class StringToNumbers
    {
        static int _maxTen = int.MaxValue / 10;
        static int _maxMod = int.MaxValue % 10;

        public int MyAtoi(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            int result = 0, i = 0, n = 0;
            bool negative = false;

            for (; i < str.Length && (str[i] == ' ' || str[i] == '\t'); i++) { }

            if (str[i] == '-' || str[i] == '+')
                negative = str[i++] == '-' ? true : false;

            for (; i < str.Length; i++)
            {
                n = str[i] - '0';
                if (n >= 0 && n <= 9)
                {
                    if (result > _maxTen || (result == _maxTen && n > _maxMod))
                        return negative ? int.MinValue : int.MaxValue;

                    result = result * 10 + n;
                }
                else
                    break;
            }

            return negative ? 0 - result : result;
        }
    }
}

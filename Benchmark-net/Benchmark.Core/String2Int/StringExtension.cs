using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core
{
    public static class StringExtension
    {
        public static IEnumerable<string> SplitByComma(this string str)
        {
            int start = 0, end = 0, trim = -1;
            int len = string.IsNullOrWhiteSpace(str) ? 0 : str.Length;

            while (start < len)
            {
                for (; start < len && IsIgnored(str[start]); start++) ;

                for (end = start; end < len && !IsSplitor(str[end]); end++)
                {
                    if (!Char.IsWhiteSpace(str[end]))
                        trim = -1;
                    else if (trim == -1)
                        trim = end;
                }

                if (trim > start)
                    yield return str.Substring(start, trim - start);
                else if (end > start)
                    yield return str.Substring(start, end - start);

                start = end + 1;
                trim = -1;
            }
        }

        static bool IsSplitor(char ch)
        {
            return ch == ',' || ch == '，';
        }

        static bool IsIgnored(char ch)
        {
            return IsSplitor(ch) || Char.IsWhiteSpace(ch);
        }
    }
}

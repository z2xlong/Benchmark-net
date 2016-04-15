using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace Benchmark.Core.Protobuf
{
    public class Serialize
    {
        public static void RunWithWrapper(Dictionary<int, int> dict)
        {
            var wrapper = new DictionaryForSerialization<int, int>(dict);

            using (LeanMemoryStream ms = new LeanMemoryStream())
            {
                Serializer.Serialize(ms, wrapper);
            }
        }

        public static void RunWithPrefixLength(Dictionary<int, int> dict)
        {
            using (LeanMemoryStream ms = new LeanMemoryStream())
            {
                foreach (var kvp in dict)
                {
                    Serializer.SerializeWithLengthPrefix(ms, kvp, PrefixStyle.Base128, 1);
                }
            }
        }
    }
}

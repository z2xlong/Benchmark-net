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

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, wrapper);
            }
        }

        public static void RunWithPrefixLength(Dictionary<int, int> dict)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (var kvp in dict)
                {
                    Serializer.SerializeWithLengthPrefix(ms, kvp, PrefixStyle.Base128, 1);
                }
            }
        }
    }
}

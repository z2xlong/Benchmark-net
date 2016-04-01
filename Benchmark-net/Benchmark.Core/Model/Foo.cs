using ProtoBuf;

namespace Benchmark.Core.Model
{
    [ProtoContract]
    public class Foo
    {
        [ProtoMember(1)]
        public int Id { get; private set; }

        public Foo() { }

        public Foo(int id)
        {
            Id = id;
        }
    }
}

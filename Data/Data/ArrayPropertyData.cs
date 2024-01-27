using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Data.Data;

public class ArrayPropertyData : PropertyData
{
    public JavaType LengthType { get; }

    public ArrayPropertyData(PropertyInfo property, JavaRawAttribute javaAttribute, JavaArrayAttribute arrayAttribute) : base(property, javaAttribute)
    {
        LengthType = arrayAttribute.LengthType;
    }

    protected override async ValueTask<long?> ReadLengthAsync(JavaByteCodeReader reader, CancellationToken cancellationToken) => await ReadLengthAsync(reader, LengthType, cancellationToken);
    protected override async ValueTask WriteLengthAsync(JavaByteCodeWriter writer, long length, CancellationToken cancellationToken) => await WriteLengthAsync(writer, LengthType, length, cancellationToken);
}

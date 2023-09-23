using System.Reflection;
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

    protected override long? ReadLength(JavaByteCodeReader reader) => ReadLength(reader, LengthType);
    protected override void WriteLength(JavaByteCodeWriter writer, long length) => WriteLength(writer, LengthType, length);
}

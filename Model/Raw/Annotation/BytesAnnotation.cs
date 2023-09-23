using Java.Net.Binary;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;

namespace Java.Net.Model.Raw.Annotation;

public abstract class BytesAnnotation<I> : IAnnotation<I> where I : BytesAnnotation<I>
{
    public BytesAnnotation(byte[] info) => Info = info;
    [JavaRaw(Index: 1, IsReaded: false)] public byte[] Info { get; set; }
    public override JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, PropertyData data, object value) => data.Index switch
    {
        1 => writer.WriteCount(Info),
        _ => base.WriteProperty(writer, data, value)
    };
}

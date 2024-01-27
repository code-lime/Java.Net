using Java.Net.Binary;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Model.Raw.Annotation;

public abstract class BytesAnnotation<I> : IAnnotation<I> where I : BytesAnnotation<I>
{
    public BytesAnnotation(byte[] info) => Info = info;
    [JavaRaw(Index: 1, IsReaded: false)] public byte[] Info { get; set; }
    public override async ValueTask<JavaByteCodeWriter> WritePropertyAsync(JavaByteCodeWriter writer, PropertyData data, object? value, CancellationToken cancellationToken) => data.Index switch
    {
        1 => await writer.WriteCountAsync(Info, cancellationToken),
        _ => await base.WritePropertyAsync(writer, data, value, cancellationToken)
    };
}

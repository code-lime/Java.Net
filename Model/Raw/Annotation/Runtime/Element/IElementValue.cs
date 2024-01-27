using System;
using System.Threading;
using System.Threading.Tasks;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public interface IElementValue : IRaw
{
    ElementTag Tag { get; set; }
    [InstanceOfTag]
    public static async ValueTask<IElementValue> InstanceOfTag(
        [TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader,
        CancellationToken cancellationToken)
    {
        ElementTag tag;
        IElementValue value = (tag = (ElementTag)await reader.ReadByteAsync(cancellationToken)) switch
        {
            ElementTag.Byte => new ConstantValue(),
            ElementTag.Char => new ConstantValue(),
            ElementTag.Double => new ConstantValue(),
            ElementTag.Float => new ConstantValue(),
            ElementTag.Integer => new ConstantValue(),
            ElementTag.Long => new ConstantValue(),
            ElementTag.Short => new ConstantValue(),
            ElementTag.Boolean => new ConstantValue(),
            ElementTag.String => new ConstantValue(),

            ElementTag.Enum => new EnumValue(),
            ElementTag.Class => new ClassValue(),
            ElementTag.Annotation => new AnnotationValue(),
            ElementTag.Array => new ArrayValue(),

            _ => throw new ArgumentException($"ElementTag '{tag}' not supported!")
        };
        value.Tag = tag;
        return value;
    }
}

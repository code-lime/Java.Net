using System;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation.Runtime;
using Java.Net.Model.Raw.Base;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public interface IAnnotation : IRaw
{
    ushort NameIndex { get; set; }
    string Name { get; set; }
    AnnotationType Type { get; }

    [InstanceOfTag]
    public static IAnnotation InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader, [TagType(TagTypeAttribute.Tag.Handle)] JavaClass handle)
    {
        ushort name_index = reader.ReadUShort();
        string name = (handle.Constants[name_index] as Utf8Constant).Value;
        uint size = reader.ReadUInt();
        AnnotationType type;
        IAnnotation attribute = (type = Enum.TryParse(name, out type) ? type : AnnotationType.Unknown) switch
        {
            AnnotationType.ConstantValue => new ConstantValueAnnotation(),
            AnnotationType.Code => new CodeAnnotation(),
            AnnotationType.StackMapTable => new StackMapTableAnnotation(),
            AnnotationType.Exceptions => new ExceptionsAnnotation(),
            AnnotationType.InnerClasses => new InnerClassesAnnotation(),
            AnnotationType.EnclosingMethod => new EnclosingMethodAnnotation(),
            AnnotationType.Synthetic => new SyntheticAnnotation(),
            AnnotationType.Signature => new SignatureAnnotation(),
            AnnotationType.SourceFile => new SourceFileAnnotation(),
            AnnotationType.SourceDebugExtension => new SourceDebugExtensionAnnotation(reader.ReadCount(size)),
            AnnotationType.LineNumberTable => new LineNumberTableAnnotation(),
            AnnotationType.LocalVariableTable => new LocalVariableTableAnnotation(),
            AnnotationType.LocalVariableTypeTable => new LocalVariableTypeTableAnnotation(),
            AnnotationType.Deprecated => new DeprecatedAnnotation(),
            AnnotationType.RuntimeVisibleAnnotations => new RuntimeVisibleAnnotationsAnnotation(),
            AnnotationType.RuntimeInvisibleAnnotations => new RuntimeInvisibleAnnotationsAnnotation(),
            AnnotationType.RuntimeVisibleParameterAnnotations => new RuntimeVisibleParameterAnnotationsAnnotation(),
            AnnotationType.RuntimeInvisibleParameterAnnotations => new RuntimeInvisibleParameterAnnotationsAnnotation(),
            AnnotationType.AnnotationDefault => new AnnotationDefaultAnnotation(),
            AnnotationType.BootstrapMethods => new BootstrapMethodsAnnotation(),
            _ => new UnknownAnnotation(reader.ReadCount(size))
        };
        attribute.NameIndex = name_index;
        return attribute;
    }
}
public abstract class IAnnotation<I> : BaseRaw<I>, IAnnotation where I : IAnnotation<I>
{
    public ushort NameIndex { get; set; }
    public string Name
    {
        get => (Handle.Constants[NameIndex] as Utf8Constant).Value;
        set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public abstract AnnotationType Type { get; }
    public override IRaw SetHandle(JavaClass handle)
    {
        IRaw dat = base.SetHandle(handle);
        if (NameIndex == 0 && Type != AnnotationType.Unknown) Name = Type.ToString();
        return dat;
    }
    public sealed override JavaByteCodeWriter Write(JavaByteCodeWriter writer)
    {
        using System.IO.MemoryStream stream = new System.IO.MemoryStream();
        JavaByteCodeWriter out_writer = base.Write(new JavaByteCodeWriter(stream));

        return writer
            .WriteUShort(NameIndex)
            .WriteUInt((uint)stream.Length)
            .Append(out_writer);
    }
}

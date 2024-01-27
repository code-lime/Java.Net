using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class LocalVariableTypeTableAnnotation : IAnnotation<LocalVariableTypeTableAnnotation>
{
    public class VariableType : BaseRaw<VariableType>
    {
        [JavaRaw] public ushort StartPC { get; set; }
        [JavaRaw] public ushort Length { get; set; }
        [JavaRaw] public ushort NameIndex { get; set; }
        public Utf8Constant Name
        {
            get => (Utf8Constant)Handle.Constants[NameIndex];
            set => NameIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort SignatureIndex { get; set; }
        public Utf8Constant Signature
        {
            get => (Utf8Constant)Handle.Constants[SignatureIndex];
            set => SignatureIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort Index { get; set; }
    }
    public override AnnotationType Type => AnnotationType.LocalVariableTypeTable;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<VariableType> LocalVariableTypeTable { get; set; } = null!;
}

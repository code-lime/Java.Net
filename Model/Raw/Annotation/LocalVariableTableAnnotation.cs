using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class LocalVariableTableAnnotation : IAnnotation<LocalVariableTableAnnotation>
{
    public sealed class Variable : BaseRaw<Variable>
    {
        [JavaRaw] public ushort StartPC { get; set; }
        [JavaRaw] public ushort Length { get; set; }
        [JavaRaw] public ushort NameIndex { get; set; }
        public Utf8Constant Name
        {
            get => (Utf8Constant)Handle.Constants[NameIndex];
            set => NameIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort DescriptorIndex { get; set; }
        public Utf8Constant Descriptor
        {
            get => (Utf8Constant)Handle.Constants[DescriptorIndex];
            set => DescriptorIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort Index { get; set; }
    }
    public override AnnotationType Type => AnnotationType.LocalVariableTable;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Variable> LocalVariableTable { get; set; } = null!;
}

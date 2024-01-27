using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation;

public sealed class LineNumberTableAnnotation : IAnnotation<LineNumberTableAnnotation>
{
    public sealed class Line : BaseRaw<Line>
    {
        [JavaRaw] public ushort StartPC { get; set; }
        [JavaRaw] public ushort LineNumber { get; set; }
    }
    public override AnnotationType Type => AnnotationType.LineNumberTable;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Line> LineNumberTable { get; set; } = null!;
}

using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class CodeAnnotation : IAnnotation<CodeAnnotation>
{
    public sealed class Exception : BaseRaw<Exception>
    {
        [JavaRaw] public ushort StartPC { get; set; }
        [JavaRaw] public ushort EndPC { get; set; }
        [JavaRaw] public ushort HandlerPC { get; set; }
        [JavaRaw] public ushort CatchTypeIndex { get; set; }
        public ClassConstant CatchType
        {
            get => (ClassConstant)Handle.Constants[CatchTypeIndex];
            set => CatchTypeIndex = Handle.OfConstant(value);
        }
    }

    public override AnnotationType Type => AnnotationType.Code;
    [JavaRaw] public ushort MaxStack { get; set; }
    [JavaRaw] public ushort MaxLocals { get; set; }
    [JavaRaw][JavaArray(JavaType.UInt)] public byte[] Code { get; set; } = null!;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Exception> ExceptionTable { get; set; } = null!;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<IAnnotation> Attributes { get; set; } = null!;
}

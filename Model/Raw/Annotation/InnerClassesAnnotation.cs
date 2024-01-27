using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class InnerClassesAnnotation : IAnnotation<InnerClassesAnnotation>
{
    public override AnnotationType Type => AnnotationType.InnerClasses;
    public sealed class ClassInfo : BaseRaw<ClassInfo>
    {
        [JavaRaw] public ushort InnerClassInfoIndex { get; set; }
        public ClassConstant InnerClassInfo
        {
            get => (ClassConstant)Handle.Constants[InnerClassInfoIndex];
            set => InnerClassInfoIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort OuterClassInfoIndex { get; set; }
        public ClassConstant OuterClassInfo
        {
            get => (ClassConstant)Handle.Constants[OuterClassInfoIndex];
            set => OuterClassInfoIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort InnerNameIndex { get; set; }
        public Utf8Constant InnerName
        {
            get => (Utf8Constant)Handle.Constants[InnerNameIndex];
            set => InnerNameIndex = Handle.OfConstant(value);
        }
        [JavaRaw(JavaType.UShort)] public Flags.AccessClass InnerClassAccessFlags { get; set; }
    }
    [JavaRaw][JavaArray(JavaType.UShort)] public List<ClassInfo> Classes { get; set; } = null!;
}

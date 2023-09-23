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
            get => Handle.Constants[InnerClassInfoIndex] as ClassConstant;
            set => InnerClassInfoIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort OuterClassInfoIndex { get; set; }
        public ClassConstant OuterClassInfo
        {
            get => Handle.Constants[OuterClassInfoIndex] as ClassConstant;
            set => OuterClassInfoIndex = Handle.OfConstant(value);
        }
        [JavaRaw] public ushort InnerNameIndex { get; set; }
        public Utf8Constant InnerName
        {
            get => Handle.Constants[InnerNameIndex] as Utf8Constant;
            set => InnerNameIndex = Handle.OfConstant(value);
        }
        [JavaRaw(JavaType.UShort)] public Flags.AccessClass InnerClassAccessFlags { get; set; }
    }
    [JavaRaw][JavaArray(JavaType.UShort)] public List<ClassInfo> Classes { get; set; }
}

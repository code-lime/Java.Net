using System.Collections.Generic;
using System.Linq;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class BootstrapMethodsAnnotation : IAnnotation<BootstrapMethodsAnnotation>
{
    public sealed class Bootstrap : BaseRaw<Bootstrap>
    {
        [JavaRaw] public ushort MethodRefIndex { get; set; }
        public MethodHandleConstant MethodRef
        {
            get => Handle.Constants[MethodRefIndex] as MethodHandleConstant;
            set => MethodRefIndex = Handle.OfConstant(value);
        }
        [JavaRaw][JavaArray(JavaType.UShort)] public List<ushort> ArgumentsIndexes { get; set; }
        public IConstant[] Arguments
        {
            get => ArgumentsIndexes.Select(v => Handle.Constants[v]).ToArray();
            set => ArgumentsIndexes = value.Select(v => Handle.OfConstant(v)).ToList();
        }

        public Bootstrap DeepClone(JavaClass handle) => Create(handle, v =>
        {
            v.MethodRef = MethodRef.DeepClone(handle);
            v.Arguments = Arguments.Select(j => j.DeepClone(handle)).ToArray();
        });
    }
    public override AnnotationType Type => AnnotationType.BootstrapMethods;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Bootstrap> Methods { get; set; }
}

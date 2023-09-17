using Java.Net.Data.Signature;
using Java.Net.Flags;
using Java.Net.Model;
using Java.Net.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java.Net.Data
{
    public sealed class JavaMethodData
    {
        public sealed class MethodArgument
        {
            public ITypeSignature Type { get; set; }
            public List<JavaAnnotationData> Annotations { get; set; } = new List<JavaAnnotationData>();
        }

        public AccessMethod Flags { get; set; }
        public string Name { get; set; }

        public List<FormalTypeParameter> FormalTypeParameters { get; set; } = new List<FormalTypeParameter>();
        public List<MethodArgument> Arguments { get; set; } = new List<MethodArgument>();
        public Descriptor.IReturnType ReturnType { get; set; }
        public List<IThrowsSignature> Throws { get; set; } = new List<IThrowsSignature>();
        public JavaMethodBodyData Body { get; set; }

        public List<JavaAnnotationData> Annotations { get; set; } = new List<JavaAnnotationData>();
        public Data.JavaAnnotationData.ElementValue DefaultValue { get; set; }

        public JavaMethod ToJava(JavaClass handle, TypeVariableReader variable)
        {
            variable = variable.Add(FormalTypeParameters);
            MethodTypeSignature signature = new MethodTypeSignature()
            {
                FormalTypeParameters = FormalTypeParameters.Count == 0 ? null : new FormalTypeParameters() { FormalTypeParameter = FormalTypeParameters.ToArray() },
                TypeSignature = Arguments.Select(v => v.Type).ToArray(),
                ReturnType = ReturnType,
                ThrowsSignature = Throws.ToArray()
            };
            return JavaMethod.Create(handle, Name, signature.ToDescriptor(variable).DescriptorFormat, dat =>
            {
                dat.Flags = Flags;
                if (Annotations.Count > 0) dat.Attributes.Add(RuntimeInvisibleAnnotationsAttribute.Create(handle, v =>
                {
                    v.Annotations = Annotations.Select(_v => _v.ToJava(handle)).ToList();
                }));
                dat.Attributes.Add(SignatureAttribute.Create(handle, v =>
                {
                    v.Signature = Utf8Constant.Create(handle, v => v.Value = signature.SignatureFormat);
                }));
                if (Arguments.Any(v => v.Annotations.Any())) dat.Attributes.Add(RuntimeInvisibleParameterAnnotationsAttribute.Create(handle, v =>
                {
                    v.ParameterAnnotations = Arguments.Select(_v => Model.Attribute.Runtime.ParameterAnnotation.Create(handle, __v => __v.Annotations = _v.Annotations.Select(___v => ___v.ToJava(handle)).ToList())).ToList();
                }));
                if (DefaultValue == null) dat.Attributes.Add(AnnotationDefaultAttribute.Create(handle, v =>
                {
                    v.DefaultValue = DefaultValue.ToJava(handle);
                }));
            });
        }
    }
}

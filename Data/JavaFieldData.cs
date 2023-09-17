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
    public sealed class JavaFieldData
    {
        public AccessField Flags { get; set; }
        public string Name { get; set; }

        public FieldTypeSignature FieldType { get; set; }
        public List<JavaAnnotationData> Annotations { get; set; } = new List<JavaAnnotationData>();
        public Java.Net.Data.JavaAnnotationData.ElementValue DefaultValue { get; set; }

        public JavaField ToJava(JavaClass handle, TypeVariableReader variable) => JavaField.Create(handle, Name, FieldType.ToDescriptor(variable).DescriptorFormat, dat =>
        {
            dat.Flags = Flags;
            if (Annotations.Count > 0) dat.Attributes.Add(RuntimeInvisibleAnnotationsAttribute.Create(handle, v =>
            {
                v.Annotations = Annotations.Select(_v => _v.ToJava(handle)).ToList();
            }));
            if (DefaultValue == null) dat.Attributes.Add(AnnotationDefaultAttribute.Create(handle, v =>
            {
                v.DefaultValue = DefaultValue.ToJava(handle);
            }));
            dat.Attributes.Add(SignatureAttribute.Create(handle, v =>
            {
                v.Signature = Utf8Constant.Create(handle, _v => _v.Value = FieldType.SignatureFormat);
            }));
        });
    }
}

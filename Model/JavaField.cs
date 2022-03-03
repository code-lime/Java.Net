using Java.Net.Flags;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Java.Net.Model
{
    public class JavaField : IJava<JavaField>, INet<Mono.Cecil.FieldDefinition, Mono.Cecil.TypeDefinition>, IFlag<AccessField>, IAttributes
    {
        public static JavaField Create(JavaClass handle, string name, string descriptor, Action<JavaField> apply = null)
        {
            JavaField dat = new JavaField
            {
                Handle = handle,
                Name = name,
                Descriptor = descriptor,
                Attributes = new List<JavaAttribute>()
            };
            apply.Invoke(dat);
            return dat;
        }

        [IJava(IJavaType.UShort)] public AccessField Flags { get; set; }
        [IJava] public ushort NameIndex { get; set; }
        public string Name
        {
            get => (Handle.Constants[NameIndex] as Utf8Constant).Value;
            set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        [IJava] public ushort DescriptorIndex { get; set; }
        public string Descriptor
        {
            get => (Handle.Constants[DescriptorIndex] as Utf8Constant).Value;
            set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        [IJava][IJavaArray] public List<JavaAttribute> Attributes { get; set; }

        public IEnumerable<FieldDefinition> ToNet(TypeDefinition parent)
        {
            FieldAttributes fieldAttributes = 0;
            if (Flags.HasFlag(AccessField.FINAL)) fieldAttributes |= FieldAttributes.InitOnly;
            if (Flags.HasFlag(AccessField.PRIVATE)) fieldAttributes |= FieldAttributes.Private;
            if (Flags.HasFlag(AccessField.PROTECTED)) fieldAttributes |= FieldAttributes.Family;
            if (Flags.HasFlag(AccessField.PUBLIC)) fieldAttributes |= FieldAttributes.Public;
            if (Flags.HasFlag(AccessField.STATIC)) fieldAttributes |= FieldAttributes.Static;
            string desc = Descriptor;
            FieldDefinition field = new FieldDefinition(Name, fieldAttributes, TypeDescriptor.Read(ref desc, parent, parent.Module.Assembly));
            parent.Fields.Add(field);
            yield return field;
        }

        public override string ToString() => Name + Descriptor;
    }
}

using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Data.Descriptor;
using Java.Net.Data.Descriptor.Field;
using Java.Net.Flags;
using Java.Net.Model.Net;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Constant;
using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace Java.Net.Model.Raw.Base;

public class JavaField : BaseRaw<JavaField>, INet<FieldDefinition, TypeDefinition>, IFlag<AccessField>, IAnnotations
{
    public static JavaField Create(JavaClass handle, string name, string descriptor, Action<JavaField>? apply = null)
    {
        JavaField dat = new JavaField
        {
            Handle = handle,
            Name = name,
            Descriptor = descriptor,
            Annotations = new List<IAnnotation>()
        };
        apply?.Invoke(dat);
        return dat;
    }

    [JavaRaw(JavaType.UShort)] public AccessField Flags { get; set; }
    [JavaRaw] public ushort NameIndex { get; set; }
    public string Name
    {
        get => (Handle.Constants[NameIndex] as Utf8Constant)!.Value;
        set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    [JavaRaw] public ushort DescriptorIndex { get; set; }
    public string Descriptor
    {
        get => (Handle.Constants[DescriptorIndex] as Utf8Constant)!.Value;
        set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public IFieldDescriptor FieldDescriptor
    {
        get => IDescriptor.TryParse<IFieldDescriptor>(Descriptor, true);
        set => Descriptor = value.DescriptorFormat;
    }
    [JavaRaw][JavaArray] public List<IAnnotation> Annotations { get; set; } = null!;

    public IEnumerable<FieldDefinition> ToNet(TypeDefinition parent)
    {
        throw new NotSupportedException();
        /*FieldAttributes fieldAttributes = 0;
        if (Flags.HasFlag(AccessField.FINAL)) fieldAttributes |= FieldAttributes.InitOnly;
        if (Flags.HasFlag(AccessField.PRIVATE)) fieldAttributes |= FieldAttributes.Private;
        if (Flags.HasFlag(AccessField.PROTECTED)) fieldAttributes |= FieldAttributes.Family;
        if (Flags.HasFlag(AccessField.PUBLIC)) fieldAttributes |= FieldAttributes.Public;
        if (Flags.HasFlag(AccessField.STATIC)) fieldAttributes |= FieldAttributes.Static;
        string desc = Descriptor;
        FieldDefinition field = new FieldDefinition(Name, fieldAttributes, TypeDescriptor.Read(ref desc, parent, parent.Module.Assembly));
        parent.Fields.Add(field);
        yield return field;*/
    }

    public override string ToString() => Name + Descriptor;
}

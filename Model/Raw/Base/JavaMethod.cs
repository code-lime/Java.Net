using Java.Net.Code;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Data.Descriptor;
using Java.Net.Data.Descriptor.Method;
using Java.Net.Flags;
using Java.Net.Model.Net;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Constant;
using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace Java.Net.Model.Raw.Base;

public class JavaMethod : BaseRaw<JavaMethod>, INet<MethodDefinition, TypeDefinition>, IFlag<AccessMethod>, IAnnotations
{
    public static JavaMethod Create(JavaClass handle, string name, string descriptor, Action<JavaMethod>? apply = null)
    {
        JavaMethod dat = new JavaMethod
        {
            Handle = handle,
            Name = name,
            Descriptor = descriptor,
            Annotations = new List<IAnnotation>()
        };
        apply?.Invoke(dat);
        return dat;
    }

    [JavaRaw(JavaType.UShort)] public AccessMethod Flags { get; set; }
    [JavaRaw] public ushort NameIndex { get; set; }
    public string Name
    {
        get => ((Utf8Constant)Handle.Constants[NameIndex]).Value;
        set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    [JavaRaw] public ushort DescriptorIndex { get; set; }
    public string Descriptor
    {
        get => ((Utf8Constant)Handle.Constants[DescriptorIndex]).Value;
        set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    [JavaRaw][JavaArray] public List<IAnnotation> Annotations { get; set; } = null!;

    private JavaMethodBody? body = null;
    public JavaMethodBody Body => body ??= new JavaMethodBody(this);
    public MethodDescriptor MethodDescriptor
    {
        get => IDescriptor.TryParse<MethodDescriptor>(Descriptor, true);
        set => Descriptor = value.DescriptorFormat;
    }

    public IEnumerable<MethodDefinition> ToNet(TypeDefinition parent)
    {
        throw new NotSupportedException();
        /*string descriptor = Descriptor;
        MethodAttributes methodAttributes = MethodAttributes.HideBySig;
        if (Flags.HasFlag(AccessMethod.ABSTRACT)) methodAttributes |= MethodAttributes.Abstract;
        if (Flags.HasFlag(AccessMethod.FINAL)) methodAttributes |= MethodAttributes.Final;
        if (Flags.HasFlag(AccessMethod.PRIVATE)) methodAttributes |= MethodAttributes.Private;
        if (Flags.HasFlag(AccessMethod.PROTECTED)) methodAttributes |= MethodAttributes.Family;
        if (Flags.HasFlag(AccessMethod.PUBLIC)) methodAttributes |= MethodAttributes.Public;
        if (Flags.HasFlag(AccessMethod.STATIC)) methodAttributes |= MethodAttributes.Static;

        string name = Name;
        switch (name)
        {
            case "<init>":
                methodAttributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                name = ".ctor";
                break;
            case "<clinit>":
                methodAttributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                name = ".cctor";
                break;
        }
        MethodDefinition method = new MethodDefinition(name, methodAttributes, parent.Module.TypeSystem.Object);
        (TypeReference[] args, TypeReference ret) = MethodDescriptor.Parse(ref descriptor, method, parent.Module.Assembly);
        method.ReturnType = ret;
        Attribute.CodeAttribute code = (Attribute.CodeAttribute)Attributes.FirstOrDefault(v => v is Attribute.CodeAttribute);
        Attribute.LocalVariableTableAttribute local = (Attribute.LocalVariableTableAttribute)code?.Attributes?.FirstOrDefault(v => v is Attribute.LocalVariableTableAttribute);
        int length = args.Length;
        int offset = Flags.HasFlag(AccessMethod.STATIC) ? 0 : 1;

        int offset_local = length - (local?.LocalVariableTable?.Count ?? 0);

        for (int i = 0; i < length; i++)
        {
            TypeReference arg = args[i];
            string arg_name = local == null ? string.Empty : (local.LocalVariableTable.FirstOrDefault(v => v.Index == (i + offset))?.Name?.Value ?? string.Empty);
            method.Parameters.Add(new ParameterDefinition(arg_name, ParameterAttributes.None, arg));
        }
        parent.Methods.Add(method);
        if (Flags.HasFlag(AccessMethod.SYNCHRONIZED))
        {
            CustomAttributeArgument attributeArgument = new CustomAttributeArgument(
                parent.Module.ImportReference(typeof(MethodImplOptions)),
                MethodImplOptions.Synchronized
            );
            CustomAttribute attribute = new CustomAttribute(
                parent.Module.ImportReference(typeof(MethodImplAttribute).GetConstructor(new Type[] { typeof(MethodImplOptions) }))
            );
            attribute.ConstructorArguments.Add(attributeArgument);
            method.CustomAttributes.Add(attribute);
        }
        if (Flags.HasFlag(AccessMethod.VARARGS))
        {
            CustomAttribute attribute = new CustomAttribute(
                parent.Module.ImportReference(typeof(ParamArrayAttribute).GetConstructor(new Type[] { }))
            );
            method.CustomAttributes.Add(attribute);
        }

        yield return method;
        Annotation.Apply(Attributes, method, parent.Module.Assembly);
        yield return method;*/
    }

    public override string ToString() => Name + Descriptor;
}
